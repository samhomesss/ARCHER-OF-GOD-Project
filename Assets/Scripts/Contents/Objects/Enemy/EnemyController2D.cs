using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BowShooter2D))]
public class BotController2D : MonoBehaviour
{
    public enum BotState { Move, Attack, Skill }


    [Header("Refs")]
    [SerializeField] private Transform target; // assign Player transform


    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float moveRadius = 6f;
    [SerializeField] private float moveDuration = 1.75f;


    [Header("Attack")]
    [SerializeField] private float attackDuration = 1.2f;
    [SerializeField] private float aimJitterDegrees = 5f;


    [Header("Skill Phase")]
    [SerializeField] private float skillPause = 0.4f; // small pause before skill


    private Rigidbody2D _rb;
    private BowShooter2D _shooter;
    private SkillBase2D[] _skills;
    private BotState _state;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _shooter = GetComponent<BowShooter2D>();
        _skills = GetComponents<SkillBase2D>();
    }


    void Start()
    {
        if (target == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) target = p.transform;
        }
        StartCoroutine(StateLoop());
    }


    private IEnumerator StateLoop()
    {
        while (true)
        {
            // MOVE
            _state = BotState.Move;
            float endTime = Time.time + moveDuration;
            Vector2 moveTarget = (Vector2)transform.position + Random.insideUnitCircle.normalized * moveRadius;
            while (Time.time < endTime)
            {
                MoveTowards(moveTarget);
                AimAtTarget();
                yield return null;
            }
            _rb.linearVelocity = Vector2.zero;


            // ATTACK
            _state = BotState.Attack;
            endTime = Time.time + attackDuration;
            while (Time.time < endTime)
            {
                AimAtTarget(true);
                _shooter.Fire(transform.right, gameObject.tag);
                yield return null;
            }


            // SKILL
            _state = BotState.Skill;
            _rb.linearVelocity = Vector2.zero;
            AimAtTarget();
            yield return new WaitForSeconds(skillPause);
            TryRandomSkill();


            // small beat
            yield return new WaitForSeconds(0.25f);
        }
    }
    private void MoveTowards(Vector2 dest)
    {
        Vector2 dir = (dest - (Vector2)transform.position).normalized;
        _rb.linearVelocity = dir * moveSpeed;
    }


    private void AimAtTarget(bool jitter = false)
    {
        if (!target) return;
        Vector2 dir = (Vector2)target.position - (Vector2)transform.position;
        if (jitter) dir = Quaternion.Euler(0, 0, Random.Range(-aimJitterDegrees, aimJitterDegrees)) * dir;
        float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, ang);
    }


    private void TryRandomSkill()
    {
        if (_skills == null || _skills.Length == 0) return;
        int start = Random.Range(0, _skills.Length);
        for (int i = 0; i < _skills.Length; i++)
        {
            int idx = (start + i) % _skills.Length;
            if (_skills[idx].IsReady)
            {
                _skills[idx].TryCast();
                return;
            }
        }
    }
}