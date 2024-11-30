using System.Collections.Generic;
using System.Xml;
using UnityEngine;

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

                    if (choice.CommandIndex >= 0)
                    {
                        choiceElement.SetAttribute("CommandIndex", choice.CommandIndex.ToString());
                    }
                    choicesElement.AppendChild(choiceElement);
                }
                commandElement.AppendChild(choicesElement);
            }
            else if (command is D_Goto dGoto)
            {
                commandElement = doc.CreateElement("Goto");
                commandElement.SetAttribute("CommandIndex", dGoto.commandIndex.ToString());
            }
            else if (command is D_SpeakerName dSpeakerName)
            {
                commandElement = doc.CreateElement("SpeakerName");
                commandElement.SetAttribute("Text_Ko", dSpeakerName.Text_Ko);
                commandElement.SetAttribute("Text_En", dSpeakerName.Text_En);
            }
            //TODO 명령어 추가

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
                    var commandIndexAttribute = choiceElement.GetAttribute("CommandIndex");

                    var strAttribute = choiceElement.GetAttribute("STR");
                    var aglAttribute = choiceElement.GetAttribute("AGL");
                    var intAttribute = choiceElement.GetAttribute("INT");
                    var secretAttribute = choiceElement.GetAttribute("Secret");

                    if (!int.TryParse(commandIndexAttribute, out var commandIndex))
                    {
                        commandIndex = -1;
                    }
                    int.TryParse(strAttribute, out var strength);
                    int.TryParse(aglAttribute, out var agility);
                    int.TryParse(intAttribute, out var intelligence);
                    int.TryParse(secretAttribute, out var secret);
                    choices.Add(new D_ChoiceItem(textKo, textEn, commandIndex, new HeroGameCaseStatReward(strength, agility, intelligence, secret)));
                }
                commands.Add(new D_Choice(choices));
            }
            else if (commandElement.Name == "Goto")
            {
                var commandIndex = int.Parse(commandElement.GetAttribute("CommandIndex"));
                commands.Add(new D_Goto(commandIndex));
            }
            else if (commandElement.Name == "SpeakerName")
            {
                var textKo = commandElement.GetAttribute("Text_Ko");
                var textEn = commandElement.GetAttribute("Text_En");
                commands.Add(new D_SpeakerName(textKo, textEn));
            }
            else if (commandElement.Name == "SpeakerSprite")
            {
                var spritePath = commandElement.GetAttribute("SpritePath");
                commands.Add(new D_SpeakerSprite(spritePath));
            }
            else if (commandElement.Name == "PauseBgm")
            {
                commands.Add(new D_PauseBgm());
            }
            else if (commandElement.Name == "ResumeBgm")
            {
                commands.Add(new D_ResumeBgm());
            }
        }

        return commands;
    }
}
