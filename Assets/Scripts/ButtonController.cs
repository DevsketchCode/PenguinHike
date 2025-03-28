using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public PlayerMovement playerMovement;

    void Start()
    {
        // Find PlayerMovement component programmatically
        if (playerMovement == null)
        {
            playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
            if (playerMovement == null)
            {
                Debug.LogError("PlayerMovement script not found on GameObject with tag 'Player'!");
            }
            else
            {
                Debug.Log("PlayerMovement script found!");
            }
        }
    }

    public void JumpButton()
    {
        Debug.Log(this.name + ": Jump Button Pressed");
        if (playerMovement != null)
        {
            playerMovement.playerAction = PlayerMovement.PLAYER_ACTION.Jump;
            playerMovement.jumpButtonAction = PlayerMovement.BUTTON_ACTION.Down;
        }
    }

    public void CrouchButtonDown()
    {
        Debug.Log(this.name + "Crouch Button Down");
        if (playerMovement != null)
        {
            playerMovement.playerAction = PlayerMovement.PLAYER_ACTION.Crouch;
            playerMovement.crouchButtonAction = PlayerMovement.BUTTON_ACTION.Down;
        }
    }

    public void CrouchButtonReleased()
    {
        Debug.Log(this.name + "Crouch Button Released");
        if (playerMovement != null)
        {
            playerMovement.playerAction = PlayerMovement.PLAYER_ACTION.Crouch;
            playerMovement.crouchButtonAction = PlayerMovement.BUTTON_ACTION.Up;
        }
    }
}