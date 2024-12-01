using System.Collections.Generic;

public interface IDialogueCommand
{
    void ApplyUI(IHeroGameDialogueUI ui);
}

public class D_Text : IDialogueCommand
{
    public readonly string Text_Ko;
    public readonly string Text_En;

    public D_Text(string text_ko, string text_en)
    {
        Text_Ko = text_ko;
        Text_En = text_en;
    }

    public void ApplyUI(IHeroGameDialogueUI ui)
    {
        ui.PlayText(Text_Ko);
    }
}

public class D_Choice : IDialogueCommand
{
    public readonly List<D_ChoiceItem> Choices;

    public D_Choice(List<D_ChoiceItem> choices)
    {
        Choices = choices;
    }

    public void ApplyUI(IHeroGameDialogueUI ui)
    {
        ui.ApplyChoice(Choices);
    }
}


public class D_ChoiceItem
{
    public readonly string Text_Ko;
    public readonly string Text_En;
    public readonly string LabelName;
    public readonly HeroGameCaseStatReward StatReward;

    public D_ChoiceItem(string text_ko, string text_en, string labelName, HeroGameCaseStatReward statReward)
    {
        Text_Ko = text_ko;
        Text_En = text_en;
        LabelName = labelName;
        StatReward = statReward;
    }
}

public class D_Label : IDialogueCommand
{
    public readonly string Name;
    
    public D_Label(string name)
    {
        Name = name;
    }

    public void ApplyUI(IHeroGameDialogueUI ui)
    {
    }
}

public class D_Flag : IDialogueCommand
{
    public readonly string Name;

    public D_Flag(string name)
    {
        Name = name;
    }

    public void ApplyUI(IHeroGameDialogueUI ui)
    {
        HeroGameContextHolder.I.GameContext.AddDialogueFlag(Name);
    }
}

public class D_Goto : IDialogueCommand
{
    public readonly int commandIndex;

    public D_Goto(int index)
    {
        commandIndex = index;
    }

    public void ApplyUI(IHeroGameDialogueUI ui)
    {
        var player = ui as HeroGameDialogueUI; // UI를 HeroGameDialogueUI로 캐스팅
        if (player != null)
        {
            player.GoToCommand(commandIndex); // 즉시 지정된 인덱스로 이동
        }
    }
}

public class D_SpeakerName : IDialogueCommand
{
    public readonly string Text_Ko;
    public readonly string Text_En;

    public D_SpeakerName(string text_ko, string text_en)
    {
        Text_Ko = text_ko;
        Text_En = text_en;
    }

    public void ApplyUI(IHeroGameDialogueUI ui)
    {
        ui.ApplySpeakerName(Text_Ko);
    }
}

public class D_SpeakerSprite : IDialogueCommand
{
    public readonly string SpritePath_Ko;
    public readonly string SpritePath_En;

    public D_SpeakerSprite(string spritePath_ko, string spritePath_en)
    {
        SpritePath_Ko = spritePath_ko;
        SpritePath_En = spritePath_en;
    }

    public void ApplyUI(IHeroGameDialogueUI ui)
    {
        ui.ApplySpeakerSprite(SpritePath_Ko);
    }
}

public class D_PauseBgm : IDialogueCommand
{
    public D_PauseBgm()
    {
    }

    public void ApplyUI(IHeroGameDialogueUI ui)
    {
        AudioManager.I.PauseBGM();
    }
}

public class D_ResumeBgm : IDialogueCommand
{
    public D_ResumeBgm()
    {
    }

    public void ApplyUI(IHeroGameDialogueUI ui)
    {
        AudioManager.I.ResumeBGM();
    }
}


public class D_EndDialogue : IDialogueCommand
{
    public D_EndDialogue()
    {
    }

    public void ApplyUI(IHeroGameDialogueUI ui)
    {
        ui.EndDialogue();
    }
}