public class CommonSingleton : PersistentSingleton<CommonSingleton>
{
    public StaticDataHolder StaticDataHolder { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        StaticDataHolder = new StaticDataHolder();
    }
}
