using UnityEngine;

[ExecuteAlways]  // エディター上でも動作させる
public class SpriteColorSync : MonoBehaviour
{
    private Material material;
    private SpriteRenderer spriteRenderer;
    // マテリアルのパス（Assets/Resources/以下のパスを指定）
    private string materialPath = "BackgroundMaterial"; // Resourcesフォルダ内のマテリアルのパス

    void Awake()
    {
        Initialize();
    }

    // エディター上で値が変更されたときに呼ばれる
    void OnValidate()
    {
        Initialize();
    }

    void Update()
    {
        if (spriteRenderer && material)
        {
            spriteRenderer.sharedMaterial.SetColor("_Color", spriteRenderer.color);
        }
    }

    void Initialize()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer)
        {
            // Resourcesフォルダから指定されたマテリアルをロード
            if (Resources.Load<Material>(materialPath) is Material loadedMaterial)
            {
                spriteRenderer.material = new Material(loadedMaterial);
                material = spriteRenderer.sharedMaterial;
            }
            else
            {
                Debug.LogWarning("Resourcesフォルダ内に指定のマテリアルがありませんでした。");
            }
        }
        else
        {
            Debug.LogWarning("Rendererがアタッチされていません。");
        }
    }
}

