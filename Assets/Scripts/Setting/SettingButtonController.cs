using UnityEngine;
using UnityEngine.UIElements;

public class SettingButtonController : MonoBehaviour
{
    private UIDocument _document;
    private Button _settingButton;
    public UIDocument settingMenuDocument;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        _settingButton = _document.rootVisualElement.Q<Button>("SettingButton");
        _settingButton.clicked += OnSettingButtonClicked;
    }

    private void OnSettingButtonClicked()
    {
        settingMenuDocument.rootVisualElement.Q<VisualElement>("Panel").style.display = DisplayStyle.Flex;
    }
}
