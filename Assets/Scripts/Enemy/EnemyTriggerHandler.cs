using UnityEngine;

/// <summary>
/// �G�L�����N�^�[�̃g���K�[�C�x���g����������N���X�B
/// �v���C���[���G�L�����N�^�[�̓����蔻��̈�ɐN�������ۂɁA�v���C���[���폜���܂��B
/// </summary>
public class EnemyTriggerHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // �N�������I�u�W�F�N�g��Tag��Player�������ꍇ�A���̃I�u�W�F�N�g���폜����
            Destroy(collision.gameObject);
        }
    }
}
