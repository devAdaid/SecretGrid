using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    [SerializeField]
    private string sceneName;

    private void Awake()
    {
        DeactivateGameObject();
    }

    [Conditional("PRODUCTION")]
    void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }

    public void OnClick()
    {
        SceneManager.LoadScene(sceneName);
    }
}
