using UnityEngine;

public class OceanCurrent : MonoBehaviour
{
    public static OceanCurrent Instance { get; private set; }

    [Header("Сила течения, м/с")]
    [SerializeField] private float minStrength = 0.4f;
    [SerializeField] private float maxStrength = 1.5f;
    [Range(0f, 1f)]
    [SerializeField] private float calmChance = 0.25f;

    [Header("Смена режима")]
    [SerializeField] private float changeInterval = 45f;
    [SerializeField] private float headingTurnSpeed = 4f;    // град/сек
    [SerializeField] private float strengthChangeSpeed = 0.3f; // м/с за сек

    public Vector3 Velocity { get; private set; }
    public bool IsCalm => Velocity.sqrMagnitude < 0.01f;

    private float heading;        // текущий курс в градусах
    private float targetHeading;
    private float strength;
    private float targetStrength;
    private float timer;

    private void Awake()
    {
        Instance = this;
        heading = targetHeading = Random.Range(0f, 360f);
        strength = targetStrength = Random.Range(minStrength, maxStrength);
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f) PickNewTarget();

        heading  = Mathf.MoveTowardsAngle(heading, targetHeading, headingTurnSpeed * Time.deltaTime);
        strength = Mathf.MoveTowards(strength, targetStrength, strengthChangeSpeed * Time.deltaTime);

        float rad = heading * Mathf.Deg2Rad;
        Velocity = new Vector3(Mathf.Sin(rad), 0f, Mathf.Cos(rad)) * strength;
    }

    private void PickNewTarget()
    {
        timer = changeInterval * Random.Range(0.7f, 1.3f);
        targetHeading = Random.Range(0f, 360f);
        targetStrength = Random.value < calmChance
            ? 0f
            : Random.Range(minStrength, maxStrength);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(Vector3.up * 0.5f, Velocity * 10f);
    }
}