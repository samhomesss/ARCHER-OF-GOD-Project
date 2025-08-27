using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BowShooter2D))]
public class EnemyController2D : MonoBehaviour
{
    public enum BotState { Move, Pause, Skill }


    [Header("Refs")]
    [SerializeField] private Transform target; // Player


    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float moveRadius = 6f;
    [SerializeField] private float moveDuration = 1.6f;


    [Header("Rhythm")]
    [SerializeField] private float pauseBeforeSkill = 0.35f;


    private Rigidbody2D _rb;
    private BowShooter2D _shooter;
    private SkillBase2D[] _skills;
    private BotState _state;


    private Facing2D _facing;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _shooter = GetComponent<BowShooter2D>();
        _skills = GetComponents<SkillBase2D>();
        _facing = GetComponent<Facing2D>(); 
    }

    private void MoveTowards(Vector2 dest)
    {
        Vector2 dir = new Vector2(dest.x - transform.position.x, 0f).normalized;
        _rb.linearVelocity = new Vector2(dir.x * moveSpeed, 0f);

        if (_facing) _facing.FaceByVelocityX(dir.x);
    }

    private void FaceTargetX()
    {
        if (!_facing || !target) return;
        _facing.Face(target.position.x < transform.position.x ? -1 : 1);
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
            // MOVE phase
            _state = BotState.Move;
            float endTime = Time.time + moveDuration;
            float offsetX = Random.Range(-moveRadius, moveRadius);
            Vector2 moveTarget = new Vector2(transform.position.x + offsetX, transform.position.y);
            while (Time.time < endTime)
            {
                MoveTowards(moveTarget);
                yield return null;
            }
            _rb.linearVelocity = Vector2.zero;


            // brief PAUSE before a skill (AutoAttackController2D will keep auto-shooting between skills)
            _state = BotState.Pause;
            FaceTargetX();
            yield return new WaitForSeconds(pauseBeforeSkill);


            // SKILL phase (pick first ready)
            _state = BotState.Skill;
            TryRandomReadySkill();


            // short beat to avoid too frequent skill spam
            yield return new WaitForSeconds(0.25f);
        }
    }

    private void TryRandomReadySkill()
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