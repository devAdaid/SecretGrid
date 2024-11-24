using UnityEngine;

public class PanelShake : MonoBehaviour
{
    public float shakeAmount = 1f;
    public float shakeSpeed = 1f;

    private int randomInt = 0;
    private Vector3 initialPosition;

    private void OnEnable()
    {
        randomInt = Random.Range(0, 100);
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        // Perlin Noise에 index를 추가해 서로 다른 흔들림 생성
        float offsetX = (Mathf.PerlinNoise(Time.time * shakeSpeed, randomInt) - 0.5f) * shakeAmount;
        float offsetY = (Mathf.PerlinNoise(randomInt, Time.time * shakeSpeed) - 0.5f) * shakeAmount;

        // 흔들림 적용
        transform.localPosition = initialPosition + new Vector3(offsetX, offsetY, 0);
    }
}