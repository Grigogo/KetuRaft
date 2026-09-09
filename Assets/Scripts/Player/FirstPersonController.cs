using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(InputReader))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Движение")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float sprintSpeed = 7f;
    [SerializeField] private float jumpHeight = 1.1f;
    [SerializeField] private float crouchSpeed = 2f;
    [SerializeField] private float standHeight = 1.8f;
    [SerializeField] private float crouchHeight = 1.0f;
    [SerializeField] private float crouchLerpSpeed = 12f;
    [SerializeField] private float gravity = -20f;

    [Header("Камера")]
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private float lookSensitivity = 0.1f;
    [SerializeField] private float pitchLimit = 85f;

    private CharacterController cc;
    private InputReader input;
    private float verticalVelocity;
    private float pitch;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        input = GetComponent<InputReader>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleLook();
        HandleCrouch();
        HandleMove();
    }

    private void HandleLook()
    {
        // yaw — крутим всё тело: за поворотом следует и направление ходьбы
        transform.Rotate(0f, input.Look.x * lookSensitivity, 0f);

        // pitch — наклоняем только держатель камеры, тело остаётся вертикальным
        pitch = Mathf.Clamp(pitch - input.Look.y * lookSensitivity, -pitchLimit, pitchLimit);
        cameraHolder.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    private void HandleMove()
    {
        // прижим к земле: без него isGrounded мерцает на стыках коллайдеров
        if (cc.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        if (cc.isGrounded && input.JumpPressed)
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

        verticalVelocity += gravity * Time.deltaTime;

        float speed = input.Crouch ? crouchSpeed
            : input.Sprint ? sprintSpeed
            : walkSpeed;
        Vector3 flat = (transform.right * input.Move.x + transform.forward * input.Move.y) * speed;

        cc.Move((flat + Vector3.up * verticalVelocity) * Time.deltaTime);
    }
    
    private void HandleCrouch()
    {
        float targetHeight = input.Crouch ? crouchHeight : standHeight;

        // плавно тянем текущий рост к целевому
        float newHeight = Mathf.Lerp(cc.height, targetHeight, crouchLerpSpeed * Time.deltaTime);
        cc.height = newHeight;

        // центр капсулы — всегда на половине роста, чтобы ноги оставались на полу
        cc.center = new Vector3(0f, newHeight * 0.5f, 0f);

        // глаза чуть ниже макушки
        cameraHolder.localPosition = new Vector3(0f, newHeight - 0.2f, 0f);
    }
}