using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FixedAspectRatio : MonoBehaviour
{
    public float targetAspect = 16.0f / 9.0f; // 원하는 화면 비율 (16:9)

    void Update()
    {
        // 현재 화면의 비율 계산
        float windowAspect = (float)Screen.width / (float)Screen.height;

        // 비율 차이 계산
        float scaleHeight = windowAspect / targetAspect;

        // 카메라 설정 조정
        Camera camera = GetComponent<Camera>();

        if (scaleHeight < 1.0f)
        {
            // 화면 높이가 더 큰 경우 (Letterbox 추가)
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            camera.rect = rect;
        }
        else
        {
            // 화면 너비가 더 큰 경우 (Pillarbox 추가)
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = camera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }
}