using UnityEngine;

public class PanelShake : MonoBehaviour
{
    public int index;
    public float shakeAmount = 1f;
    public float shakeSpeed = 1f;

    private Vector3 initialPosition;

    void Start()
    {
        // 초기 위치 저장
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        // Perlin Noise에 index를 추가해 서로 다른 흔들림 생성
        float offsetX = (Mathf.PerlinNoise(Time.time * shakeSpeed, index) - 0.5f) * shakeAmount;
        float offsetY = (Mathf.PerlinNoise(index, Time.time * shakeSpeed) - 0.5f) * shakeAmount;

        // 흔들림 적용
        transform.localPosition = initialPosition + new Vector3(offsetX, offsetY, 0);
    }
}