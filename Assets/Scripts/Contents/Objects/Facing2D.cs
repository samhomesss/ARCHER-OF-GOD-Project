using UnityEngine;

[DisallowMultipleComponent]
public class Facing2D : MonoBehaviour
{
    [Tooltip("�ð��� ������ ������ ���⿡ ��������Ʈ(�Ǵ� Visual ��Ʈ)�� ��������. ���� �ڱ� �ڽ��� �������ϴ�.")]
    [SerializeField] private Transform visual;

    public int Sign { get; private set; } = 1;  // +1 = ������, -1 = ����
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
        if (Mathf.Abs(h) < 0.01f) return; 
        Face(Mathf.Sign(h) < 0 ? -1 : 1);
    }

    public void FaceByVelocityX(float vx)
    {
        if (Mathf.Abs(vx) < 0.001f) return;
        Face(vx < 0 ? -1 : 1);
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