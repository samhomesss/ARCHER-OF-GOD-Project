using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class HitFlash2D : MonoBehaviour
{
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashTime = 0.08f;


    private SpriteRenderer _sr;
    private Color _base;


    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _base = _sr.color;
        var hp = GetComponent<Health2D>();
        if (hp) hp.onDamaged.AddListener(FlashOnce);
    }


    private void FlashOnce()
    {
        StopAllCoroutines();
        StartCoroutine(Flash());
    }


    private System.Collections.IEnumerator Flash()
    {
        _sr.color = flashColor;
        yield return new WaitForSeconds(flashTime);
        _sr.color = _base;
    }
}