using UnityEngine;

public class FloatingDebris : MonoBehaviour
{
    [SerializeField] private float bobAmplitude = 0.06f;
    [SerializeField] private float bobFrequency = 1.2f;
    [SerializeField] private float maxSpinSpeed = 15f;
    [SerializeField] private float floatHeight = 0.05f;

    private float baseY;
    private float bobPhase;
    private float spinSpeed;

    private void Start()
    {
        baseY = floatHeight;
        bobPhase = Random.Range(0f, Mathf.PI * 2f);
        spinSpeed = Random.Range(-maxSpinSpeed, maxSpinSpeed);
    }

    private void Update()
    {
        Vector3 pos = transform.position + OceanCurrent.Instance.Velocity * Time.deltaTime;
        pos.y = baseY + Mathf.Sin(Time.time * bobFrequency + bobPhase) * bobAmplitude;
        transform.position = pos;

        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.World);
    }
}