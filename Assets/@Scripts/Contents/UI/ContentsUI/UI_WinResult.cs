using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_WinResult : UI_Scene
{
    enum Buttons
    {
        RestartButton,
    }
        
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButtons(typeof(Buttons));

        GetButton((int)Buttons.RestartButton).onClick.AddListener(() => 
        {
            SceneManager.LoadScene("GameScene");
        });

        return true;
    }
}
