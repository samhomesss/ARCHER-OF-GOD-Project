using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_TitleScene : UI_Scene
{
    enum Buttons
    {
        GameStartButton,
    }


    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButtons(typeof(Buttons));

        GetButton((int)Buttons.GameStartButton).onClick.AddListener(() =>
        {
            SceneManager.LoadScene("GameScene");
        });

        return true;
    }
}
