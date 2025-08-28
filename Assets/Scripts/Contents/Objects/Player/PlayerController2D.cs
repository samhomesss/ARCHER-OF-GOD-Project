using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BowShooter2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;


    [Header("Input Keys")]
    [SerializeField] private KeyCode skill1Key = KeyCode.Q;
    [SerializeField] private KeyCode skill2Key = KeyCode.E;
    [SerializeField] private KeyCode skill3Key = KeyCode.R;
    [SerializeField] private KeyCode skill4Key = KeyCode.T;
    [SerializeField] private KeyCode skill5Key = KeyCode.F;


    private Rigidbody2D _rb;
    private BowShooter2D _shooter;
    private SkillBase2D[] _skills;

    private Facing2D _facing;
    private AutoAttackController2D _autoAttack;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _shooter = GetComponent<BowShooter2D>();
        _skills = GetComponents<SkillBase2D>();
        _facing = GetComponent<Facing2D>();
        _autoAttack = GetComponent<AutoAttackController2D>();
    }

    void Update()
    {
        // 마우스 에임 완전 제거
        HandleSkills();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        Debug.Log("좌우 값 : " + h);
        Vector2 dir = new Vector2(h, 0f).normalized;
        _rb.linearVelocity = dir * moveSpeed;

        if (_facing)
        {
            if (Mathf.Abs(h) > 0.01f)
            {
                Debug.Log("좌우 값 : " + h);
                _facing.FaceByInput(-h);
            }
            else if (_autoAttack && _autoAttack.Target)
            {
                float diff = _autoAttack.Target.position.x - transform.position.x;
                if (Mathf.Abs(diff) > 0.01f)
                    _facing.Face(diff < 0 ? 1 : -1);
            }
        }
    }


    private void HandleSkills()
    {
        if (Input.GetKeyDown(skill1Key)) TrySkillIndex(0);
        if (Input.GetKeyDown(skill2Key)) TrySkillIndex(1);
        if (Input.GetKeyDown(skill3Key)) TrySkillIndex(2);
        if (Input.GetKeyDown(skill3Key)) TrySkillIndex(3);
        if (Input.GetKeyDown(skill3Key)) TrySkillIndex(4);
    }


    private void TrySkillIndex(int idx)
    {
        if (idx < 0 || idx >= _skills.Length) return;
        _skills[idx].TryCast();
    }
}