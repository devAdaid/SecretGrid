using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct HeroGameCaseSelectionUIControlData
{

}


public class HeroGameCaseSelectionUIControl : MonoBehaviour
{
    [SerializeField]
    private TMP_Text descriptionText;

    [SerializeField]
    private Button confirmButton;

    [SerializeField]
    private TMP_Text confirmButtonText;
}
