using UnityEngine;

public class PanelShake : MonoBehaviour
{
    public int index;
    public float shakeAmount = 1f;
    public float shakeSpeed = 1f;

    private Vector3 initialPosition;

    void Start()
    {
        // �ʱ� ��ġ ����
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        // Perlin Noise�� index�� �߰��� ���� �ٸ� ��鸲 ����
        float offsetX = (Mathf.PerlinNoise(Time.time * shakeSpeed, index) - 0.5f) * shakeAmount;
        float offsetY = (Mathf.PerlinNoise(index, Time.time * shakeSpeed) - 0.5f) * shakeAmount;

        // ��鸲 ����
        transform.localPosition = initialPosition + new Vector3(offsetX, offsetY, 0);
    }
}