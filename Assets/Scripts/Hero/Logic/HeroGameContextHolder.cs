public class HeroGameContextHolder : MonoSingleton<HeroGameContextHolder>
{
    public HeroGameContext GameContext { get; private set; }

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        GameContext = new HeroGameContext();
    }
}
