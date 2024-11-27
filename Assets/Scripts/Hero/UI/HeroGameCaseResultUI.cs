using System.Collections;
using TMPro;
using UnityEngine;

public class HeroGameCaseResultUI : MonoBehaviour
{
    [SerializeField]
    private GameObject root;

    [SerializeField]
    private TMP_Text loadingText;

    [SerializeField]
    private GameObject resultTextRoot;

    [SerializeField]
    private TMP_Text resultText;

    [SerializeField]
    private TMP_Text resultStateText;

    [SerializeField]
    private TMP_Text rewardText;

    [SerializeField]
    private float spaceCheckMaxInterval = 0.5f;

    [SerializeField]
    private HeroGameButtonBase nextButton;

    private HeroGameCaseSelectionUIControlData data;

    private float resultShowTime = 0f;
    private int continuousClickCount = 0;
    private float lastClickTime = 0;

    private bool isWaiting;

    private static readonly int SPACE_CHECK_COUNT = 10;

    private void Awake()
    {
        nextButton.AddOnClickListener(OnClickNextButton);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (nextButton.gameObject.activeInHierarchy)
            {
                continuousClickCount = 0;
                OnNextButtonRequested();
            }
            else if (isWaiting)
            {
                UpdateContinuousClickCount();

                if (CommonSingleton.I.PersistentContext.IsSecret3Enabled)
                {
                    if (continuousClickCount >= 2)
                    {
                        StopAllCoroutines();

                        resultShowTime = Time.time;
                        isWaiting = false;
                        ProcessSelect();
                    }
                }
                else
                {
                    if (continuousClickCount >= SPACE_CHECK_COUNT)
                    {
                        CommonSingleton.I.PersistentContext.SetSecret3Enable(true);
                        continuousClickCount = 0;
                    }
                }
            }
        }
    }

    private void UpdateContinuousClickCount()
    {
        float currentTime = Time.time;
        lastClickTime = currentTime;

        if (currentTime - lastClickTime <= spaceCheckMaxInterval)
        {
            continuousClickCount++;
        }
        else
        {
            continuousClickCount = 1;
        }
    }

    private void OnNextButtonRequested()
    {
        var now = Time.time;
        if (now < resultShowTime + 0.5f)
        {
            return;
        }

        OnClickNextButton();
    }

    public void Show(HeroGameCaseSelectionUIControlData data)
    {
        this.data = data;

        root.SetActive(true);

        loadingText.gameObject.SetActive(true);
        resultTextRoot.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);

        StartCoroutine(UpdateLoadingText());
    }

    public void Hide()
    {
        root.SetActive(false);
    }

    private IEnumerator UpdateLoadingText()
    {
        isWaiting = true;

        int dotCount = 0;
        float elapsedTime = 0f;
        float duration = 2f;

        while (elapsedTime < duration)
        {
            loadingText.text = "Working" + new string('.', dotCount);
            dotCount = (dotCount + 1) % 4;
            AudioManager.I.PlaySFX(SFXType.Wait);
            yield return new WaitForSeconds(0.5f);
            elapsedTime += 0.5f;
        }

        resultShowTime = Time.time;

        isWaiting = false;

        ProcessSelect();
    }

    private void ProcessSelect()
    {
        loadingText.gameObject.SetActive(false);
        resultTextRoot.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(true);

        var success = HeroGameContextHolder.I.SelectCaseSelection(data.Selection.CaseIndex, data.Selection.SelectionIndex);
        if (success)
        {
            resultText.text = "Success!!";
            resultStateText.text = "You successfully solved the case\n" +
                "without being identified.";
            rewardText.text = data.Selection.StatReward.ToUIString();
        }
        else
        {
            //TODO
            resultText.text = "Fail.";
            resultStateText.text = "TODO: fail message.";
            rewardText.text = $"{HeroGameStatType.Secret.ToIconString()}-{data.Selection.StaticData.DecreaseSecretValueOnFail}";
        }
    }

    private void OnClickNextButton()
    {
        AudioManager.I.PlaySFX(SFXType.Select);
        HeroGameContextHolder.I.ProcessNext();
    }
}
