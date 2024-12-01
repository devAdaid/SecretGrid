using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class L10nText : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text;

    [SerializeField]
    [TextArea]
    private string text_en;

    [SerializeField]
    [TextArea]
    private string text_ko;

    private void Reset()
    {
        text = GetComponent<TMP_Text>();
        text_en = text.text;
        text_ko = text.text;
    }

    private void Start()
    {
        text.text = CommonSingleton.I.IsKoreanLanguage ? text_ko : text_en;
    }
}
