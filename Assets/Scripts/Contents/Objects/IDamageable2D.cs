using UnityEngine;

public interface IDamageable2D
{
    void TakeDamage(float amount, Vector2 hitPoint, Vector2 hitNormal, GameObject source);
}
