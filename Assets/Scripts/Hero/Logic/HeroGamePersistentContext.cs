public class HeroGamePersistentContext
{
    public bool IsSecret3Enabled { get; private set; }
    public bool IsSecret2Enabled { get; private set; }

    private ToastUI toastUI;

    public HeroGamePersistentContext(ToastUI toastUI)
    {
        this.toastUI = toastUI;
    }

    public void SetSecret3Enable(bool enable)
    {
        if (IsSecret3Enabled == enable)
        {
            return;
        }

        AudioManager.I.PlaySFX(SFXType.Secret);

        IsSecret3Enabled = enable;

        toastUI.Show(
            CommonSingleton.I.IsKoreanLanguage ?
            "비밀 활성화: 결과를 빠르게 확인할 수 있게 되었습니다." :
            "SECRET Enabled: You can check the results quickly."
        );
    }

    public void SetSecret2Enable(bool enable)
    {
        if (IsSecret2Enabled == enable)
        {
            return;
        }

        AudioManager.I.PlaySFX(SFXType.Secret);

        IsSecret2Enabled = enable;

        toastUI.Show(
            CommonSingleton.I.IsKoreanLanguage ?
            "비밀 활성화: 비밀 스탯이 0이 되지 않습니다.." :
            "SECRET Enabled: The 'secret' stat will not be reduced to 0."
        );
    }
}
