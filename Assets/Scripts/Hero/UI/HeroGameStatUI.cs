using UnityEngine;

public class HeroGameStatUI : MonoBehaviour
{
    [SerializeField]
    private HeroGameStatItemUIControl strengthStatControl;

    [SerializeField]
    private HeroGameStatItemUIControl agilityStatControl;

    [SerializeField]
    private HeroGameStatItemUIControl intelligenceStatControl;

    [SerializeField]
    private HeroGameStatItemUIControl secretStatControl;


    public void Apply(HeroPlayerContext playerContext)
    {
        strengthStatControl.Apply(playerContext.Strength);
        agilityStatControl.Apply(playerContext.Agility);
        intelligenceStatControl.Apply(playerContext.Intelligence);
        secretStatControl.Apply(playerContext.Secret);
    }
}
