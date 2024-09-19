using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] Vector2 offsetSpeed;
    private Material material;

    void Start()
    {
        if(GetComponent<Renderer>() is Renderer Renderer)
        {
            material = Renderer.sharedMaterial;
        }
        else
        {
            Debug.LogWarning("Renderer���A�^�b�`����Ă��܂���B");
        }
    }

    void Update()
    {
        if (material)
        {
            // x��y�̒l��0~1�ɂȂ�悤�ɂ���
            float x = Mathf.Repeat(Time.time * offsetSpeed.x, 1);
            float y = Mathf.Repeat(Time.time * offsetSpeed.y, 1);
            Vector2 offset = new Vector2(x, y);
            material.SetTextureOffset("_MainTex", offset);
        }
        else
        {
            Debug.LogWarning("Material���A�^�b�`����Ă��܂���B");
        }
    }

    private void OnDestroy()
    {
        if (material)
        {
            material.SetTextureOffset("_MainTex", Vector2.zero);
        }
    }
}
