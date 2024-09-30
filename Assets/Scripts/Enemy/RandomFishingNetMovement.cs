using UnityEngine;

/// <summary>
/// �ނ�Ԃ̓����𐧌䂷��N���X�B
/// �����ʒu����ɁA�w�肳�ꂽ�͈͓��Ń����_���ɓ����B
/// </summary>
public class RandomFishingNetMovement : MonoBehaviour
{
    [Header("�ړ��ݒ�")]
    [Tooltip("�ړ��͈͂̑傫��")]
    [SerializeField] float randomMovementRange = 0.05f; // �����_���ړ��͈͂̑傫��

    private Vector2 initialPosition;              // �ނ�Ԃ̏����ʒu

    void Start()
    {
        // ���݂̈ʒu�������ʒu�Ƃ��ĕۑ�
        initialPosition = transform.position;
    }

    void Update()
    {
        // �����ʒu�Ƀ����_���ȃI�t�Z�b�g���������V�����ʒu��ݒ�
        transform.position = initialPosition + Random.insideUnitCircle * randomMovementRange;
    }
}
