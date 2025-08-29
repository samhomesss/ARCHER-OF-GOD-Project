using UnityEngine;
using DG.Tweening;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Projectile2D : MonoBehaviour
{
    [Header("Kinetics")]
    [SerializeField] private float speed = 14f;
    [SerializeField] private float gravityScale = 1f;
    [SerializeField] private float arcHeight = 0f; // set 0 for straight shots


    [Header("Damage")]
    [SerializeField] private float damage = 15f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private bool pierce = false;
    [SerializeField] private int pierceCount = 1;


    [Header("Owner")]
    [SerializeField] private string ownerTag; // set by spawner


    [Header("Visual Alignment")]
    [SerializeField] private bool alignToVelocity = true; // keep the sprite pointing along travel dir
    [SerializeField] private float alignLerp = 20f; // higher = snappier
    [SerializeField] private float visualAngleOffsetDeg = -90f; // sprite faces UP by default ¡æ -90¡Æ


    private Rigidbody2D _rb;
    private float _timer;
    private Tween _moveTween;

    public float Speed => speed;
    public float ArcHeight => arcHeight;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = gravityScale;
    }


    public void Fire(Vector2 dir, string owner)
    {
        ownerTag = owner;
        _rb.gravityScale = gravityScale;
        Vector2 v = dir.normalized * speed;
        v.y += arcHeight; // keep 0 for laser-straight
        _rb.linearVelocity = v;
        _timer = 0f;
        AlignInstant(v);
    }


    public void FireWithVelocity(Vector2 initialVelocity, string owner)
    {
        ownerTag = owner;
        _rb.gravityScale = gravityScale;
        _rb.linearVelocity = initialVelocity;
        _timer = 0f;
        AlignInstant(initialVelocity);
    }

    public void FireArc(Vector2 targetPos, float flightTime, float jumpPower, string owner)
    {
        ownerTag = owner;
        _timer = 0f;
        _rb.gravityScale = 0f;
        _rb.linearVelocity = Vector2.zero;


        Vector2 prevPos = _rb.position;
        _moveTween?.Kill();
        _moveTween = _rb.DOJump(targetPos, jumpPower, 1, flightTime)
        .SetEase(Ease.Linear)
        .OnUpdate(() =>
        {
            Vector2 current = _rb.position;
            Vector2 delta = current - prevPos;
            if (delta.sqrMagnitude > 0.0001f)
            {
                float ang = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg + visualAngleOffsetDeg;
                transform.rotation = Quaternion.Euler(0f, 0f, ang);
            }
            prevPos = current;
        })
        .OnComplete(() => Destroy(gameObject));
    }


    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= lifetime)
        {
            _moveTween?.Kill();
            Destroy(gameObject);
        }
    }


    void FixedUpdate()
    {
        if (!alignToVelocity) return;
        var v = _rb.linearVelocity;
        if (v.sqrMagnitude > 0.0001f)
        {
            float ang = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg + visualAngleOffsetDeg;
            var target = Quaternion.Euler(0f, 0f, ang);
            transform.rotation = Quaternion.Lerp(transform.rotation, target, 1f - Mathf.Exp(-alignLerp * Time.fixedDeltaTime));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Projectile2D>() != null) return; // ignore projectiles
        if (other.attachedRigidbody && other.attachedRigidbody.gameObject.CompareTag(ownerTag)) return; // ignore owner


        var dmg = other.GetComponentInParent<IDamageable2D>();
        if (dmg != null)
        {
            Vector2 hitPoint = other.ClosestPoint(transform.position);
            Vector2 hitNormal = (Vector2)transform.position - hitPoint;
            dmg.TakeDamage(damage, hitPoint, hitNormal, gameObject);


            if (!pierce || (--pierceCount <= 0))
            {
                _moveTween?.Kill();
                Destroy(gameObject);
            }
        }
        else if (!other.isTrigger || !other.CompareTag("Wall"))
        {
            _moveTween?.Kill();
            Destroy(gameObject);
        }
    }


    private void AlignInstant(Vector2 v)
    {
        if (v.sqrMagnitude < 0.0001f) return;
        float ang = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg + visualAngleOffsetDeg;
        transform.rotation = Quaternion.Euler(0f, 0f, ang);
    }
}