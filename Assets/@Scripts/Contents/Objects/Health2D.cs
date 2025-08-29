using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


[DisallowMultipleComponent]
public class Health2D : MonoBehaviour, IDamageable2D
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private bool destroyOnDeath = false;

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
        Debug.Log(CurrentHealth + "커런트 값");
        healthSlider.value += amount;
        Debug.Log(healthSlider.value + "Value 값");
    }


    public void TakeDamage(float amount, Vector2 hitPoint, Vector2 hitNormal, GameObject source)
    {
        if (CurrentHealth <= 0) return;
        CurrentHealth -= amount;
        healthSlider.value -= amount;
        onDamaged?.Invoke();

        if (CurrentHealth <= 0)
        {
            onDeath?.Invoke();

            var anim = GetComponentInChildren<Animator>();
            if (anim)
                anim.Play("die");
            Destroy(gameObject);
        }
    }
}