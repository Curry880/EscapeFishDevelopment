using UnityEngine;

/// <summary>
/// �D�̃I�u�W�F�N�g���㉺�ɒP�U��������N���X�B
/// ���g���A�U�����p�����[�^�Ƃ��Ē����\�B
/// </summary>
public class ShipVibration : MonoBehaviour
{
    [Header("�U���̐ݒ�")]
    [Tooltip("�U���̍����𒲐����܂��B")]
    [SerializeField] float amplitude = 1.0f;            // �U�� (�U���̍���)
    [Tooltip("�U���̑����𒲐����܂��B")]
    [SerializeField] float frequency = 1.0f;            // ���g�� (�U���̑���)

    private Vector2 vibrationDirection = Vector3.up;    // �U���̕���
    private Vector2 initialPosition;                    // �����ʒu��ۑ�

    void Start()
    {
        // �Q�[���J�n���̃I�u�W�F�N�g�̏����ʒu���擾
        initialPosition = transform.position;
    }

    void Update()
    {
        // �V�����ʒu���v�Z���A�I�u�W�F�N�g���ړ�������
        UpdatePosition();
    }

    /// <summary>
    /// �U���Ɋ�Â��ăI�u�W�F�N�g�̈ʒu���X�V����B
    /// </summary>
    private void UpdatePosition()
    {
        Vector2 newPosition = initialPosition + vibrationDirection.normalized * CalculateOscillation();
        transform.position = new Vector3(transform.position.x, newPosition.y, transform.position.z);
    }

    /// <summary>
    /// �P�U���̒l���v�Z����B
    /// </summary>
    /// <returns>���݂̎����Ɋ�Â����U���̒l�B</returns>
    private float CalculateOscillation()
    {
        return amplitude * Mathf.Sin(2 * Mathf.PI * frequency * Time.time);
    }
}
