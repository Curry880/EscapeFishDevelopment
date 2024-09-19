using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float leftwardForce = 2f;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float move = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(jumpForce, move * moveSpeed);
        }

        // ��ɍ��ɓ����͂�������
        rb.AddForce(new Vector2(-leftwardForce, 0));

        // �ړ��̐���
        Clamp();
    }

    void Clamp()
    {
        // ��ʍ����̃��[���h���W���r���[�|�C���g����擾
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        // ��ʉE��̃��[���h���W���r���[�|�C���g����擾
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        Vector2 pos = transform.position;

        // �v���C���[�̈ʒu����ʓ��Ɏ��܂�悤�ɐ�����������
        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        transform.position = pos;
    }
}
