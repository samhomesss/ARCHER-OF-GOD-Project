using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance => _instance;
    static GameManager _instance;

    [SerializeField] private Health2D playerHealth;
    [SerializeField] private Health2D botHealth;
    [SerializeField] private GameObject winResultCanvas;
    [SerializeField] private GameObject loseResultCanvas;

    private Animator playerAnim;
    private Animator enemyAnim;
    private bool resultShown;

    public event Action<int> OnPlayerDamagedEvent;
    public event Action<int> OnEnemyDamagedEvent;

    public Health2D PlayerHealth => playerHealth;
    public Health2D EnemyHealth => botHealth;

    void Awake()
    {
        _instance = this;
    }

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

        if (winResultCanvas.GetComponent<Canvas>().enabled)
            winResultCanvas.GetComponent<Canvas>().enabled = false;
        if (loseResultCanvas.GetComponent<Canvas>().enabled)
            loseResultCanvas.GetComponent<Canvas>().enabled = false;


        if (playerHealth) playerAnim = playerHealth.GetComponentInChildren<Animator>();
        if (botHealth) enemyAnim = botHealth.GetComponentInChildren<Animator>();

        if (playerHealth) playerHealth.onDeath.AddListener(() =>
        {
            if (resultShown)
                return;

            resultShown = true;
            if (enemyAnim) enemyAnim.Play("victory");
            loseResultCanvas.GetComponent<Canvas>().enabled = true;
        });
        if (botHealth) botHealth.onDeath.AddListener(() =>
        {
            if (resultShown)
                return;

            resultShown = true;
            if (playerAnim) playerAnim.Play("victory");
            winResultCanvas.GetComponent<Canvas>().enabled = true;

        });
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

    public void DetermineWinnerByHealth()
    {
        if (resultShown)
            return;

        resultShown = true;

        if (playerHealth != null && botHealth != null && playerHealth.CurrentHealth > botHealth.CurrentHealth)
        {
            if (playerAnim) playerAnim.Play("victory");
            winResultCanvas.GetComponent<Canvas>().enabled = true;
        }
        else
        {
            if (enemyAnim) enemyAnim.Play("victory");
            loseResultCanvas.GetComponent<Canvas>().enabled = true;
        }
    }
}