using System;
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

            switch (command)
            {
                case D_Text dText:
                    commandElement = doc.CreateElement("Text");
                    commandElement.SetAttribute("Text_Ko", dText.Text_Ko);
                    commandElement.SetAttribute("Text_En", dText.Text_En);
                    break;
                case D_Choice dChoice:
                    {
                        commandElement = doc.CreateElement("Choice");
                        var choicesElement = doc.CreateElement("Choices");
                        foreach (var choice in dChoice.Choices)
                        {
                            var choiceElement = doc.CreateElement("ChoiceItem");
                            choiceElement.SetAttribute("Text_Ko", choice.Text_Ko);
                            choiceElement.SetAttribute("Text_En", choice.Text_En);
                            choiceElement.SetAttribute("LabelName", choice.LabelName);
                            choicesElement.AppendChild(choiceElement);
                        }
                        commandElement.AppendChild(choicesElement);
                        break;
                    }
                case D_Goto dGoto:
                    commandElement = doc.CreateElement("Goto");
                    commandElement.SetAttribute("CommandIndex", dGoto.commandIndex.ToString());
                    break;
                case D_SpeakerName dSpeakerName:
                    commandElement = doc.CreateElement("SpeakerName");
                    commandElement.SetAttribute("Text_Ko", dSpeakerName.Text_Ko);
                    commandElement.SetAttribute("Text_En", dSpeakerName.Text_En);
                    break;
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

    public static List<IDialogueCommand> LoadDialogueFromXML(string xmlText, out Dictionary<string, int> indexByLabel)
    {
        var commands = new List<IDialogueCommand>();
        var doc = new XmlDocument();
        doc.LoadXml(xmlText);

        var rootElement = doc.DocumentElement;
        indexByLabel = new Dictionary<string, int>();

        var index = 0;
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
                    var labelName = choiceElement.GetAttribute("LabelName");

                    var strAttribute = choiceElement.GetAttribute("STR");
                    var aglAttribute = choiceElement.GetAttribute("AGL");
                    var intAttribute = choiceElement.GetAttribute("INT");
                    var secretAttribute = choiceElement.GetAttribute("Secret");

                    int.TryParse(strAttribute, out var strength);
                    int.TryParse(aglAttribute, out var agility);
                    int.TryParse(intAttribute, out var intelligence);
                    int.TryParse(secretAttribute, out var secret);
                    choices.Add(new D_ChoiceItem(textKo, textEn, labelName, new HeroGameCaseStatReward(strength, agility, intelligence, secret)));
                }
                commands.Add(new D_Choice(choices));
            }
            else if (commandElement.Name == "Label")
            {
                var name = commandElement.GetAttribute("Name");
                if (string.IsNullOrEmpty(name))
                {
                    Debug.LogError("Label.Name이 빈값입니다.");
                }
                else
                {
                    commands.Add(new D_Label(name));
                    indexByLabel.Add(name, index);
                }
            }
            else if (commandElement.Name == "Goto")
            {
                var commandIndex = int.Parse(commandElement.GetAttribute("CommandIndex"));
                commands.Add(new D_Goto(commandIndex));
            }
            else if (commandElement.Name == "Flag")
            {
                var name = commandElement.GetAttribute("Name");
                commands.Add(new D_Flag(name));
            }
            else if (commandElement.Name == "EndDialogue")
            {
                commands.Add(new D_EndDialogue());
            }
            else if (commandElement.Name == "SpeakerName")
            {
                var textKo = commandElement.GetAttribute("Text_Ko");
                var textEn = commandElement.GetAttribute("Text_En");
                commands.Add(new D_SpeakerName(textKo, textEn));
            }
            else if (commandElement.Name == "SpeakerSprite")
            {
                var spritePath_ko = commandElement.GetAttribute("SpritePath_Ko");
                var spritePath_en = commandElement.GetAttribute("SpritePath_En");
                commands.Add(new D_SpeakerSprite(spritePath_ko, spritePath_en));
            }
            else if (commandElement.Name == "PauseBgm")
            {
                commands.Add(new D_PauseBgm());
            }
            else if (commandElement.Name == "ResumeBgm")
            {
                commands.Add(new D_ResumeBgm());
            }
            else if (commandElement.Name == "PlayBgm")
            {
                var typeAttribute = commandElement.GetAttribute("Type");

                Enum.TryParse<BGMType>(typeAttribute, out var type);
                commands.Add(new D_PlayBgm(type));
            }
            else if (commandElement.Name == "PlaySfx")
            {
                var typeAttribute = commandElement.GetAttribute("Type");

                Enum.TryParse<SFXType>(typeAttribute, out var type);
                commands.Add(new D_PlaySfx(type));
            }
            index++;
        }

        return commands;
    }
}
