using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // Required for UI button interaction

public class PlayerMovement : MonoBehaviour
{
    public enum PLAYER_ACTION
    {
        None,
        Jump,
        Crouch
    }

    public enum BUTTON_ACTION
    {
        None,
        Pressed,
        Down,
        Up
    }

    public CharacterController2D controller;
    public Joystick joystick;
    public Animator animator;
    public Player player; // Reference to your player
    public float runSpeed = 20f;
    
    public bool crouch = false;

    public PLAYER_ACTION playerAction;
    public BUTTON_ACTION jumpButtonAction;
    public BUTTON_ACTION crouchButtonAction;

    public bool jump = false; // To trigger the initial jump in FixedUpdate
    public bool isDoubleJumping = false; // Flag to indicate if the player is in the double jump hold state
    public int jumpsPerformed = 0; // Track the number of jumps performed
    private int maxJumps = 2; // Max number of jumps, one additional jump from initial, so 2 jumps 0 and 1

    float horizontalMove = 0f;


    // Called when the script is enabled.
    private void OnEnable()
    {
        // Subscribe to the InputSystem.onActionChange event to listen for input action changes.
        InputSystem.onActionChange += OnActionChange;
    }

    // Called when the script is disabled.
    private void OnDisable()
    {
        // Unsubscribe from the InputSystem.onActionChange event to prevent memory leaks.
        InputSystem.onActionChange -= OnActionChange;
    }

    // Update is called once per frame
    void Update()
    {
        // Read the horizontal movement value from the "Player/Move" action.
        horizontalMove = InputSystem.actions["Player/Move"].ReadValue<Vector2>().x * runSpeed;

        // Get input from Joystick
        Vector2 joystickInput = Vector2.zero;

        // Prioritize joystick input if available
        if (joystick != null && Mathf.Abs(joystick.Horizontal) > 0.1f)
        {
            joystickInput = new Vector2(joystick.Horizontal, 0f); // Only horizontal movement

            // Normalize the joystick input vector
            Vector2 normalizedJoystickInput = joystickInput.normalized;

            // Calculate the movement based on normalized input and constant speed
            horizontalMove = normalizedJoystickInput.x * runSpeed;
            // horizontalMove = joystick.Horizontal * runSpeed;
        }

        // Set the "Speed" parameter of the animator based on the absolute value of horizontal movement.
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        // Handle UI button actions if they are active
        if (playerAction == PLAYER_ACTION.Jump && jumpButtonAction == BUTTON_ACTION.Down)
        {
            StartJump();
            Debug.Log("Jump is true (UI)");
            jumpButtonAction = BUTTON_ACTION.None; // Reset after handling
            playerAction = PLAYER_ACTION.None;
        }
        else if (playerAction == PLAYER_ACTION.Jump && jumpButtonAction == BUTTON_ACTION.Up)
        {
            EndJump();
            Debug.Log("Crouch GetButtonUp (UI)");
            jumpButtonAction = BUTTON_ACTION.None;
            playerAction = PLAYER_ACTION.None;
        }

        if (playerAction == PLAYER_ACTION.Crouch && crouchButtonAction == BUTTON_ACTION.Down)
        {
            StartCrouch();
            Debug.Log("Crouch GetButtonDown (UI)");
            crouchButtonAction = BUTTON_ACTION.None;
            playerAction = PLAYER_ACTION.None;
        }
        else if (playerAction == PLAYER_ACTION.Crouch && crouchButtonAction == BUTTON_ACTION.Up)
        {
            EndCrouch();
            Debug.Log("Crouch GetButtonUp (UI)");
            crouchButtonAction = BUTTON_ACTION.None;
            playerAction = PLAYER_ACTION.None;
        }

        // Handle double jump animation
        animator.SetBool("IsDoubleJumping", isDoubleJumping);
    }

    // Called at a fixed rate, useful for physics calculations.
    private void FixedUpdate()
    {
        // Move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump, isDoubleJumping);
        Debug.Log("Player should be jumping: " + jump);
        jump = false; // Reset the jump input flag
    }

    // Called when an input action changes (performed, canceled, etc.).
    private void OnActionChange(object obj, InputActionChange change)
    {
        if (change == InputActionChange.ActionPerformed)
        {
            InputAction action = obj as InputAction;
            if (action == InputSystem.actions["Player/Crouch"])
            {
                StartCrouch();
            }
            else if (action == InputSystem.actions["Player/Jump"])
            {
                StartJump();
            }
        }
        else if (change == InputActionChange.ActionCanceled)
        {
            InputAction action = obj as InputAction;
            if (action == InputSystem.actions["Player/Crouch"])
            {
                EndCrouch();
            }
            else if (action == InputSystem.actions["Player/Jump"])
            {
                EndJump();
            }
        }
    }

    private void StartJump()
    {
        
        if (jumpsPerformed > 0 && jumpsPerformed <= maxJumps && (player.HasAbility(IPlayer.AbilityType.DoubleJump)))
        {
            jump = true; // Set the flag to trigger jump in FixedUpdate
            isDoubleJumping = true; // Set the flag to indicate double jump hold
            jumpsPerformed++;
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsDoubleJumping", true);
            Debug.Log($"Jump Attempted. Jumps Performed: {jumpsPerformed}");
        }
        else if (jumpsPerformed == 0 && jumpsPerformed < maxJumps)
        {
            jump = true; // Set the flag to trigger jump in FixedUpdate
            jumpsPerformed++;
            isDoubleJumping = false; // Set the flag to trigger jump in FixedUpdate
            Debug.Log("Regular Jump");
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsDoubleJumping", false);
        }
        
    }

    private void EndJump()
    {

        // If double jump is happening
        if (isDoubleJumping)
        {
            if (jumpsPerformed <= maxJumps)
            {
                animator.SetBool("IsJumping", true);
                animator.SetBool("IsDoubleJumping", false);
            }
            else if (jumpsPerformed == maxJumps)
            {
                jump = false; // Set the flag to trigger jump in FixedUpdate
                isDoubleJumping = false; // Set the flag to indicate double jump hold
            }
                Debug.Log($"Jump Attempted. Jumps Performed: {jumpsPerformed}");
        }
    }

    private void StartCrouch()
    {
        crouch = true;
        Debug.Log("Crouch GetButtonDown (Input System)");
    }

    private void EndCrouch()
    {
        crouch = false;
        Debug.Log("Crouch GetButtonUp (Input System)");
    }

    public void OnLanding()
    {

        jump = false;
        isDoubleJumping = false; // Ensure double jump hold is reset on landing
        jumpButtonAction = BUTTON_ACTION.None;
        jumpsPerformed = 0; // Reset jump counter on landing
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsDoubleJumping", false);
        Debug.Log("On Landing Ran");
    }

    public void OnCrouch(bool isCrouching)
    {
        Debug.Log("On Crouch Ran");
        animator.SetBool("IsCrouching", isCrouching);
    }
}
