using System.Collections.Generic;
using UnityEngine;

public class DialoguePlayer
{
    public List<IDialogueCommand> Commands = new List<IDialogueCommand>();

    public DialoguePlayer(List<IDialogueCommand> commands)
    {
        this.Commands = commands;
    }

    public void ExecuteCommand(int commandIndex, IHeroGameDialogueUI ui)
    {
        if (commandIndex < 0 || commandIndex >= Commands.Count)
        {
            Debug.LogError($"commandIndex가 범위를 벗어남. commandIndex[{commandIndex}] Commands.Count[{Commands.Count}]");
            return;
        }

        Commands[commandIndex].ApplyUI(ui);
    }
}