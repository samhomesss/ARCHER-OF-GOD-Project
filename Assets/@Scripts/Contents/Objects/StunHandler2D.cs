using UnityEngine;

public class StunHandler2D : MonoBehaviour, IStunnable2D
{
    [SerializeField] private MonoBehaviour[] disableOnStun; 
    [SerializeField] private Rigidbody2D rb;

    private float _timer;
    private bool _stunActive;
    private bool _savedKinematic;
    private RigidbodyConstraints2D _savedConstraints;

    void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
    }

    public bool IsStunned => _timer > 0f;

    public void Stun(float duration)
    {
        if (duration <= 0f) return;

        if (!_stunActive)
        {
            _stunActive = true;

            foreach (var mb in disableOnStun)
                if (mb) mb.enabled = false;

            DG.Tweening.DOTween.Kill(transform);
            if (rb) DG.Tweening.DOTween.Kill(rb);

            if (rb)
            {
                _savedKinematic = rb.isKinematic;
                _savedConstraints = rb.constraints;

                rb.isKinematic = true;
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;

            }
        }

        _timer = Mathf.Max(_timer, duration);
    }

    void Update()
    {
        if (_timer <= 0f) return;

        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            if (rb)
            {
                rb.isKinematic = _savedKinematic;
                rb.constraints = _savedConstraints;
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            foreach (var mb in disableOnStun)
                if (mb) mb.enabled = true;

            _stunActive = false;
        }
    }
}
