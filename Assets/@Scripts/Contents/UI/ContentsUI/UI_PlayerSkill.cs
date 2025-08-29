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

        // 처음 시작 초기화 부분 공통된 부분이 많아서 합쳐줘

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
            // 플레이어 SkillJumpShot 실행
            // 실행했을때 해당 쿨 타임 만틈 Skill1CoolTimer 켰다가 끄기 
            // SKill1CoolTimer 자식에 있는 TMP_Text를 통해서 쿨타임이 얼마나 남았는지 알려주기 
        });

        GetButton((int)Buttons.SkillCardImage2).onClick.AddListener(() =>
        {
            // 플레이어 SkillVerTicalRainStrike 실행
            // 실행했을때 해당 쿨 타임 만틈 Skill2CoolTimer 켰다가 끄기 
            // SKill2CoolTimer 자식에 있는 TMP_Text를 통해서 쿨타임이 얼마나 남았는지 알려주기 
        });

        GetButton((int)Buttons.SkillCardImage3).onClick.AddListener(() =>
        {
            // 플레이어 SkillPiercingShoot 실행
            // 실행했을때 해당 쿨 타임 만틈 Skill3CoolTimer 켰다가 끄기 
            // SKill3CoolTimer 자식에 있는 TMP_Text를 통해서 쿨타임이 얼마나 남았는지 알려주기 
        });
        GetButton((int)Buttons.SkillCardImage4).onClick.AddListener(() =>
        {
            // 플레이어 SkillStunShoot 실행
            // 실행했을때 해당 쿨 타임 만틈 Skill4CoolTimer 켰다가 끄기 
            // SKill4CoolTimer 자식에 있는 TMP_Text를 통해서 쿨타임이 얼마나 남았는지 알려주기 
        });
        GetButton((int)Buttons.SkillCardImage5).onClick.AddListener(() =>
        {
            // 플레이어 SkillDash 실행
            // 실행했을때 해당 쿨 타임 만틈 Skill5CoolTimer 켰다가 끄기 
            // SKill5CoolTimer 자식에 있는 TMP_Text를 통해서 쿨타임이 얼마나 남았는지 알려주기 
        });
    }
}
