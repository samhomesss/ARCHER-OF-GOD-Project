using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BowShooter2D))]
[RequireComponent(typeof(EnemyAutoAttack2D))]
public class EnemyController2D : MonoBehaviour
{
    public enum BotState { Move, Pause, Skill }


    [Header("Refs")]
    [SerializeField] private Transform target; // Player


    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float moveDistance = 3f;
    [SerializeField] private Transform leftLimit;
    [SerializeField] private Transform rightLimit;


    [Header("Rhythm")]
    [SerializeField] private float pauseBeforeSkill = 0.35f;


    private Rigidbody2D _rb;
    private BowShooter2D _shooter;
    private SkillBase2D[] _skills;
    private BotState _state;

    private Facing2D _facing;
    private EnemyAutoAttack2D _autoAttack;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _shooter = GetComponent<BowShooter2D>();
        _skills = GetComponents<SkillBase2D>();
        _facing = GetComponent<Facing2D>();
        _autoAttack = GetComponent<EnemyAutoAttack2D>();
    }

    private void MoveTowards(Vector2 dest)
    {
        float minX = leftLimit ? leftLimit.position.x : float.NegativeInfinity;
        float maxX = rightLimit ? rightLimit.position.x : float.PositiveInfinity;
        dest.x = Mathf.Clamp(dest.x, minX, maxX);

        Vector2 dir = new Vector2(dest.x - transform.position.x, 0f).normalized;

        if ((dir.x < 0f && transform.position.x <= minX) ||
            (dir.x > 0f && transform.position.x >= maxX))
        {
            _rb.linearVelocity = Vector2.zero;
            return;
        }

        _rb.linearVelocity = new Vector2(dir.x * moveSpeed, 0f);

        if (_facing) _facing.FaceByVelocityX(-dir.x);
    }

    private void FaceTargetX()
    {
        if (!_facing || !target) return;
        _facing.Face(target.position.x < transform.position.x ? +1 : -1);
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
            float dir = Random.value < 0.5f ? -1f : 1f;
            float targetX = transform.position.x + dir * moveDistance;
            if (leftLimit && rightLimit)
            {
                float minX = Mathf.Min(leftLimit.position.x, rightLimit.position.x);
                float maxX = Mathf.Max(leftLimit.position.x, rightLimit.position.x);
                targetX = Mathf.Clamp(targetX, minX, maxX);
            }
            Vector2 moveTarget = new Vector2(targetX, transform.position.y);
            while (Mathf.Abs(transform.position.x - moveTarget.x) > 0.05f)
            {
                MoveTowards(moveTarget);
                yield return null;
            }
            _rb.linearVelocity = Vector2.zero;


            // brief PAUSE before a skill (AutoAttackController2D will keep auto-shooting between skills)
            _state = BotState.Pause;
            FaceTargetX();
            yield return new WaitForSeconds(pauseBeforeSkill);


            // SKILL phase (attack once after moving)
            _state = BotState.Skill;
            if (_autoAttack)
                _autoAttack.Fire();
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
