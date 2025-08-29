using UnityEngine;
using UnityEngine.EventSystems;

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
        var leftButton = GetButton((int)Buttons.LeftArrow);
        var rightButton = GetButton((int)Buttons.RightArrow);

        UI_Base.BindEvent(leftButton.gameObject, (PointerEventData evt) =>
        {
            if (_player != null)
                _player.SetHorizontal(-1f);
        }, Define.EUIEvent.PointerDown);

        UI_Base.BindEvent(leftButton.gameObject, (PointerEventData evt) =>
        {
            if (_player != null)
                _player.SetHorizontal(0f);
        }, Define.EUIEvent.PointerUp);

        UI_Base.BindEvent(rightButton.gameObject, (PointerEventData evt) =>
        {
            if (_player != null)
                _player.SetHorizontal(1f);
        }, Define.EUIEvent.PointerDown);

        UI_Base.BindEvent(rightButton.gameObject, (PointerEventData evt) =>
        {
            if (_player != null)
                _player.SetHorizontal(0f);
        }, Define.EUIEvent.PointerUp);
    }
}
