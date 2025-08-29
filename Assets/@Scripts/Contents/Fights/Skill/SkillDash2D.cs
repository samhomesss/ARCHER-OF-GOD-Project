using UnityEngine;
using System.Collections;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class SkillDash2D : SkillBase2D
{
    [SerializeField] private float dashDistance = 4f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private bool invulnerableDuringDash = true;
    [SerializeField] private GameObject dashEffectPrefab;
    [SerializeField] private float dashEffectInterval = 0.05f;
    [SerializeField] private float dashEffectDuration = 0.3f;


    private Rigidbody2D _rb;
    private Health2D _health;
    private bool _dashing;
    private Tween _dashTween;
    private Facing2D _facing;
    private EnemyController2D _controller;
    private SpriteRenderer _spriteRenderer;
    private Coroutine _dashEffectCoroutine;

    protected override bool ShouldTriggerAttackAnim => false;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _health = GetComponent<Health2D>();
        _facing = GetComponent<Facing2D>();
        _controller = GetComponent<EnemyController2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }


    protected override bool Cast()
    {
        if (_dashing) return false;
        Debug.Log("대쉬 하는중");

        Vector2 dir = GetPreferredDirection();
        float distance = dashDistance;

        if (_controller && _controller.LeftLimit && _controller.RightLimit)
        {
            float distLeft = Mathf.Abs(_rb.position.x - _controller.LeftLimit.position.x);
            float distRight = Mathf.Abs(_rb.position.x - _controller.RightLimit.position.x);
            Transform farther = distLeft > distRight ? _controller.LeftLimit : _controller.RightLimit;

            dir = new Vector2(farther.position.x - _rb.position.x, 0f).normalized;
            float available = Mathf.Max(0f, Mathf.Abs(farther.position.x - _rb.position.x) - 0.2f);
            distance = Mathf.Min(dashDistance, available);
        }
        else if (dir.sqrMagnitude < 0.0001f)
        {
            dir = transform.right;
        }

        Vector2 moveDir = GetPreferredDirection();
        if (moveDir.sqrMagnitude < 0.0001f) moveDir = transform.right;
        BeginCast();
        PerformDash(dir, distance);
        return true;
    }

    private void PerformDash(Vector2 dir, float distance)
    {
        _dashing = true;
        var originalLayer = gameObject.layer;
        if (invulnerableDuringDash)
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        if (dashEffectPrefab)
            _dashEffectCoroutine = StartCoroutine(SpawnDashEffect());


        Vector2 target = _rb.position + dir * distance;
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
        return _facing ? -_facing.Forward : -(Vector2)transform.right;
    }

    private IEnumerator SpawnDashEffect()
    {
        while (_dashing)
        {
            var effect = Instantiate(dashEffectPrefab, transform.position + Vector3.up * 1.9f, Quaternion.identity);
            if (effect)
            {
                var effectSr = effect.GetComponentInChildren<SpriteRenderer>();
                if (effectSr && _spriteRenderer)
                {
                    effectSr.sprite = _spriteRenderer.sprite;
                    effectSr.flipX = _spriteRenderer.flipX;
                    effectSr.flipY = _spriteRenderer.flipY;
                    effect.transform.localScale = transform.localScale;
                    effectSr.DOFade(0f, dashEffectDuration).OnComplete(() => Destroy(effect));
                }
                else
                {
                    Destroy(effect, dashEffectDuration);
                }
            }

            yield return new WaitForSeconds(dashEffectInterval);
        }
    }
}