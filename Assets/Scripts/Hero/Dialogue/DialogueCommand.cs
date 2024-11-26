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
        ui.PlayText(Text_En);
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
    public readonly int CommandIndex;

    public D_ChoiceItem(string text_ko, string text_en, int commandIndex)
    {
        Text_Ko = text_ko;
        Text_En = text_en;
        CommandIndex = commandIndex;
    }
}
public class D_Goto : IDialogueCommand
{
    public readonly int gotoIndex;

    public D_Goto(int index)
    {
        gotoIndex = index;
    }

    public void ApplyUI(IHeroGameDialogueUI ui)
    {
        var player = ui as HeroGameDialogueUI; // UI를 HeroGameDialogueUI로 캐스팅
        if (player != null)
        {
            player.GoToCommand(gotoIndex); // 즉시 지정된 인덱스로 이동
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
        ui.UpdateSpeakerName(Text_En);
    }
}