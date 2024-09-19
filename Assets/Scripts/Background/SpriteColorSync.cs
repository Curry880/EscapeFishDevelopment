using UnityEngine;

[ExecuteAlways]  // �G�f�B�^�[��ł����삳����
public class SpriteColorSync : MonoBehaviour
{
    private Material material;
    private SpriteRenderer spriteRenderer;
    // �}�e���A���̃p�X�iAssets/Resources/�ȉ��̃p�X���w��j
    private string materialPath = "BackgroundMaterial"; // Resources�t�H���_���̃}�e���A���̃p�X

    void Awake()
    {
        Initialize();
    }

    // �G�f�B�^�[��Œl���ύX���ꂽ�Ƃ��ɌĂ΂��
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
            // Resources�t�H���_����w�肳�ꂽ�}�e���A�������[�h
            if (Resources.Load<Material>(materialPath) is Material loadedMaterial)
            {
                spriteRenderer.material = new Material(loadedMaterial);
                material = spriteRenderer.sharedMaterial;
            }
            else
            {
                Debug.LogWarning("Resources�t�H���_���Ɏw��̃}�e���A��������܂���ł����B");
            }
        }
        else
        {
            Debug.LogWarning("Renderer���A�^�b�`����Ă��܂���B");
        }
    }
}

