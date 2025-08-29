using UnityEngine;
using System.Collections;

public class SkillVerticalRainStrike2D : SkillBase2D
{
    [Header("Upward Shot (player)")]
    [SerializeField] private Transform firePoint;           // 비워두면 Awake에서 자동 탐색
    [SerializeField] private Projectile2D projectilePrefab;
    [SerializeField] private float upwardSpeed = 20f;       // 위로 쏘는 속도
    [SerializeField] private float upwardGravityScale = 1.5f;
    [SerializeField] private float preDelay = 0.12f;        // 위로 쏘고 나서 비 시작까지 대기

    [Header("Vertical Rain (over enemy head)")]
    [SerializeField] private int drops = 8;                 // 낙하 화살 개수
    [SerializeField] private float startHeight = 6f;        // 적 머리 위 높이
    [SerializeField] private float dropSpeed = 18f;         // 수직 낙하 속도(초기속도)
    [SerializeField] private float dropGravityScale = 1.5f; // 낙하 감 (0으로 하면 완전 직선)
    [SerializeField] private float interval = 0.055f;       // 낙하 간 간격
    [SerializeField] private float xJitter = 0.0f;          // 0이면 정확히 머리 위, 값 주면 좌우 퍼짐

    private Transform _target;

    void Awake()
    {
        if (!firePoint)
        {
            var t = transform.Find("FirePoint");
            if (t) firePoint = t;
        }
    }

    void Start()
    {
        string desiredTag = CompareTag("Player") ? "Enemy" : "Player";
        var go = GameObject.FindGameObjectWithTag(desiredTag);
        if (go) _target = go.transform;
    }

    protected override bool Cast()
    {
        if (!projectilePrefab || !firePoint || !_target) return false;
        BeginCast();
        StartCoroutine(Routine());
        return true;
    }

    private IEnumerator Routine()
    {
        // 1) 플레이어 위로 수직 1발
        {
            var upProj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            var rb = upProj.GetComponent<Rigidbody2D>();
            if (rb) rb.gravityScale = upwardGravityScale;
            upProj.FireWithVelocity(new Vector2(0f, upwardSpeed), gameObject.tag);
        }

        yield return new WaitForSeconds(preDelay);

        if (!_target)
        {
            EndCast();
            yield break;
        }

        // 2) 적 머리 위에서 수직 낙하
        Vector2 center = _target.position;
        for (int i = 0; i < drops; i++)
        {
            float x = center.x + (xJitter > 0f ? Random.Range(-xJitter, xJitter) : 0f);
            Vector2 start = new Vector2(x, center.y + startHeight);

            var proj = Instantiate(projectilePrefab, start, Quaternion.identity);
            var rb = proj.GetComponent<Rigidbody2D>();
            if (rb) rb.gravityScale = dropGravityScale;

            // 수직 낙하(포물선 X) → 초기 속도만 아래 방향으로
            proj.FireWithVelocity(new Vector2(0f, -dropSpeed), gameObject.tag);

            if (i < drops - 1)
                yield return new WaitForSeconds(interval);
        }

        EndCast();
    }
}
