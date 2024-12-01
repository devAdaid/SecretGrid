using RedBlueGames.Tools.TextTyper;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public interface IHeroGameDialogueUI
{
    void PlayText(string text);
    void ApplyChoice(List<D_ChoiceItem> choiceItems);
    void ApplySpeakerName(string speakerName);
    void ApplySpeakerSprite(string spritePath);
    void EndDialogue();
}

public class HeroGameDialogueUI : MonoBehaviour, IHeroGameDialogueUI
{
    [SerializeField]
    private GameObject root;

    [SerializeField]
    private HeroGameButtonBase skipButton;

    [SerializeField]
    private TextTyper textTyper;

    [SerializeField]
    private GameObject textRoot;

    [SerializeField]
    private GameObject choiceRoot;
    [SerializeField]
    private DialogueChoiceItemControl choiceItemPrefab; // 선택지 버튼 프리팹

    [SerializeField]
    private Button dialogueBoxButton;

    [SerializeField]
    private Image speakerImage;

    [SerializeField]
    private TMP_Text speakerNameText;

    private List<DialogueChoiceItemControl> activeChoiceButtons = new List<DialogueChoiceItemControl>();
    private DialoguePlayer dialoguePlayer;

    private int currentCommandIndex = 0;
    private bool isChoiceActive = false; // 선택지가 활성화 상태인지 여부

    private bool isDialoguePlaying;
    private UnityAction onDialogueEnd = null;

    private Dictionary<string, int> commandIndexByLabelName;

    public void PlayDialogue(TextAsset xmlText, bool isSkippable, UnityAction dialogueEndCallback)
    {
        isDialoguePlaying = true;

        onDialogueEnd = dialogueEndCallback;

        root.SetActive(true);
        skipButton.gameObject.SetActive(isSkippable);
        textRoot.SetActive(true);
        choiceRoot.SetActive(false);

        var commands = DialogueXMLSerializer.LoadDialogueFromXML(xmlText.text, out commandIndexByLabelName);
        dialoguePlayer = new DialoguePlayer(commands);
        currentCommandIndex = 0;

        // 첫 번째 명령 실행
        ProceedToNextCommand();
    }

    private void OnDialogueEnd()
    {
        isDialoguePlaying = false;

        root.SetActive(false);
        onDialogueEnd?.Invoke();
    }

    private void Awake()
    {
        textTyper.CharacterPrinted.AddListener((_) => AudioManager.I.PlayTypeSFX(SFXType.Type));
        dialogueBoxButton.onClick.AddListener(RequestProceed);
        skipButton.AddOnClickListener(EndDialogue);
    }

    private void Update()
    {
        if (!isDialoguePlaying)
        {
            return;
        }

        if (isChoiceActive)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RequestProceed();
        }
    }

    private void RequestProceed()
    {
        if (textTyper.IsSkippable())
        {
            textTyper.Skip();
            return;
        }

        ProceedToNextCommand();
    }

    public void PlayText(string text)
    {
        textTyper.TypeText(text);
    }

    public void ApplyChoice(List<D_ChoiceItem> choiceItems)
    {
        // 선택지 UI 표시
        isChoiceActive = true; // 선택지 활성화 플래그 설정
        choiceRoot.SetActive(true);
        textRoot.SetActive(false);

        // TODO: 버튼 3개로 재활용하기
        // 기존 버튼 제거
        foreach (var button in activeChoiceButtons)
        {
            Destroy(button.gameObject);
        }
        activeChoiceButtons.Clear();

        // 선택지 버튼 생성
        for (int i = 0; i < choiceItems.Count; i++)
        {
            var choice = choiceItems[i];
            var button = Instantiate(choiceItemPrefab, choiceRoot.transform);
            button.Apply(choice.Text_Ko, choice.StatReward, choice.LabelName);

            activeChoiceButtons.Add(button);
        }
    }

    public void OnChoiceButtonSelected(string labelName)
    {
        // 선택한 선택지의 명령 인덱스로 이동
        if (commandIndexByLabelName.TryGetValue(labelName, out var index))
        {
            currentCommandIndex = index;
        }
        // 선택지 UI 숨기기
        choiceRoot.SetActive(false);
        textRoot.SetActive(true);
        isChoiceActive = false; // 선택지 활성화 플래그 해제

        // 다음 명령 실행
        ProceedToNextCommand();
    }

    private void ProceedToNextCommand()
    {
        if (!isDialoguePlaying)
        {
            return;
        }

        if (dialoguePlayer == null || currentCommandIndex >= dialoguePlayer.Commands.Count)
        {
            //Debug.Log("대화 종료");
            OnDialogueEnd();
            return;
        }

        var command = dialoguePlayer.Commands[currentCommandIndex];
        command.ApplyUI(this);

        currentCommandIndex++;

        // 다음 커맨드로 이동
        if (command is not D_Text && command is not D_Choice)
        {
            ProceedToNextCommand();
        }
    }

    // GotoCommand 실행 (D_Goto 커맨드에서 호출)
    public void GoToCommand(int commandIndex)
    {
        currentCommandIndex = commandIndex;
        //ProceedToNextCommand(); // 이동 후 다음 명령 실행
    }

    // 화자 이름 갱신 (D_SpeakerName 커맨드에서 호출)
    public void ApplySpeakerName(string speakerName)
    {
        speakerNameText.text = speakerName; // UI 텍스트 갱신
    }
    
    public void ApplySpeakerSprite(string spritePath)
    {
        var sprite = Resources.Load<Sprite>(spritePath);
        speakerImage.sprite = sprite;
    }

    public void EndDialogue()
    {
        OnDialogueEnd();
    }
}
