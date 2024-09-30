using UnityEngine;

/// <summary>
/// �G�L�����N�^�[�̈ړ��Ɖ�ʊO�ɏo���ۂ̋����𐧌䂷��N���X
/// </summary>
public class EnemyObjectMovement : MonoBehaviour
{
    [Header("�ړ��ݒ�")]
    [Tooltip("�G�L�����N�^�[�̈ړ����x")]
    [SerializeField] float moveSpeed = 2.0f;            // �G�̈ړ����x
    [Tooltip("��ʒ[�̃I�t�Z�b�g�ʁi�O�ɏo�鋗���j")]
    [SerializeField] float warpOffset = 5f;             // ���[�v�ʒu�̃I�t�Z�b�g
    [Header("��ʊO�����ݒ�")]
    [Tooltip("��ʊO�ɏo���ۂɃ��[�v���邩�ǂ���")]
    [SerializeField] bool shouldLoop = false;           // ���[�v���邩�ǂ���

    private float leftBoundary;                         // ��ʍ��[�̃��[�v臒l
    private Vector2 rightWarpPosition = Vector2.zero;�@ // ���[�v��̈ʒu(��ʉE�[)

    void Start()
    {
        // �J�������Q��
        Camera mainCamera = Camera.main;

        // ��ʍ��[�̃��[�v����臒l��ݒ�
        leftBoundary = mainCamera.ViewportToWorldPoint(new Vector2(0, 0)).x - warpOffset;

        // ���[�v��̈ʒu��ݒ�
        rightWarpPosition.x = mainCamera.ViewportToWorldPoint(new Vector2(1, 1)).x + warpOffset;
        rightWarpPosition.y = transform.position.y;
    }

    void Update()
    {
        // �������ֈړ�
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        // ��ʍ��[�ɓ��B�������ǂ���
        if (IsOutOfLeftBoundary())
        {
            if(shouldLoop)
            {
                // ���[�v�ݒ肪�L���ȏꍇ�A��ʉE�[�Ƀ��[�v������
                transform.position = rightWarpPosition;
            }
            else
            {
                // ���[�v���Ȃ��ꍇ�A�I�u�W�F�N�g��j�󂷂�
                Destroy(gameObject);
            }
        }
    }

    // <summary>
    /// �G�L�����N�^�[����ʍ��[�𒴂������𔻒肷��
    /// </summary>
    /// <returns>��ʍ��[�𒴂������ǂ���</returns>
    private bool IsOutOfLeftBoundary()
    {
        return transform.position.x < leftBoundary;
    }
}
