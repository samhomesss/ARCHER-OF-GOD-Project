using UnityEngine;

public class HPUPItem : MonoBehaviour
{
    [SerializeField] private float healAmount = 500f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            var health = other.GetComponentInParent<Health2D>();

            if (health != null)
            {
                health.Heal(healAmount);
            }

            Destroy(gameObject);
        }
    }
}
