using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using static D_Goto;

public class DialogueXMLSerializer : MonoBehaviour
{
    public static void SaveDialogueToXML(List<IDialogueCommand> commands, string filePath)
    {
        var doc = new XmlDocument();
        var rootElement = doc.CreateElement("DialogueCommands");

        foreach (var command in commands)
        {
            XmlElement commandElement = null;

            if (command is D_Text dText)
            {
                commandElement = doc.CreateElement("Text");
                commandElement.SetAttribute("Text_Ko", dText.Text_Ko);
                commandElement.SetAttribute("Text_En", dText.Text_En);
            }
            else if (command is D_Choice dChoice)
            {
                commandElement = doc.CreateElement("Choice");
                var choicesElement = doc.CreateElement("Choices");
                foreach (var choice in dChoice.Choices)
                {
                    var choiceElement = doc.CreateElement("ChoiceItem");
                    choiceElement.SetAttribute("Text_Ko", choice.Text_Ko);
                    choiceElement.SetAttribute("Text_En", choice.Text_En);
                    choiceElement.SetAttribute("CommandIndex", choice.CommandIndex.ToString());
                    choicesElement.AppendChild(choiceElement);
                }
                commandElement.AppendChild(choicesElement);
            }
            else if (command is D_Goto dGoto)
            {
                commandElement = doc.CreateElement("Goto");
                commandElement.SetAttribute("GotoIndex", dGoto.gotoIndex.ToString());
            }
            else if (command is D_SpeakerName dSpeakerName)
            {
                commandElement = doc.CreateElement("SpeakerName");
                commandElement.SetAttribute("Text_Ko", dSpeakerName.Text_Ko);
                commandElement.SetAttribute("Text_En", dSpeakerName.Text_En);
            }

            if (commandElement != null)
            {
                rootElement.AppendChild(commandElement);
            }
        }

        doc.AppendChild(rootElement);
        doc.Save(filePath);
    }

    public static List<IDialogueCommand> LoadDialogueFromXML(string xmlText)
    {
        var commands = new List<IDialogueCommand>();
        var doc = new XmlDocument();
        doc.LoadXml(xmlText);

        var rootElement = doc.DocumentElement;
        foreach (XmlElement commandElement in rootElement.ChildNodes)
        {
            if (commandElement.Name == "Text")
            {
                var textKo = commandElement.GetAttribute("Text_Ko");
                var textEn = commandElement.GetAttribute("Text_En");
                commands.Add(new D_Text(textKo, textEn));
            }
            else if (commandElement.Name == "Choice")
            {
                var choices = new List<D_ChoiceItem>();
                foreach (XmlElement choiceElement in commandElement["Choices"].ChildNodes)
                {
                    var textKo = choiceElement.GetAttribute("Text_Ko");
                    var textEn = choiceElement.GetAttribute("Text_En");
                    var commandIndex = int.Parse(choiceElement.GetAttribute("CommandIndex"));
                    choices.Add(new D_ChoiceItem(textKo, textEn, commandIndex));
                }
                commands.Add(new D_Choice(choices));
            }
            else if (commandElement.Name == "Goto")
            {
                var gotoIndex = int.Parse(commandElement.GetAttribute("GotoIndex"));
                commands.Add(new D_Goto(gotoIndex));
            }
            else if (commandElement.Name == "SpeakerName")
            {
                var textKo = commandElement.GetAttribute("Text_Ko");
                var textEn = commandElement.GetAttribute("Text_En");
                commands.Add(new D_SpeakerName(textKo, textEn));
            }
        }

        return commands;
    }
}
