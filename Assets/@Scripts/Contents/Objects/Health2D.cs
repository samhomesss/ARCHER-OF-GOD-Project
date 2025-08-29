using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


[DisallowMultipleComponent]
public class Health2D : MonoBehaviour, IDamageable2D
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private bool destroyOnDeath = true;

    public Slider healthSlider;
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
        healthSlider.value -= amount;
        onDamaged?.Invoke();
        // TODO: 여기에 UI 감소 되는 부분 추가 

        if (CurrentHealth <= 0)
        {
            onDeath?.Invoke();
            if (destroyOnDeath)
                Destroy(gameObject);
        }
    }
}