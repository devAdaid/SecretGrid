using UnityEngine;

public class DialogueTest : MonoBehaviour
{
    [SerializeField]
    private HeroGameDialogueUI dialogueUI;

    [SerializeField]
    private TextAsset dialogueData;

    private void Start()
    {
        var loadedCommands = DialogueXMLSerializer.LoadDialogueFromXML(dialogueData.text);

        // 로드된 대화 명령 출력
        foreach (var command in loadedCommands)
        {
            //Debug.Log(command.GetType().Name);
        }

        dialogueUI.PlayDialogue(dialogueData, true, null);
    }
}
