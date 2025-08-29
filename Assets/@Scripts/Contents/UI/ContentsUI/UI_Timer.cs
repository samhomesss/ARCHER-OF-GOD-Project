using TMPro;
using UnityEngine;

public class UI_Timer : UI_Scene
{
    enum Texts
    {
        TimerText,
    }

    TMP_Text _timerText;

    public override bool Init()
    {
        if (base.Init() == false) 
            return false;

        BindTexts(typeof(Texts));

        _timerText = GetText((int)Texts.TimerText);

        Debug.Log(_timerText.name);

        return true;    
    }
}
