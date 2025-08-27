using UnityEngine;


[DisallowMultipleComponent]
public class AutoAttackController2D : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target; // assign at runtime if null


    [Header("Attack Timing")]
    [SerializeField] private float interval = 0.5f;


    [Header("Ballistic Settings")]
    [SerializeField] private Projectile2D projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float gravityScale = 1.5f; // effective gravity for projectile
    [SerializeField] private float minFlightTime = 0.4f;
    [SerializeField] private float maxFlightTime = 1.1f;
    [SerializeField] private float preferHorizSpeed = 10f;


    private float _nextTime;
    private SkillBase2D[] _skills;
    private Facing2D _facing;  // ★ 추가

    void Awake()
    {
        _skills = GetComponents<SkillBase2D>();
        _facing = GetComponent<Facing2D>(); // ★
    }


    void Start()
    {
        if (target == null)
        {
            // If this is the player, target enemy by tag; if enemy, target player by tag
            string desiredTag = CompareTag("Player") ? "Enemy" : "Player";
            var t = GameObject.FindGameObjectWithTag(desiredTag);
            if (t) target = t.transform;
        }
        _nextTime = Time.time + interval;
    }


    void Update()
    {
        if (Time.time < _nextTime) return;
        if (IsCastingAnySkill()) return;
        if (!target || !projectilePrefab || !firePoint) return;


        ShootBallistic(target.position);
        _nextTime = Time.time + interval;
    }

    private bool IsCastingAnySkill()
    {
        if (_skills == null) return false;
        for (int i = 0; i < _skills.Length; i++)
        {
            if (_skills[i] && _skills[i].IsCasting) return true;
        }
        return false;
    }


    private void ShootBallistic(Vector2 targetPos)
    {
        Vector2 p0 = firePoint.position;
        Vector2 delta = targetPos - p0;
        float g = Mathf.Abs(Physics2D.gravity.y) * gravityScale;
        float distX = Mathf.Max(0.01f, Mathf.Abs(delta.x));

        float t = Mathf.Clamp(distX / Mathf.Max(0.01f, preferHorizSpeed), minFlightTime, maxFlightTime);
        float vx = delta.x / t;
        float vy = (delta.y + 0.5f * g * t * t) / t;

        // ★ 발사 직전 시각만 스케일 플립
        if (_facing) _facing.FaceByVelocityX(vx);

        var proj = Instantiate(projectilePrefab, p0, Quaternion.identity);
        var rb = proj.GetComponent<Rigidbody2D>();
        if (rb) rb.gravityScale = gravityScale;

        proj.FireWithVelocity(new Vector2(vx, vy), gameObject.tag);
    }
}