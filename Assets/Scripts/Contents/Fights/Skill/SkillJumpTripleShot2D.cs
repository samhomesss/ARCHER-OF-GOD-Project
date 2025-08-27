using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SkillJumpTripleShot2D : SkillBase2D
{
    [Header("Jump")]
    [SerializeField] private float jumpHeight = 2.0f;
    [SerializeField] private float jumpUpTime = 0.12f;
    [SerializeField] private float jumpDownTime = 0.14f;

    [Header("Shooting")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private Projectile2D projectilePrefab;
    [SerializeField] private int shots = 3;
    [SerializeField] private float shotInterval = 0.07f;
    [SerializeField] private float straightSpeed = 22f;  // 직선 속도
    [SerializeField] private float targetHeightOffset = 0.0f; // 머리나 몸통을 노리고 싶으면 조절

    private Transform _target;
    private Rigidbody2D _rb;
    private Tween _jumpTween;

    void Awake()
    {
        if (!firePoint)
        {
            var t = transform.Find("FirePoint");
            if (t) firePoint = t;
        }
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        string desiredTag = CompareTag("Player") ? "Enemy" : "Player";
        var go = GameObject.FindGameObjectWithTag(desiredTag);
        if (go) _target = go.transform;
    }

    protected override bool Cast()
    {
        if (!firePoint || !projectilePrefab || !_target) return false;

        BeginCast();
        StartCoroutine(JumpAndShoot());
        return true;
    }

    private IEnumerator JumpAndShoot()
    {
        Vector2 basePos = _rb.position;

        // 점프(상승→하강)
        _jumpTween?.Kill();
        _jumpTween = DOTween.Sequence()
            .Append(_rb.DOMoveY(basePos.y + jumpHeight, jumpUpTime).SetEase(Ease.OutQuad))
            .Append(_rb.DOMoveY(basePos.y, jumpDownTime).SetEase(Ease.InQuad));

        // 3연속 직사
        for (int i = 0; i < shots; i++)
        {
            Vector2 targetPos = (Vector2)_target.position + Vector2.up * targetHeightOffset;
            Vector2 dir = (targetPos - (Vector2)firePoint.position).normalized;

            var proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            var rb = proj.GetComponent<Rigidbody2D>();
            if (rb) rb.gravityScale = 0f; // 포물선 X, 완전 직사
            proj.FireWithVelocity(dir * straightSpeed, gameObject.tag);

            if (i < shots - 1)
                yield return new WaitForSeconds(shotInterval);
        }

        // 점프 끝날 때까지 대기
        yield return _jumpTween.WaitForCompletion();
        EndCast();
    }
}
