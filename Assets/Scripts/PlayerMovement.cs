using UnityEngine;
using UnityEngine.InputSystem;

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
    public float runSpeed = 40f;
    public bool jump = false;
    public bool crouch = false;

    public PLAYER_ACTION playerAction;
    public BUTTON_ACTION jumpButtonAction;
    public BUTTON_ACTION crouchButtonAction;

    float horizontalMoveAltInput = 0f;
    float horizontalMoveJoystick = 0f;
    float horizontalMove = 0f;

    // Update is called once per frame
    void Update()
    {
        // Get input from player to Move
        horizontalMoveAltInput = Input.GetAxisRaw("Horizontal") * runSpeed;
        horizontalMoveJoystick = joystick.Horizontal * runSpeed;

        // Prioritize joystick input if available
        horizontalMove = Mathf.Abs(horizontalMoveJoystick) > 0.1f ? horizontalMoveJoystick : horizontalMoveAltInput;

        // Set Animator Speed
        // Mathf.Abs returns the absolute value of a float that is ALWAYS positive
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        // Get input from player to Jump
        if (Input.GetButtonDown("Jump") || (playerAction == PLAYER_ACTION.Jump && jumpButtonAction == BUTTON_ACTION.Down))
        {
            jump = true;
            // PlayerAction(playerAction, jumpButtonAction);
            Debug.Log("Jump is true");
        }

        if (Input.GetButtonDown("Crouch") || (playerAction == PLAYER_ACTION.Crouch && crouchButtonAction == BUTTON_ACTION.Down))
        {
            crouch = true;
            Debug.Log("Crouch GetButtonDown");
            // PlayerAction(playerAction, crouchButtonAction);
        }
        else if (Input.GetButtonUp("Crouch") || (playerAction == PLAYER_ACTION.Crouch && crouchButtonAction == BUTTON_ACTION.Up))
        {
            crouch = false;
            crouchButtonAction = BUTTON_ACTION.None;
            Debug.Log("Crouch GetButtonUp");
            // PlayerAction(playerAction, crouchButtonAction);
            
        }
    }

    private void FixedUpdate()
    {
        // Move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        if (jump)
        {
            jump = false;
            jumpButtonAction = BUTTON_ACTION.None;
            Debug.Log("Jump is false");
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    public void OnCrouch(bool isCrouching)
    {
        animator.SetBool("IsCrouching", isCrouching);
    }
}
