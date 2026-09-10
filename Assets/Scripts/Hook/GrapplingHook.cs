using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    private enum State { Idle, Flying, Landed, Reeling }

    [Header("Бросок")]
    [SerializeField] private GameObject hookPrefab;
    [SerializeField] private float throwSpeed = 14f;
    [SerializeField] private float throwUpAngle = 20f;
    [SerializeField] private float gravity = -12f;

    [Header("Возврат")]
    [SerializeField] private float reelSpeed = 6f;
    [SerializeField] private float collectDistance = 1.2f;

    [Header("Ссылки")]
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private Inventory inventory;

    private State state = State.Idle;
    private Transform hook;
    private LineRenderer rope;
    private Vector3 velocity;
    private InputReader input;

    private void Awake() => input = GetComponent<InputReader>();

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                if (input.ThrowPressed) Throw();
                break;
            case State.Flying:
                FlyStep();
                break;
            case State.Landed:
                hook.position += OceanCurrent.Instance.Velocity * Time.deltaTime;
                if (input.ThrowPressed) state = State.Reeling;
                break;
            case State.Reeling:
                ReelStep();
                break;
        }

        if (hook != null) UpdateRope();
    }

    private Vector3 HandPos() =>
        cameraHolder.position
        + cameraHolder.forward * 0.5f
        + cameraHolder.right * 0.3f
        - cameraHolder.up * 0.3f;

    private void Throw()
    {
        hook = Instantiate(hookPrefab, HandPos(), Quaternion.identity).transform;
        rope = hook.GetComponent<LineRenderer>();

        Vector3 dir = Quaternion.AngleAxis(-throwUpAngle, cameraHolder.right)
                      * cameraHolder.forward;
        velocity = dir * throwSpeed;
        state = State.Flying;
    }

    private void FlyStep()
    {
        velocity.y += gravity * Time.deltaTime;
        hook.position += velocity * Time.deltaTime;

        if (hook.position.y <= 0.05f)
        {
            hook.position = new Vector3(hook.position.x, 0.05f, hook.position.z);
            state = State.Landed;
        }
    }

    private void ReelStep()
    {
        Vector3 target = HandPos();
        hook.position = Vector3.MoveTowards(hook.position, target,
                                            reelSpeed * Time.deltaTime);

        if (Vector3.Distance(hook.position, target) < collectDistance)
            Collect();
    }

    private void Collect()
    {
        foreach (var item in hook.GetComponentsInChildren<DebrisItem>())
        {
            inventory.Add(item.Type, item.Amount);
            Destroy(item.gameObject);
        }

        Destroy(hook.gameObject);
        hook = null;
        state = State.Idle;
    }

    private void UpdateRope()
    {
        rope.SetPosition(0, HandPos());
        rope.SetPosition(1, hook.position);
    }
}