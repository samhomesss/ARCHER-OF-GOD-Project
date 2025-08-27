using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Projectile2D : MonoBehaviour
{
    [SerializeField] private float speed = 14f;
    [SerializeField] private float damage = 15f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private bool pierce = false;
    [SerializeField] private int pierceCount = 1;


    [Header("Owner Control")]
    [SerializeField] private string ownerTag; // set by spawner


    private Rigidbody2D _rb;
    private float _timer;


    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }


    public void Fire(Vector2 dir, string owner)
    {
        ownerTag = owner;
        _rb.linearVelocity = dir.normalized * speed;
        _timer = 0f;
    }
    public void FireWithVelocity(Vector2 initialVelocity, string owner)
    {
        ownerTag = owner;        
        _rb.linearVelocity = initialVelocity;
        _timer = 0f;             
    }


    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= lifetime) Destroy(gameObject);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
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
                if (pierceCount <= 0) Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else if (!other.isTrigger || !other.CompareTag("Wall"))
        {
            // hit environment
            Destroy(gameObject);
        }
    }
}