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
                if (other.CompareTag("Player"))
                {
                    var ui = FindObjectOfType<UI_PlayerDamageUp>();
                    if (ui != null)
                        ui.StartCoolTimer(duration);
                }
                else if (other.CompareTag("Enemy"))
                {
                    var ui = FindObjectOfType<UI_EnemyDamageUp>();
                    if (ui != null)
                        ui.StartCoolTimer(duration);
                }
            }

            Destroy(gameObject);
        }
    }
}
