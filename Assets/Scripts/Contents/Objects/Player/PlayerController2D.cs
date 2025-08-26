using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BowShooter2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;


    [Header("Aim")]
    [SerializeField] private Transform rotateVisual; // optional: sprite or child that points right


    [Header("Input Keys")]
    [SerializeField] private KeyCode fireKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode skill1Key = KeyCode.Q;
    [SerializeField] private KeyCode skill2Key = KeyCode.E;
    [SerializeField] private KeyCode skill3Key = KeyCode.R;


    private Rigidbody2D _rb;
    private BowShooter2D _shooter;
    private SkillBase2D[] _skills;


    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _shooter = GetComponent<BowShooter2D>();
        _skills = GetComponents<SkillBase2D>();
    }


    void Update()
    {
        HandleAim();
        HandleFire();
        HandleSkills();
    }


    void FixedUpdate()
    {
        HandleMovement();
    }


    private void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(h, v).normalized;
        _rb.linearVelocity = dir * moveSpeed;
    }


    private void HandleAim()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 aimDir = (mouseWorld - transform.position);
        aimDir.z = 0;
        if (aimDir.sqrMagnitude > 0.0001f)
        {
            float ang = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, ang);
            if (rotateVisual) rotateVisual.right = aimDir.normalized;
        }
    }


    private void HandleFire()
    {
        if (Input.GetKey(fireKey))
        {
            Vector2 forward = transform.right;
            _shooter.Fire(forward, gameObject.tag);
        }
    }


    private void HandleSkills()
    {
        if (Input.GetKeyDown(skill1Key)) TrySkillIndex(0);
        if (Input.GetKeyDown(skill2Key)) TrySkillIndex(1);
        if (Input.GetKeyDown(skill3Key)) TrySkillIndex(2);
    }


    private void TrySkillIndex(int idx)
    {
        if (idx < 0 || idx >= _skills.Length) return;
        _skills[idx].TryCast();
    }
}
