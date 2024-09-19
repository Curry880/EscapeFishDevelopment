using UnityEngine;

public class PlayerControllerView : MonoBehaviour
{
    public enum ControlMode { FlappyBird, ArrowKeys, Both }
    public ControlMode controlMode = ControlMode.Both;

    public PlayerMovement playerMovement;

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        playerMovement.MovePlayer(x, y);
        /*
        switch (controlMode)
        {
            case ControlMode.FlappyBird:
                playerMovement.FlappyBirdControl();
                break;
            case ControlMode.ArrowKeys:
                playerMovement.ArrowKeysControl();
                break;
            case ControlMode.Both:
                playerMovement.BothControls();
                break;
        }
        */
        //Debug.Log(playerMovement.rb.velocity);
    }
}

