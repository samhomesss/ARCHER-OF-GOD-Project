using UnityEngine;


public class BowShooter2D : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private Projectile2D projectilePrefab;
    [SerializeField] private float fireCooldown = 0.25f;
    [SerializeField] private float recoilKick = 0f;


    private float _lastFire;
    private Rigidbody2D _rb;


    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }


    public bool CanFire => Time.time >= _lastFire + fireCooldown;


    public void Fire(Vector2 direction, string ownerTag, float stunDuration = 0f)
    {
        if (!CanFire || projectilePrefab == null || firePoint == null) return;


        var proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        // DO NOT pre-rotate here; projectile will align itself by velocity
        proj.StunDuration = stunDuration;
        proj.Fire(direction, ownerTag);


        if (_rb && recoilKick > 0f)
            _rb.AddForce(-direction.normalized * recoilKick, ForceMode2D.Impulse);


        _lastFire = Time.time;
    }
}