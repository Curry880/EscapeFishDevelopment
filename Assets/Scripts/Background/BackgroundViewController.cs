using UnityEngine;

/// <summary>
/// This class controls the visual aspects of the background scrolling.
/// It interacts with the BackgroundScroller component to calculate the new position
/// based on the scroll speed and direction, and then updates the background's position.
/// </summary>
[RequireComponent(typeof(BackgroundScroller))]
public class BackgroundViewController : MonoBehaviour
{
    private BackgroundScroller scroller;

    [Header("Scroll Speed")]
    [Tooltip("Speed at which the background scrolls.")]
    [SerializeField] private float scrollSpeed;

    [Header("Scroll Direction")]
    [Tooltip("Direction in which the background scrolls.")]
    [SerializeField] private Vector3 scrollDirection;

    /// <summary>
    /// Called before the first frame update.
    /// Initializes the scroller and sets the initial scroll speed and direction.
    /// </summary>
    void Start()
    {
        scroller = GetComponent<BackgroundScroller>();
    }

    /// <summary>
    /// Called once per frame.
    /// Updates the scroll direction and speed, and moves the background to the new position.
    /// </summary>
    void Update()
    {
        Vector3 newPosition = scroller.CalculateNextPosition(scrollDirection, scrollSpeed);
        SetNewPosition(newPosition); // Set the new position
    }

    public void SetNewPosition(Vector3 newPosition)
    {
        transform.position = newPosition; // Set the new position
    }

    /// <summary>
    /// Sets the scroll speed.
    /// </summary>
    /// <param name="newSpeed">The new scroll speed to set.</param>
    public void SetScrollSpeed(float newSpeed)
    {
        if (Mathf.Approximately(scrollSpeed, newSpeed)) { return; }
        scrollSpeed = newSpeed;
        //Debug.Log("Speed is set to: " + scrollSpeed);
    }

    /// <summary>
    /// Sets the scroll direction.
    /// </summary>
    /// <param name="newDirection">The new scroll direction to set.</param>
    public void SetScrollDirection(Vector3 newDirection)
    {
        if (Mathf.Approximately(scrollDirection.x, newDirection.x) && Mathf.Approximately(scrollDirection.y, newDirection.y) && Mathf.Approximately(scrollDirection.z, newDirection.z)) { return; }
        scrollDirection = newDirection;
        //Debug.Log("Direction is set to: " + scrollDirection);
    }

    public void SetNewParameters(float newScrollSpeed, Vector3 newScrollDirection)
    {
        SetScrollSpeed(newScrollSpeed);
        SetScrollDirection(newScrollDirection);
    }
}
