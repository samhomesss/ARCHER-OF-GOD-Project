using UnityEngine;

public class DamageUpItem : MonoBehaviour
{
    [SerializeField] private float multiplier = 2f;
    [SerializeField] private float duration = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            var shooter = other.GetComponentInParent<BowShooter2D>();
            if (shooter != null)
            {
                shooter.ApplyDamageMultiplier(multiplier, duration);
            }

            Destroy(gameObject);
        }
    }
}
