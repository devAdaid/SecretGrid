using UnityEngine;

public class CommonSingleton : PersistentSingleton<CommonSingleton>
{
    public StaticDataHolder StaticDataHolder { get; private set; }
    public HeroGamePersistentContext PersistentContext { get; private set; }

    [field: SerializeField]
    public ToastUI ToastUI { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        StaticDataHolder = new StaticDataHolder();
        PersistentContext = new HeroGamePersistentContext(ToastUI);
    }
}
