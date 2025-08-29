using System;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance => _instance;
    static GameManager _instance;

    [SerializeField] private Health2D playerHealth;
    [SerializeField] private Health2D botHealth;

    public event Action<int> OnPlayerDamagedEvent;
    public event Action<int> OnEnemyDamagedEvent;

    public Health2D PlayerHealth => playerHealth;
    public Health2D EnemyHealth => botHealth;


    void Start()
    {
        if (playerHealth == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) playerHealth = p.GetComponent<Health2D>();
        }
        if (botHealth == null)
        {
            var e = GameObject.FindGameObjectWithTag("Enemy");
            if (e) botHealth = e.GetComponent<Health2D>();
        }


        if (playerHealth) playerHealth.onDeath.AddListener(() => Debug.Log("Enemy Wins!"));
        if (botHealth) botHealth.onDeath.AddListener(() => Debug.Log("Player Wins!"));
    }

    /// <summary>
    /// 플레이어가 공격받았을때 사용
    /// </summary>
    /// <param name="damage"></param>
    public void PlayerDamaged(int damage)
    {
        Debug.Log("플레이어 공격 받음");
        OnPlayerDamagedEvent?.Invoke(damage);
    }

    /// <summary>
    /// 적이 공격 받았을때 사용
    /// </summary>
    /// <param name="damage"></param>
    public void EnemyDamaged(int damage)
    {
        OnEnemyDamagedEvent?.Invoke(damage);
    }
}