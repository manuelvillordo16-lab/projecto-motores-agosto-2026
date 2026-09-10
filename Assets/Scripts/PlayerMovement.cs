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
        HandleJumpAndGravity();
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
        if (context.performed)
            isCrouching = !isCrouching;
    }

    //Movement

    private void HandleMovement()
    {

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = camRight * moveInput.x + camForward * moveInput.y;
        move = Vector3.ClampMagnitude(move, 1f);

        float targetSpeed = walkSpeed;
        if (isCrouching)
            targetSpeed = crouchSpeed;
        else if (isSprinting)
            targetSpeed = sprintSpeed;

        controller.Move(move * targetSpeed * Time.deltaTime);

        // Character sees where cursor points at
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }

    private void HandleJumpAndGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;   // Keeps player on the ground

        if (jumpPressed && controller.isGrounded && !isCrouching)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpPressed = false;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleCrouch()
    {
        float targetHeight = isCrouching ? crouchingHeight : standingHeight;
        currentHeight = Mathf.Lerp(currentHeight, targetHeight, heightChangeSpeed * Time.deltaTime);
        controller.height = currentHeight;

        // Center Adjustment
        Vector3 center = controller.center;
        center.y = currentHeight / 2f;
        controller.center = center;
    }
}