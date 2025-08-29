using System;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance => _instance;
    static GameManager _instance;

    [SerializeField] private Health2D playerHealth;
    [SerializeField] private Health2D botHealth;

    private event Action<int> OnPlayerDamagedEvent;
    private event Action<int> OnEnemyDamagedEvent;


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