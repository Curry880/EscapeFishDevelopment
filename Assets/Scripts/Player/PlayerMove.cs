using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] PlayerData playerData;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        PlayerParametersEnum playerParametersInstance = (PlayerParametersEnum)SingletonManager.Instance;
        Dictionary<PlayerParameter, float> playerParameters = playerParametersInstance.GetAllParameters();

        float swimForce = playerParameters[PlayerParameter.SwimForce];
        float currentForce = playerParameters[PlayerParameter.CurrentForce];
        float moveSpeed = playerParameters[PlayerParameter.MoveSpeed];
        */

        float swimForce = playerData.swimForce;
        float currentForce = playerData.currentForce;
        float moveSpeed = playerData.moveSpeed;

        if (Input.GetKeyDown(KeyCode.Space) || (!GameManager.isPlaying && transform.position.x < -5))
        {
            float move = Input.GetAxis("Vertical");
            Vector2 origineVector = new Vector2(swimForce, move * moveSpeed);
            rb.velocity = origineVector.normalized * swimForce;
        }

        // ��ɍ��ɓ����͂�������
        rb.AddForce(new Vector2(-currentForce, 0));

        // �ړ��̐���
        ClampPosition();
    }

    void ClampPosition()
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

