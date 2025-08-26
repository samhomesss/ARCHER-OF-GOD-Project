using UnityEngine;
using System.Collections;


public class SkillVolley2D : SkillBase2D
{
    [SerializeField] private BowShooter2D shooter;
    [SerializeField] private int waves = 3;
    [SerializeField] private int arrowsPerWave = 6;
    [SerializeField] private float waveInterval = 0.15f;
    [SerializeField] private float coneDegrees = 45f;


    protected override bool Cast()
    {
        if (shooter == null) shooter = GetComponent<BowShooter2D>();
        if (shooter == null) return false;
        if (!IsReady) return false;
        StartCoroutine(VolleyRoutine());
        return true;
    }


    private IEnumerator VolleyRoutine()
    {
        Vector2 forward = transform.right;
        for (int w = 0; w < waves; w++)
        {
            for (int i = 0; i < arrowsPerWave; i++)
            {
                float offset = Random.Range(-coneDegrees * 0.5f, coneDegrees * 0.5f);
                Vector2 dir = Quaternion.Euler(0, 0, offset) * forward;
                shooter.Fire(dir, gameObject.tag);
            }
            yield return new WaitForSeconds(waveInterval);
        }
    }
}