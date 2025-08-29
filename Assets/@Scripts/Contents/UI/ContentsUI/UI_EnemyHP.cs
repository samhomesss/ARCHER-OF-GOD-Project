using UnityEngine;
using UnityEngine.UI;

public class UI_EnemyHP : UI_Scene
{
    enum GameObjects
    {
        EnemyHPSlider,
    }

    Slider _enemyHpSlider;

    public override bool Init()
    {
        if (base.Init() == false) 
        {
            return false;
        }

        BindObjects(typeof(GameObjects));

        _enemyHpSlider = GetObject((int)GameObjects.EnemyHPSlider).GetComponent<Slider>();

        Debug.Log(_enemyHpSlider.name);


        return true;
    }
}
