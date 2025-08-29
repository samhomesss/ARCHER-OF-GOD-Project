using UnityEngine;

public class StunHandler2D : MonoBehaviour, IStunnable2D
{
    [SerializeField] private MonoBehaviour[] disableOnStun;
    [SerializeField] private Rigidbody2D rb;

    private float _timer;

    void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
    }

    public void Stun(float duration)
    {
        if (duration <= 0f) return;
        if (_timer <= 0f)
        {
            foreach (var mb in disableOnStun)
            {
                if (mb) mb.enabled = false;
            }
        }
        _timer = Mathf.Max(_timer, duration);
    }

    void Update()
    {
        if (_timer > 0f)
        {
            _timer -= Time.deltaTime;
            if (rb) rb.linearVelocity = Vector2.zero;
            if (_timer <= 0f)
            {
                foreach (var mb in disableOnStun)
                {
                    if (mb) mb.enabled = true;
                }
            }
        }
    }
}