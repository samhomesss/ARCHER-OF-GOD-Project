using UnityEngine;

public class SkillMultiShot2D : SkillBase2D
{
    [SerializeField] private BowShooter2D shooter;
    [SerializeField] private int arrowCount = 3;
    [SerializeField] private float spreadDegrees = 20f;

    private Facing2D _facing;

    void Awake()
    {
        _facing = GetComponent<Facing2D>();
    }

    protected override bool Cast()
    {
        if (shooter == null) shooter = GetComponent<BowShooter2D>();
        if (shooter == null || !shooter.CanFire) return false;

        BeginCast();

        Vector2 forward = _facing ? _facing.Forward : (Vector2)transform.right;

        int mid = arrowCount / 2;
        for (int i = 0; i < arrowCount; i++)
        {
            float offset = (i - mid) * (spreadDegrees / Mathf.Max(1, arrowCount - 1));
            Vector2 dir = Quaternion.Euler(0, 0, offset) * forward;
            shooter.Fire(dir, gameObject.tag);
        }

        StartCoroutine(EndCastNextFrame());
        return true;
    }

    private System.Collections.IEnumerator EndCastNextFrame()
    {
        yield return null;
        EndCast();
    }
}