using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerDamageUp : UI_Scene
{
    enum GameObjects
    {
        DamageUpCoolTimer,
        CoolTimer,
    }

    GameObject _damageUpCoolTimer;
    Image _coolTimer;
    Coroutine _timerRoutine;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObjects(typeof(GameObjects));

        _damageUpCoolTimer = GetObject((int)GameObjects.DamageUpCoolTimer);
        _coolTimer = GetObject((int)GameObjects.CoolTimer).GetComponent<Image>();

        _damageUpCoolTimer.SetActive(false);

        return true;
    }
    
    public void StartCoolTimer(float duration)
    {
        if (_timerRoutine != null)
            StopCoroutine(_timerRoutine);
        _timerRoutine = StartCoroutine(CoolTimerRoutine(duration));
    }

    IEnumerator CoolTimerRoutine(float duration)
    {
        if (_damageUpCoolTimer == null || _coolTimer == null)
            yield break;

        _damageUpCoolTimer.SetActive(true);
        _coolTimer.fillAmount = 1f;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _coolTimer.fillAmount = Mathf.Clamp01(1f - (elapsed / duration));
            yield return null;
        }

        _coolTimer.fillAmount = 0f;
        _damageUpCoolTimer.SetActive(false);
        _timerRoutine = null;
    }
}
