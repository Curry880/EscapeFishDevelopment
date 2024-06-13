using UnityEngine;

public class PlayerControllerView : MonoBehaviour
{
    public enum ControlMode { FlappyBird, ArrowKeys, Both }
    public ControlMode controlMode = ControlMode.Both;

    public PlayerMovement playerMovement;

    void Update()
    {
        playerMovement.VerticalMovementControl();
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
        Debug.Log(playerMovement.rb.velocity);
    }
}

