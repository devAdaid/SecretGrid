using TMPro;
using UnityEngine;

public class DialogueChoiceItemControl : MonoBehaviour
{
    [SerializeField]
    private TMP_Text contentText;

    [SerializeField]
    private TMP_Text rewardText;

    [SerializeField]
    private HeroGameButtonBase button;

    private int choiceIndex;

    public void Apply(string content, HeroGameCaseStatReward statReward, int choiceIndex)
    {
        this.choiceIndex = choiceIndex;

        contentText.text = content;
        rewardText.text = statReward.ToUIString();
        rewardText.gameObject.SetActive(!statReward.IsEmpty());
        button.AddOnClickListener(OnClickButton);
    }

    private void OnClickButton()
    {
        HeroGameUI.I.DialogueUI.OnChoiceButtonSelected(choiceIndex);
    }
}
