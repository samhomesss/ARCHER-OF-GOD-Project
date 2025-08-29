using UnityEngine;
using System.Collections;

public class BowShooter2D : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private Projectile2D projectilePrefab;
    [SerializeField] private float fireCooldown = 0.25f;
    [SerializeField] private float recoilKick = 0f;


    private float _lastFire;
    private Rigidbody2D _rb;
    private float damageMultiplier = 1f;
    private Coroutine damageRoutine;
    public float DamageMultiplier => damageMultiplier;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }


    public bool CanFire => Time.time >= _lastFire + fireCooldown;


    public void Fire(Vector2 direction, string ownerTag, float stunDuration = 0f)
    {
        Fire(direction, ownerTag, projectilePrefab, stunDuration);
    }


    public void Fire(Vector2 direction, string ownerTag, Projectile2D prefabOverride, float stunDuration = 0f)
    {
        if (!CanFire || prefabOverride == null || firePoint == null) return;


        var proj = Instantiate(prefabOverride, firePoint.position, Quaternion.identity);
        proj.Damage *= damageMultiplier;
        proj.StunDuration = stunDuration;
        proj.Fire(direction, ownerTag);


        if (_rb && recoilKick > 0f)
            _rb.AddForce(-direction.normalized * recoilKick, ForceMode2D.Impulse);


        _lastFire = Time.time;
    }

    public void ApplyDamageMultiplier(float multiplier, float duration)
    {
        if (damageRoutine != null) StopCoroutine(damageRoutine);
        damageMultiplier = multiplier;
        damageRoutine = StartCoroutine(ResetDamageMultiplier(duration));
    }

    private IEnumerator ResetDamageMultiplier(float duration)
    {
        yield return new WaitForSeconds(duration);
        damageMultiplier = 1f;
        damageRoutine = null;
    }
}