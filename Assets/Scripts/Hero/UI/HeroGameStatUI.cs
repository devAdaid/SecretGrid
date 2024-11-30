using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroGameStatUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text dayText;

    [SerializeField]
    private HeroGameStatItemUIControl strengthStatControl;

    [SerializeField]
    private HeroGameStatItemUIControl agilityStatControl;

    [SerializeField]
    private HeroGameStatItemUIControl intelligenceStatControl;

    [SerializeField]
    private HeroGameStatItemUIControl secretStatControl;

    [SerializeField]
    private Button settingButton;

    private void Awake()
    {
        settingButton.onClick.AddListener(OnSettingButtonClicked);
    }
    
    public void ApplyWithRemainPhase(int day, int remainPhase, HeroPlayerContext playerContext)
    {
        dayText.text = $"Day {day} (Remain {remainPhase})";

        ApplyStat(playerContext);
    }

    public void Apply(int day, HeroPlayerContext playerContext)
    {
        //TODO: 다음 메인 스토리 데이 표시
        dayText.text = $"Day {day}";

        ApplyStat(playerContext);
    }
    
    private void ApplyStat(HeroPlayerContext playerContext)
    {
        strengthStatControl.Apply(playerContext.Strength);
        agilityStatControl.Apply(playerContext.Agility);
        intelligenceStatControl.Apply(playerContext.Intelligence);
        secretStatControl.Apply(playerContext.Secret);
    }

    private void OnSettingButtonClicked()
    {
        SettingUI.I.Show();
        AudioManager.I.PlaySFX(SFXType.Select);
    }
}
