using UnityEngine;
using System.Collections;

public class SkillVerticalRainStrike2D : SkillBase2D
{
    [Header("Upward Shot (player)")]
    [SerializeField] private Transform firePoint;           // ����θ� Awake���� �ڵ� Ž��
    [SerializeField] private Projectile2D projectilePrefab;
    [SerializeField] private float upwardSpeed = 20f;       // ���� ��� �ӵ�
    [SerializeField] private float upwardGravityScale = 1.5f;
    [SerializeField] private float preDelay = 0.12f;        // ���� ��� ���� �� ���۱��� ���

    [Header("Vertical Rain (over enemy head)")]
    [SerializeField] private int drops = 8;                 // ���� ȭ�� ����
    [SerializeField] private float startHeight = 6f;        // �� �Ӹ� �� ����
    [SerializeField] private float dropSpeed = 18f;         // ���� ���� �ӵ�(�ʱ�ӵ�)
    [SerializeField] private float dropGravityScale = 1.5f; // ���� �� (0���� �ϸ� ���� ����)
    [SerializeField] private float interval = 0.055f;       // ���� �� ����
    [SerializeField] private float xJitter = 0.0f;          // 0�̸� ��Ȯ�� �Ӹ� ��, �� �ָ� �¿� ����

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
        // 1) �÷��̾� ���� ���� 1��
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

        // 2) �� �Ӹ� ������ ���� ����
        Vector2 center = _target.position;
        for (int i = 0; i < drops; i++)
        {
            float x = center.x + (xJitter > 0f ? Random.Range(-xJitter, xJitter) : 0f);
            Vector2 start = new Vector2(x, center.y + startHeight);

            var proj = Instantiate(projectilePrefab, start, Quaternion.identity);
            var rb = proj.GetComponent<Rigidbody2D>();
            if (rb) rb.gravityScale = dropGravityScale;

            // ���� ����(������ X) �� �ʱ� �ӵ��� �Ʒ� ��������
            proj.FireWithVelocity(new Vector2(0f, -dropSpeed), gameObject.tag);

            if (i < drops - 1)
                yield return new WaitForSeconds(interval);
        }

        EndCast();
    }
}
