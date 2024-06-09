using System;
using UnityEngine;

/// <summary>
/// Handles the scrolling of the background in a specified direction and speed.
/// </summary>
[RequireComponent(typeof(BackgroundGraphicsInfo))]
public class BackgroundScroller : MonoBehaviour
{
    /// <summary>
    /// Holds the dimensions of the sprite and view.
    /// </summary>
    [System.Serializable]
    public struct SpriteAndViewDimensions
    {
        public float spriteWidth;
        public float spriteHeight;
        public float viewWidth;
        public float viewHeight;
    }

    /// <summary>
    /// Holds the positions for when the background is considered off-screen.
    /// </summary>
    [System.Serializable]
    public struct OffScreenPosition
    {
        public float left;
        public float right;
        public float top;
        public float bottom;
    }

    /// <summary>
    /// Holds the positions to reset the background to when it goes off-screen.
    /// </summary>
    [System.Serializable]
    public struct ResetPosition
    {
        public float left;
        public float right;
        public float top;
        public float bottom;
    }

    [System.Serializable]
    public struct BoundsInfo
    {
        public float MinX;
        public float MaxX;
        public float MinY;
        public float MaxY;

        public BoundsInfo(float minX, float maxX, float minY, float maxY)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
        }
    }

        /// <summary>
        /// Enum to represent the sides of the screen.
        /// </summary>
        [Flags]
    public enum ScreenSide
    {
        /// <summary>
        /// No side of the screen.
        /// </summary>
        None = 0,

        /// <summary>
        /// The left side of the screen.
        /// </summary>
        Left = 1 << 0,   // 0001

        /// <summary>
        /// The right side of the screen.
        /// </summary>
        Right = 1 << 1,  // 0010

        /// <summary>
        /// The top side of the screen.
        /// </summary>
        Top = 1 << 2,    // 0100

        /// <summary>
        /// The bottom side of the screen.
        /// </summary>
        Bottom = 1 << 3  // 1000
    }

    [Header("Scrolling Settings")]
    [Tooltip("Positions for when the background is considered off-screen.")]
    [SerializeField, ReadOnly] private OffScreenPosition offScreenPosition;
    [Tooltip("Positions to reset the background to when it goes off-screen.")]
    [SerializeField, ReadOnly] private ResetPosition resetPosition;

    /// <summary>
    /// Unity's Start method that initializes scrolling parameters.
    /// </summary>
    private void Start()
    {
        InitializeScrollingParameters();
    }

    /// <summary>
    /// Initialize scrolling parameters based on sprite and view sizes.
    /// </summary>
    private void InitializeScrollingParameters()
    {
        BackgroundGraphicsInfo graphicsInfo = GetComponent<BackgroundGraphicsInfo>();
        SpriteAndViewDimensions dimensions = GetSpriteAndViewDimensions(graphicsInfo);
        offScreenPosition = CalculateOffScreenPosition(dimensions);
        resetPosition = CalculateResetPosition(dimensions, offScreenPosition);
    }

    /// <summary>
    /// Gets the sprite and view dimensions.
    /// </summary>
    /// <param name="backgroundSpriteInfo">The background sprite info.</param>
    /// <returns>The dimensions of the sprite and view.</returns>
    private SpriteAndViewDimensions GetSpriteAndViewDimensions(BackgroundGraphicsInfo backgroundGraphicsInfo)
    {
        return new SpriteAndViewDimensions
        {
            spriteWidth = backgroundGraphicsInfo.spriteSize.x,
            spriteHeight = backgroundGraphicsInfo.spriteSize.y,
            viewWidth = backgroundGraphicsInfo.viewSize.x,
            viewHeight = backgroundGraphicsInfo.viewSize.y
        };
    }

    /// <summary>
    /// Calculates the off-screen positions based on the sprite and view dimensions.
    /// </summary>
    /// <param name="spriteAndViewDimensions">The dimensions of the sprite and view.</param>
    /// <returns>The calculated off-screen positions.</returns>
    private OffScreenPosition CalculateOffScreenPosition(SpriteAndViewDimensions spriteAndViewDimensions)
    {
        return new OffScreenPosition
        {
            left = -(spriteAndViewDimensions.spriteWidth + spriteAndViewDimensions.viewWidth) / 2,
            right = (spriteAndViewDimensions.spriteWidth + spriteAndViewDimensions.viewWidth) / 2,
            top = (spriteAndViewDimensions.spriteHeight + spriteAndViewDimensions.viewHeight) / 2,
            bottom = -(spriteAndViewDimensions.spriteHeight + spriteAndViewDimensions.viewHeight) / 2
        };
    }

    /// <summary>
    /// Calculates the reset positions for the background.
    /// </summary>
    /// <param name="dimensions">The dimensions of the sprite and view.</param>
    /// <param name="offScreenPosition">The off-screen positions.</param>
    /// <returns>The calculated reset positions.</returns>
    private ResetPosition CalculateResetPosition(SpriteAndViewDimensions dimensions, OffScreenPosition offScreenPosition)
    {
        int siblingCount = GetSiblingCount(gameObject);
        if (siblingCount <= 1)
        {
            Debug.LogWarning("For smooth scrolling, create a parent with BackgroundManager.");
            return new ResetPosition
            {
                right = offScreenPosition.right,
                left = offScreenPosition.left,
                top = offScreenPosition.top,
                bottom = offScreenPosition.bottom
            };  
        }

        BoundsInfo siblingBounds = CalculateSiblingBounds(gameObject);
        float excessWidth = (siblingBounds.MaxX - siblingBounds.MinX) - dimensions.viewWidth;
        float excessHeight = (siblingBounds.MaxY - siblingBounds.MinY) - dimensions.viewHeight;

        ResetPosition resetPosition = new ResetPosition
        {
            right = offScreenPosition.right + excessWidth,
            left = offScreenPosition.left - excessWidth,
            top = offScreenPosition.top + excessHeight,
            bottom = offScreenPosition.bottom - excessHeight
        };

        if (excessWidth < 0)
        {
            resetPosition.right = offScreenPosition.right;
            resetPosition.left = offScreenPosition.left;
            Debug.LogWarning("For smooth scrolling, increase the number of child elements or widen the sprite's width.");
        }
        if (excessHeight < 0)
        {
            resetPosition.top = offScreenPosition.top;
            resetPosition.bottom = offScreenPosition.bottom;
            Debug.LogWarning("For smooth scrolling, increase the number of child elements or widen the sprite's height.");
        }
        
        return resetPosition;
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

    private BoundsInfo CalculateSiblingBounds(GameObject targetObject)
    {
        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minY = float.MaxValue;
        float maxY = float.MinValue;

        if (targetObject == null)
        {
            Debug.LogError("Target object is not assigned.");
            return new BoundsInfo(minX, maxX, minY, maxY);
        }

        Transform parentTransform = targetObject.transform.parent;

        if (parentTransform == null)
        {
            Debug.LogError("Target object does not have a parent.");
            return new BoundsInfo(minX, maxX, minY, maxY);
        }

        foreach (Transform child in parentTransform)
        {
            Vector3 childPosition = child.position;
            if (childPosition.x < minX) { minX = childPosition.x; }
            if (childPosition.x > maxX) { maxX = childPosition.x; }
            if (childPosition.y < minY) { minY = childPosition.y; }
            if (childPosition.y > maxY) { maxY = childPosition.y; }
        }

        return new BoundsInfo(minX, maxX, minY, maxY);
    }

    /// <summary>
    /// Calculates the next position of the background in the specified direction at the specified speed.
    /// </summary>
    /// <param name="direction">The direction to move the background.</param>
    /// <param name="speed">The speed at which to move the background.</param>
    /// <returns>The new position of the background.</returns>
    public Vector3 CalculateNextPosition(Vector3 direction, float speed)
    {
        // Calculate velocity from direction and speed
        Vector3 velocity = direction.normalized * speed * Time.deltaTime;

        // Calculate the new position using the calculated velocity
        Vector3 newPosition = transform.position + velocity;

        // Adjust the position if it goes off-screen
        //ScreenSide screenSide = DetermineOffScreenSide(newPosition, direction, offScreenPosition);
        ScreenSide screenSide = DetermineOffScreenSide(newPosition, resetPosition);
        if (screenSide != ScreenSide.None)
        {
            newPosition = AdjustPositionIfOffScreen(newPosition, screenSide);
        }
        return newPosition;
    }

    /// <summary>
    /// Determines which side of the screen the background has moved off.
    /// </summary>
    /// <param name="position">The current position of the background.</param>
    /// <param name="offScreenPosition">The off-screen positions.</param>
    /// <returns>The side(s) of the screen from which the background went off.</returns>
    private ScreenSide DetermineOffScreenSide(Vector3 position, ResetPosition resetPosition)
    {
        ScreenSide screenSide = ScreenSide.None;

        // Check X-axis movement
        if (position.x < resetPosition.left)
        {
            screenSide |= ScreenSide.Left;
        }
        else if (position.x > resetPosition.right)
        {
            screenSide |= ScreenSide.Right;
        }

        // Check Y-axis movement
        if (position.y < resetPosition.bottom)
        {
            screenSide |= ScreenSide.Bottom;
        }
        else if (position.y > resetPosition.top)
        {
            screenSide |= ScreenSide.Top;
        }

        return screenSide;
    }

    /// <summary>
    /// Adjusts the position if it goes off-screen and returns the adjusted position.
    /// </summary>
    /// <param name="position">The current position of the background.</param>
    /// <param name="screenSide">The side(s) of the screen from which the background went off.</param>
    /// <returns>The adjusted position of the background.</returns>
    private Vector3 AdjustPositionIfOffScreen(Vector3 position, ScreenSide screenSide)
    {
        if (screenSide.HasFlag(ScreenSide.Left))
        {
            position.x = offScreenPosition.right;
        }
        else if (screenSide.HasFlag(ScreenSide.Right))
        {
            position.x = offScreenPosition.left;
        }

        if (screenSide.HasFlag(ScreenSide.Top))
        {
            position.y = offScreenPosition.bottom;
        }
        else if (screenSide.HasFlag(ScreenSide.Bottom))
        {
            position.y = offScreenPosition.top;
        }

        return position;
    }
}
