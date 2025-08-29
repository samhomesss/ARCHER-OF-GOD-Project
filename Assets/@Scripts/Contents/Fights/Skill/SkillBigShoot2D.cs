using UnityEngine;

public class SkillBigShoot2D : SkillBase2D
{
    [SerializeField] private BowShooter2D shooter;
    [SerializeField] private Projectile2D projectilePrefab;
    [SerializeField] private float stunDuration = 1f; 
  

    private Facing2D _facing;

    void Awake()
    {
        _facing = GetComponent<Facing2D>();
    }

    protected override bool Cast()
    {
        if (shooter == null) shooter = GetComponent<BowShooter2D>();
        if (shooter == null || projectilePrefab == null) return false;
        if (!IsReady) return false;

        BeginCast();
        Vector2 forward = _facing ? _facing.Forward : (Vector2)transform.right;

        if (CompareTag("Player"))
        {
            forward = -forward;
        }

        shooter.Fire(forward, gameObject.tag, projectilePrefab, stunDuration);
        EndCast();
        return true;
    }

}