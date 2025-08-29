using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SkillJumpTripleShot2D : SkillBase2D
{
    [Header("Jump")]
    [SerializeField] private float jumpHeight = 2.0f;
    [SerializeField] private float jumpUpTime = 0.12f;
    [SerializeField] private float jumpDownTime = 0.14f;

    [Header("Shooting")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private Projectile2D projectilePrefab;
    [SerializeField] private int shots = 3;
    [SerializeField] private float shotInterval = 0.07f;
    [SerializeField] private float straightSpeed = 22f;
    [SerializeField] private float targetHeightOffset = 0.0f;

    [Header("Rotation")]
    [SerializeField] private float rotationDuration = 0.1f;

    [Header("Visual Root")]
    [SerializeField] private Transform gfx; 

    private Transform _target;
    private Rigidbody2D _rb;
    private Tween _jumpTween;
    private Tween _rotateTween;

    private Animator _anim;
    private MonoBehaviour _facingOrFlipper;

    private bool _wasAnimEnabled;
    private bool _wasFacingEnabled;

    protected override bool ShouldTriggerAttackAnim => false;
    void Awake()
    {
        if (!firePoint)
        {
            var t = transform.Find("FirePoint");
            if (t) firePoint = t;
        }
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>(); 

        if (!gfx)
        {
            var sr = GetComponentInChildren<SpriteRenderer>();
            if (sr) gfx = sr.transform;
            else gfx = this.transform; 
        }

        _facingOrFlipper = GetComponent<MonoBehaviour>(); 
    }

    void Start()
    {
        string desiredTag = CompareTag("Player") ? "Enemy" : "Player";
        var go = GameObject.FindGameObjectWithTag(desiredTag);
        if (go) _target = go.transform;
    }

    protected override bool Cast()
    {
        if (!firePoint || !projectilePrefab || !_target) return false;

        if (_anim)
        {
            _wasAnimEnabled = _anim.enabled;
            _anim.enabled = false;
        }
        if (_facingOrFlipper)
        {
            _wasFacingEnabled = _facingOrFlipper.enabled;
            _facingOrFlipper.enabled = false;
        }

        BeginCast();
        StartCoroutine(JumpAndShoot());
        return true;
    }

    private IEnumerator JumpAndShoot()
    {
        Vector2 basePos = _rb.position;
        Quaternion baseGfxRot = gfx.rotation;
        Vector3 baseGfxScale = gfx.localScale;

        _jumpTween?.Kill();
        _jumpTween = DOTween.Sequence()
            .Append(_rb.DOMoveY(basePos.y + jumpHeight, jumpUpTime).SetEase(Ease.OutQuad))
            .Append(_rb.DOMoveY(basePos.y, jumpDownTime).SetEase(Ease.InQuad));

        
        float sign = Mathf.Sign(baseGfxScale.x == 0 ? 1 : baseGfxScale.x);
        gfx.localScale = new Vector3(3.5f * sign, 0.8f, baseGfxScale.z);

        _rotateTween?.Kill();
        _rotateTween = gfx
            .DORotate(new Vector3(0f, 0f, 360f), rotationDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);

        yield return new WaitForSeconds(jumpUpTime);

        for (int i = 0; i < shots; i++)
        {
            if (!_target)
                break;

            Vector2 targetPos = (Vector2)_target.position + Vector2.up * targetHeightOffset;
            Vector2 dir = (targetPos - (Vector2)firePoint.position).normalized;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            var proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(0f, 0f, angle));
            var rb = proj.GetComponent<Rigidbody2D>();
            if (rb) rb.gravityScale = 0f; 
            proj.FireWithVelocity(dir * straightSpeed, gameObject.tag);

            if (i < shots - 1)
                yield return new WaitForSeconds(shotInterval);
        }

        yield return _jumpTween.WaitForCompletion();

        _rotateTween.Kill();
        gfx.rotation = baseGfxRot;
        gfx.localScale = baseGfxScale;

        EndCast();
    }
}
