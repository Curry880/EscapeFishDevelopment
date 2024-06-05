using UnityEngine;

/// <summary>
/// This class provides information about the sprite attached to the background object.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundSpriteInfo : MonoBehaviour
{
    public Vector2 texturePixSize { get; private set; }
    public Vector2 spritePixSize { get; private set; }
    public Vector2 spriteSize { get; private set; }
    public Vector2 viewSize { get; private set; }

    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Initializes the SpriteRenderer component.
    /// </summary>
    private void Awake()
    {
        TryInitializeSpriteRenderer();
    }

    /// <summary>
    /// Calculates and stores various sizes related to the sprite when the game starts.
    /// </summary>
    private void Start()
    {
        texturePixSize = GetTexturePixelSize();
        spritePixSize = CalculateSpritePixelSize();
        spriteSize = CalculateSpriteSizeInWorldUnits();
        viewSize = CalculateGameViewSizeInWorldUnits();
    }

    /// <summary>
    /// Tries to initialize the SpriteRenderer component. If not found, adds it.
    /// </summary>
    private void TryInitializeSpriteRenderer()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer component not found. Adding SpriteRenderer.");
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
    }

    /// <summary>
    /// Gets the pixel size of the texture.
    /// </summary>
    /// <returns>Texture pixel size.</returns>
    private Vector2 GetTexturePixelSize()
    {
        if (spriteRenderer.sprite == null)
        {
            Debug.LogError("Sprite not found.");
            return Vector2.zero;
        }

        Texture2D texture = spriteRenderer.sprite.texture;
        if (texture == null)
        {
            Debug.LogError("Texture not found.");
            return Vector2.zero;
        }

        return new Vector2(texture.width, texture.height);
    }

    /// <summary>
    /// Calculates the pixel size of the sprite.
    /// </summary>
    /// <returns>Sprite pixel size.</returns>
    private Vector2 CalculateSpritePixelSize()
    {
        if (!(Camera.main is Camera mainCamera))
        {
            Debug.LogError("Main camera not found.");
            return Vector2.zero;
        }

        // Rendererの境界ボックスの頂点を取得
        if (!(spriteRenderer.bounds is Bounds bounds))
        {
            Debug.LogError("Bounds not found.");
            return Vector2.zero;
        }
        Vector3[] vertices = new Vector3[8];

        vertices[0] = bounds.min;
        vertices[1] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
        vertices[2] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
        vertices[3] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
        vertices[4] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
        vertices[5] = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
        vertices[6] = bounds.max;
        vertices[7] = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);

        // 頂点をスクリーン座標に変換
        Vector2 min = mainCamera.WorldToScreenPoint(vertices[0]);
        Vector2 max = mainCamera.WorldToScreenPoint(vertices[0]);

        foreach (Vector3 vertex in vertices)
        {
            Vector2 screenPoint = mainCamera.WorldToScreenPoint(vertex);
            min = Vector2.Min(min, screenPoint);
            max = Vector2.Max(max, screenPoint);
        }

        // スクリーン座標でのサイズを計算
        return max - min;
    }

    /// <summary>
    /// Calculates the size of the sprite in world units.
    /// </summary>
    /// <returns>Sprite size in world units.</returns>
    private Vector2 CalculateSpriteSizeInWorldUnits()
    {
        if (!(spriteRenderer.sprite is Sprite sprite))
        {
            Debug.LogError("Sprite not found.");
            return Vector2.zero;
        }

        // テクスチャサイズとPixels Per Unitを考慮してスプライトのワールドサイズを計算
        float width = sprite.texture.width / sprite.pixelsPerUnit;
        float height = sprite.texture.height / sprite.pixelsPerUnit;

        // スプライトのスケールを考慮
        width *= spriteRenderer.transform.lossyScale.x;
        height *= spriteRenderer.transform.lossyScale.y;

        return new Vector2(width, height); ;
    }

    /// <summary>
    /// Calculates the size of the game view in world units.
    /// </summary>
    /// <returns>Game view size in world units.</returns>
    private Vector2 CalculateGameViewSizeInWorldUnits()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found.");
            return Vector2.zero;
        }

        // カメラの近くのクリップ面の四隅をワールド座標で取得
        float height = 2f * mainCamera.orthographicSize;
        float width = height * mainCamera.aspect;

        return new Vector2(width, height);
    }
}
