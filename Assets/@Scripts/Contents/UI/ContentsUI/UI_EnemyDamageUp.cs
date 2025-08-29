using UnityEngine;
using UnityEngine.UI;

public class UI_EnemyDamageUp : UI_Scene
{
    enum GameObjects
    {
        DamageUpCoolTimer,
        CoolTimer,
    }

    GameObject _damageUpCoolTimer;
    Image _coolTimer;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObjects(typeof(GameObjects));

        _damageUpCoolTimer = GetObject((int)GameObjects.DamageUpCoolTimer);
        _coolTimer = GetObject((int)GameObjects.CoolTimer).GetComponent<Image>();

        _damageUpCoolTimer.SetActive(false);

        return true;
    }

    //  만들어야 하는거 DamageUpItem을 먹으면 DamageUpCoolTimer 오브젝트를 활성화 시키고 DamageUPItem의 duration 만큼 CoolTimer의
    //  Fill을 감소시킨다 해당 Fill이 0이 되면 DamageUpCoolTimer 오브젝트를 비활성화
}
