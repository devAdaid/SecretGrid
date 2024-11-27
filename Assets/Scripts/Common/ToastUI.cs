using System.Collections;
using TMPro;
using UnityEngine;

public class ToastUI : MonoBehaviour
{
    [SerializeField]
    private GameObject root;

    [SerializeField]
    private TMP_Text toastText;

    [SerializeField]
    private float showTime;

    private bool isShowing;

    public void Show(string text)
    {
        if (isShowing)
        {
            StopAllCoroutines();
        }

        isShowing = true;

        root.gameObject.SetActive(true);
        toastText.text = text;

        StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(showTime);
        root.gameObject.SetActive(false);
        isShowing = false;
    }
}
