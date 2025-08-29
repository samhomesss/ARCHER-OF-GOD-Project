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
    //  만들어야 하는거 DamageUpItem을 먹으면 DamageUpCoolTimer 오브젝트를 활성화 시키고 DamageUPItem의 duration 만큼 CoolTimer의
    //  Fill을 감소시킨다 해당 Fill이 0이 되면 DamageUpCoolTimer 오브젝트를 비활성화
    // DamageUpItem을 먹으면 DamageUpCoolTimer를 활성화하고
    // DamageUPItem duration만큼 CoolTimer의 FillAmount를 감소시킨다.
    // FillAmount가 0이 되면 DamageUpCoolTimer를 비활성화한다.
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
