using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(BackgroundSpriteInfo))]
public class BackgroundScroller : MonoBehaviour
{
    [SerializeField, ReadOnly] private float scrollSpeed;  // Background scroll speed
    private float offScreenPositionX;  // X coordinate where the background is considered off-screen
    private float resetPositionX;  // X coordinate where the background is reset

    private BackgroundSpriteInfo spriteInfo;

    void Start()
    {
        spriteInfo = GetComponent<BackgroundSpriteInfo>();

        //InitializeScrollingParameters
        if (scrollSpeed == 0f) SetScrollSpeed(2f);
        offScreenPositionX = CalculateOffScreenPositionX();
        resetPositionX = CalculateResetPositionXa();
    }

    private void Update()
    {
        Scroll();
    }

    public void Scroll()
    {
        // Move the background to the left
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);

        // If the background has completely moved off-screen to the left, reset its position to the right
        if (transform.position.x < offScreenPositionX)
        {
            transform.position = new Vector3(resetPositionX, transform.position.y, transform.position.z);
        }
    }

    public void SetScrollSpeed(float newSpeed)
    {
        if (Mathf.Approximately(scrollSpeed, newSpeed)) return;
        scrollSpeed = newSpeed;
        Debug.Log("Speed is set to: " + scrollSpeed);
    }

    float CalculateOffScreenPositionX()
    {
        return -spriteInfo.spriteSize.x;
    }

    float CalculateResetPositionXb()
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

    private float CalculateResetPositionXa()
    {
        float spriteWidth = spriteInfo.spriteSize.x;
        float viewWidth = spriteInfo.viewSize.x;
        float requiredWidth = Mathf.Ceil(viewWidth / spriteWidth) * spriteWidth;
        return spriteInfo.spriteSize.x + (requiredWidth - viewWidth);
    }
}



