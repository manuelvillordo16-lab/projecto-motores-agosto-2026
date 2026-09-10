using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private CharacterController controller;

    [Header("Velocidades")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 9f;
    [SerializeField] private float crouchSpeed = 2.5f;

    [Header("Salto y gravedad")]
    [SerializeField] private float jumpHeight = 1.6f;
    [SerializeField] private float gravity = -15f;

    [Header("Agacharse")]
    [SerializeField] private float standingHeight = 2f;
    [SerializeField] private float crouchingHeight = 1.2f;
    [SerializeField] private float heightChangeSpeed = 10f;

    // Input
    private Vector2 moveInput;
    private bool jumpPressed;
    private bool isSprinting;
    private bool isCrouching;

    // Physics
    private Vector3 velocity;
    private float currentHeight;

    private void Awake()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();

        currentHeight = standingHeight;
        controller.height = standingHeight;
    }

    private void Update()
    {
        HandleMovement();
        HandleCrouch();
    }

    //Inputs

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            jumpPressed = true;
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.ReadValueAsButton();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        isCrouching = context.ReadValueAsButton();
    }

    //Movement

    private void HandleMovement()
    {
        // Camera relative movement
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = camRight * moveInput.x + camForward * moveInput.y;
        move = Vector3.ClampMagnitude(move, 1f);

        // Speed according to state
        float targetSpeed = walkSpeed;
        if (isCrouching) targetSpeed = crouchSpeed;
        else if (isSprinting) targetSpeed = sprintSpeed;

        // Jump and Gravity
        if (controller.isGrounded && velocity.y < 0f)
            velocity.y = -2f;

        if (jumpPressed && controller.isGrounded && !isCrouching)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        jumpPressed = false; // Always resets

        velocity.y += gravity * Time.deltaTime;

        // Horizontal and Vertical Movement
        Vector3 finalMove = move * targetSpeed + Vector3.up * velocity.y;
        controller.Move(finalMove * Time.deltaTime);

        // 5. Rotación del personaje
        if (move.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }

    private void HandleCrouch()
    {
        float targetHeight = isCrouching ? crouchingHeight : standingHeight;
        if (Mathf.Abs(controller.height - targetHeight) > 0.01f)
        {
            float heightDifference = targetHeight - controller.height;
            controller.height = Mathf.Lerp(controller.height, targetHeight, heightChangeSpeed * Time.deltaTime);
            Vector3 center = controller.center;
            center.y = controller.height / 2f;
            controller.center = center;
            if (controller.isGrounded)
            {
                transform.position += new Vector3(0, heightDifference * 0.5f * Time.deltaTime * heightChangeSpeed, 0);
            }
        }
    }
}