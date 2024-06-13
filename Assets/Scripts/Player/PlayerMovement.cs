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
        // ��ɍ������ɓ����͂�������
        rb.velocity = new Vector2(leftwardForce, rb.velocity.y);

        // �X�y�[�X�L�[����������E�����ɃW�����v����
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
}
