using UnityEngine;

public class UI_PlayerMove : UI_Scene
{
    enum Buttons
    {
        LeftArrow,
        RightArrow,
    }


    public override bool Init()
    {
        if(base.Init() == false)
            return false;

        BindButtons(typeof(Buttons));
       

        return true;
    }

    private void Start()
    {
        GetButton((int)Buttons.LeftArrow).onClick.AddListener(() =>
        {
            // 플레이어 왼쪽 이동 
        });

        GetButton((int)Buttons.RightArrow).onClick.AddListener(() =>
        {
            // 플레이어 오른쪽 이동 
        });
    }
}
