using UnityEngine;
using System.Collections;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class SkillDash2D : SkillBase2D
{
    [SerializeField] private float dashDistance = 4f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private bool invulnerableDuringDash = true;


    private Rigidbody2D _rb;
    private Health2D _health;
    private bool _dashing;
    private Tween _dashTween;


    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _health = GetComponent<Health2D>();
    }


    protected override bool Cast()
    {
        if (_dashing) return false;
        Debug.Log("대쉬 하는중");
        Vector2 moveDir = GetPreferredDirection();
        if (moveDir.sqrMagnitude < 0.0001f) moveDir = transform.right;
        BeginCast();
        PerformDash(moveDir.normalized);
        return true;
    }

    private void PerformDash(Vector2 dir)
    {
        _dashing = true;
        var originalLayer = gameObject.layer;
        if (invulnerableDuringDash)
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        Vector2 target = _rb.position + dir * dashDistance;
        _dashTween?.Kill();
        _dashTween = _rb.DOMove(target, dashDuration)
                        .SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            gameObject.layer = originalLayer;
                            _dashing = false;
                            EndCast();
                        });
    }



    private Vector2 GetPreferredDirection()
    {
        // Prefer current look direction
        return transform.right;
    }
}