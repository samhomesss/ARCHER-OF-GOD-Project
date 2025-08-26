using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Rigidbody2D))]
public class SkillDash2D : SkillBase2D
{
    [SerializeField] private float dashDistance = 4f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private bool invulnerableDuringDash = true;


    private Rigidbody2D _rb;
    private Health2D _health;
    private bool _dashing;


    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _health = GetComponent<Health2D>();
    }


    protected override bool Cast()
    {
        if (_dashing) return false;
        Vector2 moveDir = GetPreferredDirection();
        if (moveDir.sqrMagnitude < 0.0001f) moveDir = transform.right;
        StartCoroutine(DashRoutine(moveDir.normalized));
        return true;
    }


    private Vector2 GetPreferredDirection()
    {
        // Prefer current look direction
        return transform.right;
    }


    private IEnumerator DashRoutine(Vector2 dir)
    {
        _dashing = true;
        float t = 0f;
        float speed = dashDistance / Mathf.Max(0.01f, dashDuration);
        var originalLayer = gameObject.layer;


        if (invulnerableDuringDash)
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast"); // simple invuln trick if unused layer


        while (t < dashDuration)
        {
            _rb.linearVelocity = dir * speed;
            t += Time.deltaTime;
            yield return null;
        }


        _rb.linearVelocity = Vector2.zero;
        gameObject.layer = originalLayer;
        _dashing = false;
    }
}