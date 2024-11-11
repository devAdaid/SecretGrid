using UnityEngine;
using UnityEngine.UIElements;

public class SettingButtonController : MonoBehaviour
{
    public GameObject settingMenu;
    private UIDocument _document;
    private Button _settingButton;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        _settingButton = _document.rootVisualElement.Q<Button>("SettingButton");
        _settingButton.clicked += OnSettingButtonClicked;
    }

    private void OnSettingButtonClicked()
    {
        if (!settingMenu.activeSelf)
        {
            settingMenu.SetActive(true);
        }
    }
}
