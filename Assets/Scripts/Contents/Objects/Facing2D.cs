using UnityEngine;

[DisallowMultipleComponent]
public class Facing2D : MonoBehaviour
{
    [Tooltip("시각만 뒤집고 싶으면 여기에 스프라이트(또는 Visual 루트)를 넣으세요. 비우면 자기 자신을 뒤집습니다.")]
    [SerializeField] private Transform visual;

    public int Sign { get; private set; } = 1;  
    Vector3 _baseScale;

    void Awake()
    {
        var t = visual ? visual : transform;
        _baseScale = t.localScale;
        if (Mathf.Approximately(_baseScale.x, 0f))
            _baseScale.x = 2f; 
    }

    public void FaceByInput(float h)
    {
        if (h > 0.01f)
        {
            Face(-1); // Face right by flipping the X scale
        }
        else
        {
            // Face left when idle or moving left
            Face(1);
        }
    }


    public void FaceByVelocityX(float vx)
    {
        if (Mathf.Abs(vx) < 0.001f) return;
        Face(vx < 0 ? 1 : -1);
    }

    public void Face(int sign)
    {
        if (sign == 0) return;
        Sign = sign < 0 ? -1 : 1;

        var t = visual ? visual : transform;
        var s = t.localScale;
        s.x = Mathf.Abs(_baseScale.x) * Sign; 
        s.y = _baseScale.y;
        s.z = _baseScale.z;
        t.localScale = s;
    }

    public Vector2 Forward => new Vector2(Sign, 0f);
}