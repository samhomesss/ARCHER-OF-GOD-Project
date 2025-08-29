using System.Collections;
using UnityEngine;

public class UI_PlayerHitEffect : MonoBehaviour
{
    private Canvas _canvas;

    void Awake()
    {
        _canvas = GetComponent<Canvas>();
        if (_canvas)
            _canvas.enabled = false;
    }

    void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            var hp = player.GetComponent<Health2D>();
            if (hp)
            {
                hp.onDamaged.AddListener(PlayHitEffect);
            }
        }
    }

    private void PlayHitEffect()
    {
        StopAllCoroutines();
        StartCoroutine(HitEffectRoutine());
    }

    private IEnumerator HitEffectRoutine()
    {
        if (_canvas)
        {
            _canvas.enabled = true;
            yield return new WaitForSeconds(0.5f);
            _canvas.enabled = false;
        }
    }
}
