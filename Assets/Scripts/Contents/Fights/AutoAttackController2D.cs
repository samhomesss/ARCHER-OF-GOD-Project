using UnityEngine;

[DisallowMultipleComponent]
public class AutoAttackController2D : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Attack Timing")]
    [SerializeField] private float interval = 0.5f;

    [Header("Projectile Settings")]
    [SerializeField] private Projectile2D projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float targetHeightOffset = 2f;

    private float _nextTime;
    private SkillBase2D[] _skills;
    private Facing2D _facing;
    private Rigidbody2D _rb;

    void Awake()
    {
        _skills = GetComponents<SkillBase2D>();
        _facing = GetComponent<Facing2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (target == null)
        {
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
        if (_rb && Mathf.Abs(_rb.linearVelocity.x) > 0.01f) return;

        Shoot(target.position);
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

    public Transform Target => target;

    private void Shoot(Vector2 targetPos)
    {
        targetPos += Vector2.up * targetHeightOffset;
        Vector2 dir = targetPos - (Vector2)firePoint.position;
        if (_facing) _facing.FaceByVelocityX(dir.x);
        var proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        proj.Fire(dir, gameObject.tag);
    }
}