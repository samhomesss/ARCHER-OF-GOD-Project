using System;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance => _instance;
    static GameManager _instance;

    [SerializeField] private Health2D playerHealth;
    [SerializeField] private Health2D botHealth;

    private Animator playerAnim;
    private Animator enemyAnim;

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


        if (playerHealth) playerAnim = playerHealth.GetComponentInChildren<Animator>();
        if (botHealth) enemyAnim = botHealth.GetComponentInChildren<Animator>();

        if (playerHealth) playerHealth.onDeath.AddListener(() =>
        {
            if (enemyAnim) enemyAnim.Play("victory");
        });
        if (botHealth) botHealth.onDeath.AddListener(() =>
        {
            if (playerAnim) playerAnim.Play("victory");
        });
    }

    /// <summary>
    /// �÷��̾ ���ݹ޾����� ���
    /// </summary>
    /// <param name="damage"></param>
    public void PlayerDamaged(int damage)
    {
        Debug.Log("�÷��̾� ���� ����");
        OnPlayerDamagedEvent?.Invoke(damage);
    }

    /// <summary>
    /// ���� ���� �޾����� ���
    /// </summary>
    /// <param name="damage"></param>
    public void EnemyDamaged(int damage)
    {
        OnEnemyDamagedEvent?.Invoke(damage);
    }
}