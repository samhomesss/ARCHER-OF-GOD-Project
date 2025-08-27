using UnityEngine;
using UnityEngine.Events;


[DisallowMultipleComponent]
public class Health2D : MonoBehaviour, IDamageable2D
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private bool destroyOnDeath = true;


    public float MaxHealth => maxHealth;
    public float CurrentHealth { get; private set; }


    public UnityEvent onDamaged;
    public UnityEvent onDeath;


    void Awake()
    {
        CurrentHealth = maxHealth;
    }


    public void Heal(float amount)
    {
        if (amount <= 0) return;
        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
    }


    public void TakeDamage(float amount, Vector2 hitPoint, Vector2 hitNormal, GameObject source)
    {
        if (CurrentHealth <= 0) return;
        CurrentHealth -= amount;
        onDamaged?.Invoke();


        if (CurrentHealth <= 0)
        {
            onDeath?.Invoke();
            if (destroyOnDeath)
                Destroy(gameObject);
        }
    }
}