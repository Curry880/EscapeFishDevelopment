using UnityEngine;

[RequireComponent(typeof(BackgroundSpriteInfo))]
public class BackgroundScroller : MonoBehaviour
{
    // Struct to hold off-screen positions
    [System.Serializable]
    public struct OffScreenPosition
    {
        public float left;
        public float right;
        public float top;
        public float bottom;
    }

    // Struct to hold reset positions
    [System.Serializable]
    public struct ResetPosition
    {
        public float left;
        public float right;
        public float top;
        public float bottom;
    }

    // Struct to hold sprite and view dimensions
    [System.Serializable]
    public struct SpriteDimensions
    {
        public float spriteWidth;
        public float spriteHeight;
        public float viewWidth;
        public float viewHeight;
    }

    [SerializeField, ReadOnly] private OffScreenPosition offScreenPosition;
    [SerializeField, ReadOnly] private ResetPosition resetPosition;

    [SerializeField, ReadOnly] private float scrollSpeed;
    [SerializeField, ReadOnly] private Vector3 scrollDirection;

    private BackgroundSpriteInfo spriteInfo;

    void Start()
    {
        spriteInfo = GetComponent<BackgroundSpriteInfo>();
        scrollDirection = Vector3.left;
        // Initialize scrolling parameters
        InitializeScrollingParameters();
        SetScrollSpeed(2);
        
    }
    private void Update()
    {
        // Example: Call Scroll method from Update for demonstration
        Vector3 newPosition = Scroll(scrollDirection, scrollSpeed);
        transform.position = newPosition; // Set the new position
    }

    /// <summary>
    /// Initialize scrolling parameters based on sprite and view sizes.
    /// </summary>
    private void InitializeScrollingParameters()
    {
        SpriteDimensions dimensions = new SpriteDimensions
        {
            spriteWidth = spriteInfo.spriteSize.x,
            spriteHeight = spriteInfo.spriteSize.y,
            viewWidth = spriteInfo.viewSize.x,
            viewHeight = spriteInfo.viewSize.y
        };

        offScreenPosition.left = -(dimensions.spriteWidth + dimensions.viewWidth) / 2;
        offScreenPosition.right = (dimensions.spriteWidth + dimensions.viewWidth) / 2;
        offScreenPosition.top = (dimensions.spriteHeight + dimensions.viewHeight) / 2;
        offScreenPosition.bottom = -(dimensions.spriteHeight + dimensions.viewHeight) / 2;

        resetPosition = CalculateResetPosition(dimensions, offScreenPosition, scrollDirection);
    }

    /// <summary>
    /// Scrolls the background in the specified direction at the specified speed.
    /// </summary>
    /// <param name="direction">The direction to scroll the background.</param>
    /// <param name="speed">The speed at which to scroll the background.</param>
    /// <returns>The new position of the background.</returns>
    public Vector3 Scroll(Vector3 direction, float speed)
    {
        // Calculate velocity from direction and speed
        Vector3 velocity = direction.normalized * speed * Time.deltaTime;

        // Calculate the new position using the calculated velocity
        Vector3 newPosition = transform.position + velocity;

        // Adjust the position if it goes off-screen
        newPosition = AdjustPositionIfOffScreen(newPosition, direction);
        return newPosition;
    }

    /// <summary>
    /// Adjusts the position if it goes off-screen and returns the adjusted position.
    /// </summary>
    /// <param name="position">The current position of the background.</param>
    /// <param name="direction">The direction in which the background is moving.</param>
    /// <returns>The adjusted position of the background.</returns>
    private Vector3 AdjustPositionIfOffScreen(Vector3 position, Vector3 direction)
    {
        // Check X-axis movement
        if (direction.x < 0 && position.x < offScreenPosition.left)
        {
            position.x = resetPosition.right;
        }
        else if (direction.x > 0 && position.x > offScreenPosition.right)
        {
            position.x = resetPosition.left;
        }

        // Check Y-axis movement
        if (direction.y < 0 && position.y < offScreenPosition.bottom)
        {
            position.y = resetPosition.top;
        }
        else if (direction.y > 0 && position.y > offScreenPosition.top)
        {
            position.y = resetPosition.bottom;
        }

        return position;
    }

    /// <summary>
    /// Sets the scroll speed.
    /// </summary>
    /// <param name="newSpeed">The new scroll speed to set.</param>
    public void SetScrollSpeed(float newSpeed)
    {
        if (Mathf.Approximately(scrollSpeed, newSpeed)) return;
        scrollSpeed = newSpeed;
        Debug.Log("Speed is set to: " + scrollSpeed);
    }

    /// <summary>
    /// Calculates the reset positions for the background.
    /// </summary>
    /// <param name="dimensions">The dimensions of the sprite and view.</param>
    /// <param name="offScreenPosition">The off-screen positions.</param>
    /// <returns>The calculated reset positions.</returns>
    private ResetPosition CalculateResetPosition(SpriteDimensions dimensions, OffScreenPosition offScreenPosition, Vector3 direction)
    {
        int siblingCount = GetSiblingCount(gameObject);
        float excessWidth = (siblingCount - 1) * dimensions.spriteWidth - dimensions.viewWidth;
        float excessHeight = (siblingCount - 1) * dimensions.spriteHeight - dimensions.viewHeight;

        bool isMovingHorizontally = Mathf.Abs(direction.x) > 0;
        bool isMovingVertically = Mathf.Abs(direction.y) > 0;

        if (siblingCount == -1 || (isMovingHorizontally && excessWidth < 0) || (isMovingVertically && excessHeight < 0))
        {
            return new ResetPosition
            {
                right = offScreenPosition.right,
                left = offScreenPosition.left,
                top = offScreenPosition.top,
                bottom = offScreenPosition.bottom
            };
        }

        return new ResetPosition
        {
            /*
            right = excessWidth + offScreenPosition.right,
            left = -excessWidth + offScreenPosition.left,
            top = excessHeight + offScreenPosition.top,
            bottom = -excessHeight + offScreenPosition.bottom
            */

            right = Mathf.Abs(excessWidth + (dimensions.spriteWidth + dimensions.viewWidth) / 2),
            left = -Mathf.Abs(excessWidth + (dimensions.spriteWidth + dimensions.viewWidth) / 2),
            top = Mathf.Abs(excessHeight + (dimensions.spriteHeight + dimensions.viewHeight) / 2),
            bottom = -Mathf.Abs(excessHeight + (dimensions.spriteHeight + dimensions.viewHeight) / 2)
        };
    }

    /// <summary>
    /// Gets the sibling count of the specified game object.
    /// </summary>
    /// <param name="targetObject">The target game object.</param>
    /// <returns>The number of siblings, or -1 if an error occurs.</returns>
    private int GetSiblingCount(GameObject targetObject)
    {
        if (targetObject == null)
        {
            Debug.LogError("Target object is not assigned.");
            return -1;
        }

        Transform parentTransform = targetObject.transform.parent;

        if (parentTransform == null)
        {
            Debug.LogError("Target object does not have a parent.");
            return -1;
        }

        return parentTransform.childCount;
    }
}



