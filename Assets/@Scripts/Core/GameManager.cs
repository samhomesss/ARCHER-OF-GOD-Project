using UnityEngine;


public class GameManagerAOG : MonoBehaviour
{
    [SerializeField] private Health2D playerHealth;
    [SerializeField] private Health2D botHealth;


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
}