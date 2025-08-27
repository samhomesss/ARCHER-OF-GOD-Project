using UnityEngine;


public abstract class SkillBase2D : MonoBehaviour
{
    [SerializeField] protected float cooldown = 3f;
    [SerializeField] protected string skillName = "Skill";

    protected float _lastUseTime = -999f;
    public bool IsCasting { get; protected set; }
    protected void BeginCast() => IsCasting = true;
    protected void EndCast() => IsCasting = false;
    public override string ToString() => skillName;
    public bool IsReady => Time.time >= _lastUseTime + cooldown;
    public float Remaining => Mathf.Max(0f, _lastUseTime + cooldown - Time.time);


    public bool TryCast()
    {
        if (!IsReady) return false;
        bool ok = Cast();
        if (ok) _lastUseTime = Time.time;
        return ok;
    }


    protected abstract bool Cast();
}