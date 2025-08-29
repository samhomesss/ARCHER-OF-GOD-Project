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

    //  ������ �ϴ°� DamageUpItem�� ������ DamageUpCoolTimer ������Ʈ�� Ȱ��ȭ ��Ű�� DamageUPItem�� duration ��ŭ CoolTimer��
    //  Fill�� ���ҽ�Ų�� �ش� Fill�� 0�� �Ǹ� DamageUpCoolTimer ������Ʈ�� ��Ȱ��ȭ
}
