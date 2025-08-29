using UnityEngine;

public class UI_PlayerSkill : UI_Scene
{
    enum Buttons
    {
        SkillCardImage1,
        SkillCardImage2,
        SkillCardImage3,
        SkillCardImage4,
        SkillCardImage5,
        
    }

    enum GameObjects
    {
        Skill1CoolTimer,
        Skill2CoolTimer,
        Skill3CoolTimer,
        Skill4CoolTimer,
        Skill5CoolTimer,
    }

    GameObject _skill1Cool;
    GameObject _skill2Cool;
    GameObject _skill3Cool;
    GameObject _skill4Cool;
    GameObject _skill5Cool;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButtons(typeof(Buttons));
        BindObjects(typeof(GameObjects));

        // ó�� ���� �ʱ�ȭ �κ� ����� �κ��� ���Ƽ� ������

        _skill1Cool = GetObject((int)GameObjects.Skill1CoolTimer);
        _skill2Cool = GetObject((int)GameObjects.Skill2CoolTimer);
        _skill3Cool = GetObject((int)GameObjects.Skill3CoolTimer);
        _skill4Cool = GetObject((int)GameObjects.Skill4CoolTimer);
        _skill5Cool = GetObject((int)GameObjects.Skill5CoolTimer);

        _skill1Cool.SetActive(false);
        _skill2Cool.SetActive(false);
        _skill3Cool.SetActive(false);
        _skill4Cool.SetActive(false);
        _skill5Cool.SetActive(false);

        return true;
    }

    private void Start()
    {
        GetButton((int)Buttons.SkillCardImage1).onClick.AddListener(() =>
        {
            // �÷��̾� SkillJumpShot ����
            // ���������� �ش� �� Ÿ�� ��ƴ Skill1CoolTimer �״ٰ� ���� 
            // SKill1CoolTimer �ڽĿ� �ִ� TMP_Text�� ���ؼ� ��Ÿ���� �󸶳� ���Ҵ��� �˷��ֱ� 
        });

        GetButton((int)Buttons.SkillCardImage2).onClick.AddListener(() =>
        {
            // �÷��̾� SkillVerTicalRainStrike ����
            // ���������� �ش� �� Ÿ�� ��ƴ Skill2CoolTimer �״ٰ� ���� 
            // SKill2CoolTimer �ڽĿ� �ִ� TMP_Text�� ���ؼ� ��Ÿ���� �󸶳� ���Ҵ��� �˷��ֱ� 
        });

        GetButton((int)Buttons.SkillCardImage3).onClick.AddListener(() =>
        {
            // �÷��̾� SkillPiercingShoot ����
            // ���������� �ش� �� Ÿ�� ��ƴ Skill3CoolTimer �״ٰ� ���� 
            // SKill3CoolTimer �ڽĿ� �ִ� TMP_Text�� ���ؼ� ��Ÿ���� �󸶳� ���Ҵ��� �˷��ֱ� 
        });
        GetButton((int)Buttons.SkillCardImage4).onClick.AddListener(() =>
        {
            // �÷��̾� SkillStunShoot ����
            // ���������� �ش� �� Ÿ�� ��ƴ Skill4CoolTimer �״ٰ� ���� 
            // SKill4CoolTimer �ڽĿ� �ִ� TMP_Text�� ���ؼ� ��Ÿ���� �󸶳� ���Ҵ��� �˷��ֱ� 
        });
        GetButton((int)Buttons.SkillCardImage5).onClick.AddListener(() =>
        {
            // �÷��̾� SkillDash ����
            // ���������� �ش� �� Ÿ�� ��ƴ Skill5CoolTimer �״ٰ� ���� 
            // SKill5CoolTimer �ڽĿ� �ִ� TMP_Text�� ���ؼ� ��Ÿ���� �󸶳� ���Ҵ��� �˷��ֱ� 
        });
    }
}
