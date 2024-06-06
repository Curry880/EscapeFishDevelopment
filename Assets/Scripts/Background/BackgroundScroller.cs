using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(BackgroundSpriteInfo))]
public class BackgroundScroller : MonoBehaviour
{
    // Coordinate where the background is considered off-screen
    [System.Serializable]
    public struct OffScreenPosition
    {
        public float left;
        public float right;
        public float top;
        public float bottom;
    }

    //coordinate where the background is reset
    [System.Serializable]
    public struct ResetPosition
    {
        public float left;
        public float right;
        public float top;
        public float bottom;
    }

    [SerializeField, ReadOnly] private OffScreenPosition offScreenPosition;
    [SerializeField, ReadOnly] private ResetPosition resetPosition;

    [SerializeField, ReadOnly] private float scrollSpeed;  // Background scroll speed

    private BackgroundSpriteInfo spriteInfo;

    void Start()
    {
        spriteInfo = GetComponent<BackgroundSpriteInfo>();

        // Initialize scrolling parameters
        InitializeScrollingParameters();

        //InitializeScrollingParameters
        if (scrollSpeed == 0f) SetScrollSpeed(2f);
    }
    private void Update()
    {
        Scroll(new Vector3(0, 1, 0), 2);
    }

    private void InitializeScrollingParameters()
    {
        float spriteWidth = spriteInfo.spriteSize.x;
        float spriteHeight = spriteInfo.spriteSize.y;
        float viewWidth = spriteInfo.viewSize.x;
        float viewHeight = spriteInfo.viewSize.y;

        offScreenPosition.left = -(spriteWidth + viewWidth) / 2;
        offScreenPosition.right = (spriteWidth + viewWidth) / 2;
        offScreenPosition.top = (spriteHeight + viewHeight) / 2;
        offScreenPosition.bottom = -(spriteHeight + viewHeight) / 2;

        resetPosition = CalculateResetPosition();
    }

    public void Scroll(Vector3 direction, float speed)
    {
        // Calculate velocity from direction and speed
        Vector3 velocity = direction.normalized * speed * Time.deltaTime;

        // Move the background using the calculated velocity
        transform.Translate(velocity);

        // Check X-axis movement
        if (direction.x < 0 && transform.position.x < offScreenPosition.left)
        {
            transform.position = new Vector3(resetPosition.right, transform.position.y, transform.position.z);
        }
        else if (direction.x > 0 && transform.position.x > offScreenPosition.right)
        {
            transform.position = new Vector3(resetPosition.left, transform.position.y, transform.position.z);
        }

        // Check Y-axis movement
        if (direction.y < 0 && transform.position.y < offScreenPosition.bottom)
        {
            transform.position = new Vector3(transform.position.x, resetPosition.top, transform.position.z);
        }
        else if (direction.y > 0 && transform.position.y > offScreenPosition.top)
        {
            transform.position = new Vector3(transform.position.x, resetPosition.bottom, transform.position.z);
        }
    }

    public void SetScrollSpeed(float newSpeed)
    {
        if (Mathf.Approximately(scrollSpeed, newSpeed)) return;
        scrollSpeed = newSpeed;
        Debug.Log("Speed is set to: " + scrollSpeed);
    }

    ResetPosition CalculateResetPosition()
    {
        float spriteWidth = spriteInfo.spriteSize.x;
        float spriteHeight = spriteInfo.spriteSize.y;
        float viewWidth = spriteInfo.viewSize.x;
        float viewHeight = spriteInfo.viewSize.y;

        int siblingCount = GetSiblingCount(gameObject);
        float excessWidth = (siblingCount - 1) * spriteWidth - viewWidth;
        float excessHeight = (siblingCount - 1) * spriteHeight - viewHeight;
        if (siblingCount == -1 || excessWidth < 0 || excessHeight < 0)
        {
            resetPosition.right = (spriteWidth + viewWidth) / 2;
            resetPosition.left = -(spriteWidth + viewWidth) / 2;
            resetPosition.top = (spriteHeight + viewHeight) / 2;
            resetPosition.bottom = -(spriteHeight + viewHeight) / 2;
            return resetPosition;
        }
        resetPosition.right = Mathf.Abs(excessWidth + (spriteWidth + viewWidth) / 2);
        resetPosition.left = -Mathf.Abs(excessWidth + (spriteWidth + viewWidth) / 2);
        resetPosition.top = Mathf.Abs(excessHeight + (spriteHeight + viewHeight) / 2);
        resetPosition.bottom = -Mathf.Abs(excessHeight + (spriteHeight + viewHeight) / 2);
        return resetPosition;
    }

    private int GetSiblingCount(GameObject targetObject)
    {
        if (targetObject == null)
        {
            Debug.LogError("Target object is not assigned.");
            return -1;
        }

        // 親オブジェクトを取得
        Transform parentTransform = targetObject.transform.parent;

        if (parentTransform == null)
        {
            Debug.LogError("Target object does not have a parent.");
            return -1;
        }

        // 親の子オブジェクトの数を取得
        int siblingCount = parentTransform.childCount;

        return siblingCount;
    }
}



