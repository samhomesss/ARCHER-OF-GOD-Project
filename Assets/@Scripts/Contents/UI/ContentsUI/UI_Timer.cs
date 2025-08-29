using TMPro;
using UnityEngine;

public class UI_Timer : UI_Scene
{
    enum Texts
    {
        TimerText,
    }

    TMP_Text _timerText;

    float _timer = 60;

    public override bool Init()
    {
        if (base.Init() == false) 
            return false;

        BindTexts(typeof(Texts));

        _timerText = GetText((int)Texts.TimerText);

        _timerText.text = $"{(int)_timer}";
        Debug.Log(_timerText.name);

        return true;    
    }

    private void Update()
    {
        if (_timer <= 0)
            return;

        _timer -= Time.deltaTime;
        _timerText.text = $"{(int)_timer}";

        if ( _timer <= 0 )
        {
            // TODO : UI_Result 켜주기 
            // 플레이어 체력과 적 체력을 비교해서 켜주면 됨 

        }
    }
}
