using UnityEngine;


public class SkillMultiShot2D : SkillBase2D
{
    [SerializeField] private BowShooter2D shooter;
    [SerializeField] private int arrowCount = 3;
    [SerializeField] private float spreadDegrees = 20f;


    protected override bool Cast()
    {
        if (shooter == null) shooter = GetComponent<BowShooter2D>();
        if (shooter == null || !shooter.CanFire) return false;


        Vector2 forward = transform.right;
        int mid = arrowCount / 2;
        for (int i = 0; i < arrowCount; i++)
        {
            float offset = (i - mid) * (spreadDegrees / Mathf.Max(1, arrowCount - 1));
            Vector2 dir = Quaternion.Euler(0, 0, offset) * forward;
            shooter.Fire(dir, gameObject.tag);
        }
        return true;
    }
}