using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerHP : UI_Scene
{
    enum GameObjects
    {
        PlayerHPSlider,
    }

    Slider _playerHpSlider;

    public override bool Init()
    {
        if (base.Init() == false) 
            return false;

        BindObjects(typeof(GameObjects));

        _playerHpSlider = GetObject((int)GameObjects.PlayerHPSlider).GetComponent<Slider>();

         
        return true;
    }

    private void Start()
    {
        
    }
}
