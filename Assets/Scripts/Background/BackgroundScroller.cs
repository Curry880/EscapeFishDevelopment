using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(BackgroundSpriteInfo))]
public class BackgroundScroller : MonoBehaviour
{
    private float scrollSpeed;  // 背景のスクロール速度
    private float offScreenPositionX;  // 背景が画面外と判断されるX座標
    private float resetPositionX;  // 背景がリセットされるX座標

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
        // 背景を左に移動
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
        // 背景が左に完全に移動しきった場合、右端にリセット
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



