using UnityEngine;

/// <summary>
/// �J�����̃A�j���[�V�����𐧌䂷��N���X�B
/// Animator �R���|�[�l���g���擾���A�J�����̃A�j���[�V�����������_���Ȉʒu����Đ����܂��B
/// </summary>
public class SeagullAnimationController : MonoBehaviour
{
    [Header("�A�j���[�V�����ݒ�")]
    [Tooltip("�Đ�����A�j���[�V�����̖��O���w�肵�܂��B�f�t�H���g��Seagull")]
    [SerializeField] private string animationName = "Seagull";  // �Đ�����A�j���[�V�����̖��O

    private Animator animator;                                  // Animator�ւ̎Q��
    private float normalizedTime;                               // �Đ��J�n�ʒu(0~1�͈̔͂̃����_���Ȓl)

    void Start()
    {
        // Animator�R���|�[�l���g���擾
        animator = GetComponent<Animator>();

        // Animator���A�^�b�`����Ă��邩�m�F
        if(animator)
        {
            PlaySeagullAnimation(); // �A�j���[�V�������Đ�
        }
        else
        {
            Debug.LogWarning($"Animator �� {gameObject.name} �ɃA�^�b�`����Ă��܂���B�A�j���[�V�����Đ��s�B");
        }
    }

    /// <summary>
    /// �J�����p�̃A�j���[�V�������Đ����郁�\�b�h�B
    /// 0~1 �͈̔͂̃����_���ȊJ�n�ʒu���� "Seagull" �A�j���[�V�������Đ����܂��B
    /// </summary>
    private void PlaySeagullAnimation()
    {
        // 0~1 �͈̔͂̃����_���Ȓl�𐶐����AnormalizedTime �ɐݒ�
        normalizedTime = Random.Range(0f, 1f);

        // Seagull �A�j���[�V�������w��̊J�n�ʒu����Đ�
        animator.Play(animationName, 0, normalizedTime);
    }
}
