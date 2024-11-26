using RedBlueGames.Tools.TextTyper;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public interface IHeroGameDialogueUI
{
    void PlayText(string text);
    void ApplyChoice(List<D_ChoiceItem> choiceItems);
    void UpdateSpeakerName(string speakerName);
}

public class HeroGameDialogueUI : MonoBehaviour, IHeroGameDialogueUI
{
    [SerializeField]
    private TextTyper textTyper;

    [SerializeField]
    private GameObject textRoot;

    [SerializeField]
    private GameObject choiceRoot;
    [SerializeField]
    private DialogueChoiceItemControl choiceItemPrefab; // 선택지 버튼 프리팹

    [SerializeField]
    private TMP_Text speakerNameText; // 화자 이름 표시용 텍스트 UI

    private List<DialogueChoiceItemControl> activeChoiceButtons = new List<DialogueChoiceItemControl>();
    private DialoguePlayer dialoguePlayer;

    private int currentCommandIndex = 0;
    private bool isChoiceActive = false; // 선택지가 활성화 상태인지 여부

    private void Start()
    {
        choiceRoot.SetActive(false); // 초기 선택지 UI 비활성화
    }

    private void Update()
    {
        if (isChoiceActive)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (textTyper.IsSkippable())
            {
                textTyper.Skip();
                return;
            }

            ProceedToNextCommand();
        }
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

        // 기존 버튼 제거
        foreach (var button in activeChoiceButtons)
        {
            Destroy(button);
        }
        activeChoiceButtons.Clear();

        // 선택지 버튼 생성
        for (int i = 0; i < choiceItems.Count; i++)
        {
            var choice = choiceItems[i];
            var button = Instantiate(choiceItemPrefab, choiceRoot.transform);
            int index = choice.CommandIndex;
            button.Apply(choice.Text_En, HeroGameCaseStatReward.Empty, index);

            activeChoiceButtons.Add(button);
        }
    }

    public void OnChoiceButtonSelected(int choiceIndex)
    {
        // 선택한 선택지의 명령 인덱스로 이동
        currentCommandIndex = choiceIndex;

        // 선택지 UI 숨기기
        choiceRoot.SetActive(false);
        textRoot.SetActive(true);
        isChoiceActive = false; // 선택지 활성화 플래그 해제

        // 다음 명령 실행
        ProceedToNextCommand();
    }

    private void ProceedToNextCommand()
    {
        if (dialoguePlayer == null || currentCommandIndex >= dialoguePlayer.Commands.Count)
        {
            //Debug.Log("대화 종료");
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

    public void PlayDialogue(List<IDialogueCommand> commands)
    {
        dialoguePlayer = new DialoguePlayer(commands);
        currentCommandIndex = 0;

        // 첫 번째 명령 실행
        ProceedToNextCommand();
    }

    // GotoCommand 실행 (D_Goto 커맨드에서 호출)
    public void GoToCommand(int gotoIndex)
    {
        currentCommandIndex = gotoIndex;
        ProceedToNextCommand(); // 이동 후 다음 명령 실행
    }

    // 화자 이름 갱신 (D_SpeakerName 커맨드에서 호출)
    public void UpdateSpeakerName(string speakerName)
    {
        speakerNameText.text = speakerName; // UI 텍스트 갱신
    }
}
