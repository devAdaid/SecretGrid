using UnityEngine;
using UnityEngine.UIElements;

public class SettingMenuController : MonoBehaviour
{
    private UIDocument _document;
    private Button _saveButton;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        _saveButton = _document.rootVisualElement.Q<Button>("SaveButton");
        _saveButton.clicked += OnSaveButtonClicked;
    }

    private void OnSaveButtonClicked()
    {
        if (gameObject.activeSelf)
        {
            _saveButton.clicked -= OnSaveButtonClicked;
            gameObject.SetActive(false);
        }
    }
}
