using UnityEngine;

public class SkillVolley2D : SkillBase2D
{
    [SerializeField] private BowShooter2D shooter;
    [SerializeField] private float stunDuration = 1f;
    //[SerializeField] private int waves = 3;
    //[SerializeField] private int arrowsPerWave = 6;
    //[SerializeField] private float waveInterval = 0.15f;
    //[SerializeField] private float coneDegrees = 45f;

    private Facing2D _facing;

    void Awake()
    {
        _facing = GetComponent<Facing2D>();
    }

    protected override bool Cast()
    {
        if (shooter == null) shooter = GetComponent<BowShooter2D>();
        if (shooter == null) return false;
        if (!IsReady) return false;

        BeginCast();
        Vector2 forward = _facing ? _facing.Forward : (Vector2)transform.right;
        shooter.Fire(-forward, gameObject.tag, stunDuration);
        EndCast();
        return true;
    }

    //protected override bool Cast()
    //{
    //    if (shooter == null) shooter = GetComponent<BowShooter2D>();
    //    if (shooter == null) return false;
    //    if (!IsReady) return false;

    //    BeginCast();
    //    StartCoroutine(VolleyRoutine());
    //    return true;
    //}

    //private System.Collections.IEnumerator VolleyRoutine()
    //{
    //    Vector2 forward = _facing ? _facing.Forward : (Vector2)transform.right;

    //    for (int w = 0; w < waves; w++)
    //    {
    //        for (int i = 0; i < arrowsPerWave; i++)
    //        {
    //            float offset = Random.Range(-coneDegrees * 0.5f, coneDegrees * 0.5f);
    //            Vector2 dir = Quaternion.Euler(0, 0, offset) * -forward;
    //            shooter.Fire(dir, gameObject.tag);
    //        }
    //        yield return new WaitForSeconds(waveInterval);
    //    }
    //    EndCast();
    //}
}