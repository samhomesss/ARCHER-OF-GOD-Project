using UnityEngine;

public class EnemyAutoAttack2D : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target; 

    [Header("Attack Timing")]
    [SerializeField] private float interval = 0.6f;

    [Header("Ballistic Settings")]
    [SerializeField] private Projectile2D projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float gravityScale = 1.5f;
    [SerializeField] private float minFlightTime = 0.4f;
    [SerializeField] private float maxFlightTime = 1.1f;
    [SerializeField] private float preferHorizSpeed = 10f;

    private float _nextTime;
    private Animator _anim;

    void Start()
    {
        _anim = GetComponentInChildren<Animator>();
        if (target == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) target = p.transform;
        }
        _nextTime = Time.time + interval;
    }

    public void Fire()
    {
        if (Time.time < _nextTime) return;
        if (!target || !projectilePrefab || !firePoint) return;

        FireBallistic();
        _nextTime = Time.time + interval;
    }

    private void FireBallistic()
    {
        Vector2 p0 = firePoint.position;
        Vector2 p1 = target.position;
        Vector2 delta = p1 - p0;

        float g = Mathf.Abs(Physics2D.gravity.y) * gravityScale;
        float dist = Mathf.Max(0.01f, Mathf.Abs(delta.x));
        float t = Mathf.Clamp(dist / Mathf.Max(0.01f, preferHorizSpeed), minFlightTime, maxFlightTime);

        float vx = delta.x / t;
        float vy = (delta.y + 0.5f * g * t * t) / t;
        float jumpPower = (vy * vy) / (2f * g);


        if (_anim) _anim.Play("attack");
        var proj = Instantiate(projectilePrefab, p0, Quaternion.identity);
        proj.FireArc(p1, t, jumpPower, gameObject.tag);

        if (Mathf.Abs(vx) > 0.01f && TryGetComponent(out Facing2D facing))
        {
            facing.FaceByVelocityX(-vx); 
        }
    }
}
