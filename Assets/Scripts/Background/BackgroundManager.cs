using UnityEngine;
using System.Collections.Generic;

public class BackgroundManager : MonoBehaviour
{
    [Header("Background Parameters")]
    [Tooltip("The amount of movement power applied when scrolling.")]
    //[Range(0f, 20f)]
    public float scrollSpeed = 2f;  // �w�i�̃X�N���[�����x

    private List<BackgroundSpriteInfo> childSpriteInfos = new List<BackgroundSpriteInfo>();


    void Start()
    {
        // �q�I�u�W�F�N�g�����X�g�ɒǉ�
        foreach (Transform child in transform)
        {
            childSpriteInfos.Add(child.GetComponent<BackgroundSpriteInfo>());
        }
        for (int i = 0; i < childSpriteInfos.Count; i++)
        {
            float posX = (i + 0.5f) * childSpriteInfos[i].spriteSize.x - childSpriteInfos[i].viewSize.x / 2;
            //childSpriteInfos[i].
        }
    }
    /*
    void Update()
    {
        if (speedController != null)
        {
            speedController.SetSpeed(20f);
        }
    }
    */
}
