using System.Collections;
using TMPro;
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

    GameObject[] _skillCoolTimers;
    SkillBase2D[] _playerSkills;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButtons(typeof(Buttons));
        BindObjects(typeof(GameObjects));

        int count = System.Enum.GetValues(typeof(GameObjects)).Length;
        _skillCoolTimers = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            _skillCoolTimers[i] = GetObject(i);
            _skillCoolTimers[i].SetActive(false);
        }

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            _playerSkills = new SkillBase2D[5];
            _playerSkills[0] = player.GetComponent<SkillJumpTripleShot2D>();
            _playerSkills[1] = player.GetComponent<SkillVerticalRainStrike2D>();
            _playerSkills[2] = player.GetComponent<SkillPiercingLine2D>();
            _playerSkills[3] = player.GetComponent<SkillBigShoot2D>();
            _playerSkills[4] = player.GetComponent<SkillDash2D>();
        }

        return true;
    }

    private void Start()
    {
        GetButton((int)Buttons.SkillCardImage1).onClick.AddListener(() =>
        {
            TryCastSkill(0);
        });

        GetButton((int)Buttons.SkillCardImage2).onClick.AddListener(() =>
        {
            TryCastSkill(1);
        });

        GetButton((int)Buttons.SkillCardImage3).onClick.AddListener(() =>
        {
            TryCastSkill(2);
        });
        GetButton((int)Buttons.SkillCardImage4).onClick.AddListener(() =>
        {
            TryCastSkill(4);
        });
        GetButton((int)Buttons.SkillCardImage5).onClick.AddListener(() =>
        {
            TryCastSkill(3);
        });
    }

    void TryCastSkill(int index)
    {
        if (_playerSkills == null || index < 0 || index >= _playerSkills.Length)
            return;

        SkillBase2D skill = _playerSkills[index];
        if (skill != null && skill.TryCast())
            StartCoolTimer(index, skill);
    }

    public void StartCoolTimer(int index, SkillBase2D skill)
    {
        if (skill == null || index < 0 || index >= _skillCoolTimers.Length)
            return;

        StartCoroutine(CoolTimeRoutine(skill, _skillCoolTimers[index]));
    }

    IEnumerator CoolTimeRoutine(SkillBase2D skill, GameObject timer)
    {
        if (timer == null)
            yield break;

        TMP_Text text = timer.GetComponentInChildren<TMP_Text>();
        timer.SetActive(true);

        while (!skill.IsReady)
        {
            if (text != null)
                text.text = $"{skill.Remaining:F1}";
            yield return null;
        }

        timer.SetActive(false);
    }
}