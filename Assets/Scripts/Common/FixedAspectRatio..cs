using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Camera))]
public class FixedAspectRatio : MonoBehaviour
{
    public float targetAspect = 16.0f / 9.0f; // 고정할 화면 비율

    void Update()
    {
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        Camera camera = GetComponent<Camera>();

        if (scaleHeight < 1.0f) // 세로 레터박스 (상하 검정)
        {
            float size = (1.0f - scaleHeight) / 2.0f;
            camera.rect = new Rect(0, size, 1, scaleHeight);
        }
        else // 가로 필러박스 (좌우 검정)
        {
            float scaleWidth = 1.0f / scaleHeight;
            float size = (1.0f - scaleWidth) / 2.0f;
            camera.rect = new Rect(size, 0, scaleWidth, 1);
        }
    }
    void OnPreCull()
    {
        GL.Clear(true, true, Color.black); // 레터박스 영역을 강제로 검정색으로 칠함
    }

    void OnEnable()
    {
#if !UNITY_EDITOR

        RenderPipelineManager.beginCameraRendering += RenderPipelineManager_endCameraRendering;
#endif
    }

    void OnDisable()
    {
#if !UNITY_EDITOR

        RenderPipelineManager.beginCameraRendering -= RenderPipelineManager_endCameraRendering;
#endif

    }

    private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        GL.Clear(true, true, Color.black);
    }
}