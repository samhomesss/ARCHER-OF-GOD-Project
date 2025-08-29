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

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButtons(typeof(Buttons));

        return true;
    }
}
