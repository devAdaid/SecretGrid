public class HeroGamePersistentContext
{
    public bool IsSecret3Enabled { get; private set; }

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
        //TODO: 텍스트
        toastUI.Show("SECRET 3 Enabled: 결과 UI를 빠르게 넘길 수 있게 되었습니다.");
    }
}
