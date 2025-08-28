using UnityEngine;


[DisallowMultipleComponent]
public class Facing2D : MonoBehaviour
{
    [Tooltip("Flip only this transform. If empty, flips self.")]
    [SerializeField] private Transform visual;


    [Header("Authoring")]
    [Tooltip("Check if the sprite looks LEFT in its original art (true = faces left).")]
    [SerializeField] private bool spriteFacesLeft = true;


    /// <summary> +1 = world right, -1 = world left (logical facing) </summary>
    public int Sign { get; private set; } = 1;


    Vector3 _baseScale;
    int _authorBase; // +1: art faces right, -1: art faces left


    void Awake()
    {
        var t = visual ? visual : transform;
        _baseScale = t.localScale;
        if (Mathf.Approximately(Mathf.Abs(_baseScale.x), 0f)) _baseScale.x = 1f;
        _authorBase = spriteFacesLeft ? -1 : 1;
    }


    public void FaceByInput(float h)
    {
        if (Mathf.Abs(h) < 0.01f) return;
        Face(h > 0 ? +1 : -1);
    }


    public void FaceByVelocityX(float vx)
    {
        if (Mathf.Abs(vx) < 0.001f) return;
        Face(vx > 0 ? +1 : -1);
    }


    public void Face(int worldRightSign)
    {
        if (worldRightSign == 0) return;
        Sign = worldRightSign > 0 ? +1 : -1;


        var t = visual ? visual : transform;
        var s = t.localScale;
        s.x = Mathf.Abs(_baseScale.x) * _authorBase * Sign; // art base ¡¿ logical facing
        s.y = _baseScale.y;
        s.z = _baseScale.z;
        t.localScale = s;
    }


    public Vector2 Forward => new Vector2(Sign, 0f);
}