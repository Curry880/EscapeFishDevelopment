using UnityEngine;

/// <summary>
/// This class provides information about the graphics elements (sprite, texture, view) attached to the background object.
/// </summary>
public class BackgroundGraphicsInfo : MonoBehaviour
{
    // Public properties to hold various sizes related to the sprite and the view
    // These backing fields are read-only in the inspector
    [Header("Texture Information")]
    [Tooltip("The pixel size of the texture.")]
    [SerializeField, ReadOnly] private Vector2 texturePixSizeBackingField;

    [Header("Sprite Information")]
    [Tooltip("The pixel size of the sprite as it appears on the screen.")]
    [SerializeField, ReadOnly] private Vector2 spritePixSizeBackingField;
    [Tooltip("The size of the sprite in world units.")]
    [SerializeField, ReadOnly] private Vector2 spriteSizeBackingField;

    [Header("View Information")]
    [Tooltip("The size of the game view in world units.")]
    [SerializeField, ReadOnly] private Vector2 viewSizeBackingField;

    // Properties to access the backing fields
    public Vector2 texturePixSize 
    { 
        get => texturePixSizeBackingField;
        private set => texturePixSizeBackingField = value; 
    }
    public Vector2 spritePixSize
    {
        get => spritePixSizeBackingField;
        private set => spritePixSizeBackingField = value;
    }
    public Vector2 spriteSize
    {
        get => spriteSizeBackingField;
        private set => spriteSizeBackingField = value;
    }
    public Vector2 viewSize
    {
        get => viewSizeBackingField;
        private set => viewSizeBackingField = value;
    }

    // Private variable to hold the SpriteRenderer component
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Ensures the SpriteRenderer component is initialized.
    /// </summary>
    private void Awake()
    {
        TryInitializeSpriteRenderer();

        // Get the size of the texture in pixels
        texturePixSize = GetTexturePixelSize(spriteRenderer);
        // Calculate the size of the sprite in pixels based on screen coordinates
        spritePixSize = CalculateSpritePixelSize(spriteRenderer);
        // Calculate the size of the sprite in world units
        spriteSize = CalculateSpriteSizeInWorldUnits(spriteRenderer);
        // Calculate the size of the game view in world units
        viewSize = CalculateGameViewSizeInWorldUnits();
    }

    /// <summary>
    /// Called before the first frame update.
    /// Calculates and stores various sizes related to the sprite.
    /// </summary>
    private void Start()
    {
        
    }

    /// <summary>
    /// Tries to initialize the SpriteRenderer component.
    /// If not found, adds it to the GameObject.
    /// </summary>
    private void TryInitializeSpriteRenderer()
    {
        // Attempt to get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        // If the component is not found, add it and log a warning
        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer component not found. Adding SpriteRenderer.");
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
    }

    /// <summary>
    /// Gets the pixel size of the texture.
    /// </summary>
    /// <param name="renderer">The SpriteRenderer component to get the texture size from.</param>
    /// <returns>Texture pixel size as a Vector2.</returns>
    private Vector2 GetTexturePixelSize(SpriteRenderer renderer)
    {
        // Check if the sprite is assigned to the SpriteRenderer
        if (renderer.sprite == null)
        {
            Debug.LogError("Sprite not found.");
            return Vector2.zero;
        }

        // Get the texture from the sprite
        Texture2D texture = renderer.sprite.texture;
        if (texture == null)
        {
            Debug.LogError("Texture not found.");
            return Vector2.zero;
        }

        // Return the texture size as a Vector2
        return new Vector2(texture.width, texture.height);
    }

    /// <summary>
    /// Calculates the pixel size of the sprite.
    /// </summary>
    /// <param name="renderer">The SpriteRenderer component to calculate the sprite size from.</param>
    /// <returns>Sprite pixel size as a Vector2.</returns>
    private Vector2 CalculateSpritePixelSize(SpriteRenderer renderer)
    {
        // Get the main camera
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found.");
            return Vector2.zero;
        }

        // Get the bounds of the SpriteRenderer
        Bounds bounds = renderer.bounds;
        Vector3[] vertices = new Vector3[8];

        // Initialize vertices with the corners of the bounds
        vertices[0] = bounds.min;
        vertices[1] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
        vertices[2] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
        vertices[3] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
        vertices[4] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
        vertices[5] = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
        vertices[6] = bounds.max;
        vertices[7] = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);

        // Convert vertices to screen coordinates and find the minimum and maximum points
        Vector2 min = mainCamera.WorldToScreenPoint(vertices[0]);
        Vector2 max = mainCamera.WorldToScreenPoint(vertices[0]);

        foreach (Vector3 vertex in vertices)
        {
            Vector2 screenPoint = mainCamera.WorldToScreenPoint(vertex);
            min = Vector2.Min(min, screenPoint);
            max = Vector2.Max(max, screenPoint);
        }

        // Calculate and return the size of the sprite in pixels
        return max - min;
    }

    /// <summary>
    /// Calculates the size of the sprite in world units.
    /// </summary>
    /// <param name="renderer">The SpriteRenderer component to calculate the sprite size from.</param>
    /// <returns>Sprite size in world units as a Vector2.</returns>
    private Vector2 CalculateSpriteSizeInWorldUnits(SpriteRenderer renderer)
    {
        // Get the sprite from the SpriteRenderer
        Sprite sprite = renderer.sprite;
        if (sprite == null)
        {
            Debug.LogError("Sprite not found.");
            return Vector2.zero;
        }

        // Calculate the width and height in world units considering Pixels Per Unit and lossy scale
        float width = sprite.texture.width / sprite.pixelsPerUnit;
        float height = sprite.texture.height / sprite.pixelsPerUnit;

        // Apply the lossy scale of the SpriteRenderer's transform
        width *= spriteRenderer.transform.lossyScale.x;
        height *= spriteRenderer.transform.lossyScale.y;
        return new Vector2(width, height); ;
    }

    /// <summary>
    /// Calculates the size of the game view in world units.
    /// </summary>
    /// <returns>Game view size in world units as a Vector2.</returns>
    private Vector2 CalculateGameViewSizeInWorldUnits()
    {
        // Get the main camera
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found.");
            return Vector2.zero;
        }

        // Calculate the view size based on the camera's orthographic size and aspect ratio
        float height = 2f * mainCamera.orthographicSize;
        float width = height * mainCamera.aspect;
        return new Vector2(width, height);
    }
}
