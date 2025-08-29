using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BowShooter2D))]
[RequireComponent(typeof(EnemyAutoAttack2D))]
public class EnemyController2D : MonoBehaviour
{
    public enum BotState { Move, Pause, Skill }


    [Header("Refs")]
    [SerializeField] private Transform target; 


    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float moveDistance = 3f;
    [SerializeField] private Transform leftLimit;
    [SerializeField] private Transform rightLimit;

    public Transform LeftLimit => leftLimit;
    public Transform RightLimit => rightLimit;


    [Header("Rhythm")]
    [SerializeField] private float pauseBeforeSkill = 0.35f;


    private Rigidbody2D _rb;
    private BowShooter2D _shooter;
    private SkillBase2D[] _skills;
    private BotState _state;

    private Facing2D _facing;
    private EnemyAutoAttack2D _autoAttack;
    private Animator _anim;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _shooter = GetComponent<BowShooter2D>();
        _skills = GetComponents<SkillBase2D>();
        _facing = GetComponent<Facing2D>();
        _autoAttack = GetComponent<EnemyAutoAttack2D>();
        _anim = GetComponentInChildren<Animator>();
    }

    private void MoveTowards(Vector2 dest)
    {
        float minX = leftLimit ? leftLimit.position.x : float.NegativeInfinity;
        float maxX = rightLimit ? rightLimit.position.x : float.PositiveInfinity;

        float currentX = transform.position.x;
        if (currentX < minX || currentX > maxX)
        {
            float nearestX = Mathf.Abs(currentX - minX) < Mathf.Abs(currentX - maxX) ? minX : maxX;
            _rb.position = new Vector2(nearestX, _rb.position.y);
            currentX = nearestX;
        }

        dest.x = Mathf.Clamp(dest.x, minX, maxX);

        Vector2 currentPos = _rb.position;
        Vector2 targetPos = new Vector2(dest.x, currentPos.y);

        Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, moveSpeed * Time.deltaTime);
        _rb.MovePosition(newPos);

        if (Vector2.Distance(newPos, targetPos) < 0.001f)
            _rb.position = targetPos;

        if (_facing) _facing.FaceByVelocityX(newPos.x - currentPos.x);

        if (_anim)
        {
            var info = _anim.GetCurrentAnimatorStateInfo(0);
            bool busy = (info.IsName("attack") && info.normalizedTime < 1f) || info.IsName("die") || info.IsName("victory");
            if (!busy)
                _anim.Play("walk");
        }
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
            if (_anim)
            {
                var info = _anim.GetCurrentAnimatorStateInfo(0);
                bool busy = (info.IsName("attack") && info.normalizedTime < 1f) || info.IsName("die") || info.IsName("victory");
                if (!busy)
                    _anim.Play("idle");
            }



            _state = BotState.Pause;
            FaceTargetX();
            yield return new WaitForSeconds(pauseBeforeSkill);


            _state = BotState.Skill;
            if (_autoAttack)
                _autoAttack.Fire();
            TryRandomReadySkill();

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
