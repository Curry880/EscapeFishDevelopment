using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float jumpForce = 10f;
    public float moveSpeed = 5f;
    public float leftwardForce = -2f;
    public Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void FlappyBirdControl()
    {
        // 常に左方向に動く力を加える
        rb.velocity = new Vector2(leftwardForce, rb.velocity.y);

        // スペースキーを押したら右方向にジャンプする
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(jumpForce, rb.velocity.y);
        }
    }

    public void ArrowKeysControl()
    {
        if (Input.GetButton("Horizontal"))
        {
            float move = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    public void BothControls()
    {
        FlappyBirdControl();
        ArrowKeysControl();
    }

    public void VerticalMovementControl()
    {
        if (Input.GetButton("Vertical"))
        {
            float move = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, move * moveSpeed);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }

    // Editable Parameters
    private float maximumSpeed = 30.0f;
    private float acceleration = 1.0f;

    private float speedX = 0.0f;
    private float speedZ = 0.0f;

    public void MovePlayer(float x, float z)
    {
        // 左方向への移動
        if (x < -0.10f)
        {
            if (speedX - acceleration > maximumSpeed * x) { speedX -= acceleration; }
            else { speedX = maximumSpeed * x; }
        }
        // 右方向への移動
        else if (x > 0.10f)
        {
            if (speedX + acceleration < maximumSpeed * x) { speedX += acceleration; }
            else { speedX = maximumSpeed * x; }
        }
        // 何も入力されていない場合は減速
        else { speedX *= 0.90f; }

        // 前方向への移動
        if (z > 0.10f)
        {
            if (speedZ + acceleration < maximumSpeed * z) { speedZ += acceleration; }
            else { speedZ = maximumSpeed * z; }
        }
        // 後方向への移動
        else if (z < -0.10f)
        {
            if (speedZ - acceleration > maximumSpeed * z) { speedZ -= acceleration; }
            else { speedZ = maximumSpeed * z; }
        }
        // 何も入力されていない場合は減速
        else { speedZ *= 0.90f; }

        // 移動する
        transform.Translate(speedX * Time.deltaTime, speedZ * Time.deltaTime, 0.0f);

        /*
        // 移動可能範囲を超えないようにする
        Vector3 currentPosition = transform.position;
        currentPosition.x = Mathf.Clamp(currentPosition.x, movableArea.xNegativeLimit, movableArea.xPositiveLimit);
        currentPosition.z = Mathf.Clamp(currentPosition.z, movableArea.zNegativeLimit, movableArea.zPositiveLimit);
        transform.position = currentPosition;
        */
    }
}
