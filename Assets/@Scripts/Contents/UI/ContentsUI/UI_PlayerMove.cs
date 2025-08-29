using UnityEngine;

public class UI_PlayerMove : UI_Scene
{
    enum Buttons
    {
        LeftArrow,
        RightArrow,
    }

    PlayerController2D _player;


    public override bool Init()
    {
        if(base.Init() == false)
            return false;

        BindButtons(typeof(Buttons));

        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj)
            _player = playerObj.GetComponent<PlayerController2D>();

        return true;
    }

    private void Start()
    {
        GetButton((int)Buttons.LeftArrow).onClick.AddListener(() =>
        {
            if (_player != null)
                _player.SetHorizontal(-1f);
        });

        GetButton((int)Buttons.RightArrow).onClick.AddListener(() =>
        {
            if (_player != null)
                _player.SetHorizontal(1f);
        });
    }
}
