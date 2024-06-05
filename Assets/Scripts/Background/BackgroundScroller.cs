using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(BackgroundSpriteInfo))]
public class BackgroundScroller : MonoBehaviour
{
    private float scrollSpeed;  // �w�i�̃X�N���[�����x
    private float offScreenPositionX;  // �w�i����ʊO�Ɣ��f�����X���W
    private float resetPositionX;  // �w�i�����Z�b�g�����X���W

    private BackgroundSpriteInfo spriteInfo;

    void Start()
    {
        spriteInfo = GetComponent<BackgroundSpriteInfo>();
        if(scrollSpeed == 0f) SetScrollSpeed(2f);
        offScreenPositionX = CalculateOffScreenPositionX();
        resetPositionX = CalculateResetPositionX();
    }

    public void Scroll()
    {
        // �w�i�����Ɉړ�
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
        // �w�i�����Ɋ��S�Ɉړ����������ꍇ�A�E�[�Ƀ��Z�b�g
        if (transform.position.x < offScreenPositionX)
        {
            transform.position = new Vector3(resetPositionX, transform.position.y, transform.position.z);
        }
    }

    public void SetScrollSpeed(float newSpeed)
    {
        if (scrollSpeed == newSpeed) return;
        scrollSpeed = newSpeed;
        Debug.Log("Speed is set to: " + scrollSpeed);
    }

    float CalculateOffScreenPositionX()
    {
        float offScreenPositionX = spriteInfo.spriteSize.x * (-1f);
        return offScreenPositionX;
    }

    float CalculateResetPositionX()
    {
        float spriteWidth = spriteInfo.spriteSize.x;
        float viewWidth = spriteInfo.viewSize.x;
        float sumOfSpriteWidth = spriteWidth;
        while (sumOfSpriteWidth < viewWidth)
        {
            sumOfSpriteWidth += spriteWidth;
            //Debug.Log("1");
        }
        float widthRemainder = sumOfSpriteWidth / viewWidth;
        //Debug.Log($"widthRemainder:{widthRemainder}");
        float resetPositionX = spriteInfo.spriteSize.x + widthRemainder;
        //Debug.Log(resetPositionX);
        return resetPositionX;
    }
}



