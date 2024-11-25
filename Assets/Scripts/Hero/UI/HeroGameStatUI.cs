using TMPro;
using UnityEngine;

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


    public void Apply(int day, HeroPlayerContext playerContext)
    {
        dayText.text = $"Day {day}";

        strengthStatControl.Apply(playerContext.Strength);
        agilityStatControl.Apply(playerContext.Agility);
        intelligenceStatControl.Apply(playerContext.Intelligence);
        secretStatControl.Apply(playerContext.Secret);
    }
}
