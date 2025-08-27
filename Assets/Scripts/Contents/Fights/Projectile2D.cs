using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Projectile2D : MonoBehaviour
{
    [SerializeField] private float speed = 14f;
    [SerializeField] private float damage = 15f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private bool pierce = false;
    [SerializeField] private int pierceCount = 1;
    [SerializeField] private float gravityScale = 1f;

    [Header("Owner Control")]
    [SerializeField] private string ownerTag; // set by spawner


    private Rigidbody2D _rb;
    private float _timer;
    private Tween _moveTween;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = gravityScale;
    }


    public void Fire(Vector2 dir, string owner)
    {
        ownerTag = owner;
        _rb.gravityScale = gravityScale;
        _rb.linearVelocity = dir.normalized * speed;
        _timer = 0f;
    }
    public void FireWithVelocity(Vector2 initialVelocity, string owner)
    {
        ownerTag = owner;
        _rb.gravityScale = gravityScale;
        _rb.linearVelocity = initialVelocity;
        _timer = 0f;
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
                    transform.right = delta.normalized;
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

        if (_rb && _rb.linearVelocity.sqrMagnitude > 0.01f)
            transform.right = _rb.linearVelocity.normalized;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Projectile2D>() != null)
            return; // ignore other projectiles
        if (other.attachedRigidbody && other.attachedRigidbody.gameObject.CompareTag(ownerTag))
            return; // ignore hitting owner


        var dmg = other.GetComponentInParent<IDamageable2D>();
        if (dmg != null)
        {
            Vector2 hitPoint = other.ClosestPoint(transform.position);
            Vector2 hitNormal = (Vector2)transform.position - hitPoint;
            dmg.TakeDamage(damage, hitPoint, hitNormal, gameObject);


            if (pierce)
            {
                pierceCount--;
                if (pierceCount <= 0)
                {
                    _moveTween?.Kill();
                    Destroy(gameObject);
                }
            }
            else
            {
                _moveTween?.Kill();
                Destroy(gameObject);
            }
        }
        else if (!other.isTrigger || !other.CompareTag("Wall"))
        {
            // hit environment
            _moveTween?.Kill();
            Destroy(gameObject);
        }
    }
}