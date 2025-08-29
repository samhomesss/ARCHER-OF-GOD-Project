using UnityEngine;

public class SkillPiercingLine2D : SkillBase2D
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private Projectile2D piercingProjectilePrefab; 
    [SerializeField] private float lineSpeed = 28f;
    [SerializeField] private int shots = 1;                
    [SerializeField] private float interval = 0.06f;
    [SerializeField] private float targetHeightOffset = 0f;

    private Transform _target;
    private BowShooter2D _shooter;

    void Awake()
    {
        if (!firePoint)
        {
            var t = transform.Find("FirePoint");
            if (t) firePoint = t;
        }

        _shooter = GetComponent<BowShooter2D>();
    }

    void Start()
    {
        string desiredTag = CompareTag("Player") ? "Enemy" : "Player";
        var go = GameObject.FindGameObjectWithTag(desiredTag);
        if (go) _target = go.transform;
    }

    protected override bool Cast()
    {
        if (!piercingProjectilePrefab || !_target || !firePoint) return false;
        Debug.Log("관통샷하는 중");
        BeginCast();
        StartCoroutine(FireRoutine());
        return true;
    }

    private System.Collections.IEnumerator FireRoutine()
    {
        for (int i = 0; i < shots; i++)
        {
            if (!_target)
                break;

            Vector2 targetPos = (Vector2)_target.position + Vector2.up * targetHeightOffset;
            Vector2 dir = (targetPos - (Vector2)firePoint.position).normalized;

            var proj = Object.Instantiate(piercingProjectilePrefab, firePoint.position, Quaternion.identity);
            if (_shooter) proj.Damage *= _shooter.DamageMultiplier;
            var rb = proj.GetComponent<Rigidbody2D>();
            if (rb) rb.gravityScale = 0f; 
            proj.FireWithVelocity(dir * lineSpeed, gameObject.tag);

            if (i < shots - 1)
                yield return new WaitForSeconds(interval);
        }
        EndCast();
    }
}
