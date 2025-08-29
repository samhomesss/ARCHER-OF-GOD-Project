using UnityEngine;

public class UI_PlayerMove : UI_Scene
{
    enum Buttons
    {

    }


    public override bool Init()
    {
        if(base.Init() == false)
            return false;

        return true;
    }
}
