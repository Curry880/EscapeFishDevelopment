using System.Collections;
using UnityEngine;

public class PlayerInvincibility : MonoBehaviour
{
    public float invincibilityDuration = 3.0f;   // ���G���Ԃ̒����i�b�j
    private Renderer playerRenderer;              // �v���C���[�̃����_���[�i�_�łɎg�p�j
    private Color blinkColor = Color.clear;       // �_�Ŏ��̐F�i�����j
    private Color normalColor = Color.white;      // �ʏ펞�̐F
    public Behaviour[] disableWhileInvincible;   // ���G���ɖ����ɂ���R���|�[�l���g�i��: PlayerMovement�X�N���v�g�j

    private bool isInvincible = false;
    private Rigidbody2D playerRigidbody;           // �v���C���[�� Rigidbody �R���|�[�l���g

    void Start()
    {
        // Rigidbody ���擾
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerRenderer = GetComponent<Renderer>();
    }

    public IEnumerator BecomeInvincible()
    {
        // ���G��Ԃɐݒ�
        isInvincible = true;

        // �v���C���[�̑��x���[���ɂ���
        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector2.zero;    // ���x�� Vector2.zero �ɐݒ�
            playerRigidbody.angularVelocity = 0;
        }

        // ���G���ɖ����ɂ���R���|�[�l���g�𖳌���
        foreach (var component in disableWhileInvincible)
        {
            component.enabled = false;
        }

        // �_�ŊJ�n
        float elapsedTime = 0;
        while (elapsedTime < invincibilityDuration)
        {
            // �_�Łi�ʏ�F�Ɠ����F�����݂ɐ؂�ւ��j
            playerRenderer.material.color = (elapsedTime % 0.2f < 0.1f) ? blinkColor : normalColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���G��ԉ���
        isInvincible = false;

        // �_�ŏI����A�ʏ�̐F�ɖ߂�
        playerRenderer.material.color = normalColor;

        // ���������Ă����R���|�[�l���g���ėL����
        foreach (var component in disableWhileInvincible)
        {
            component.enabled = true;
        }
    }

    // ���̃X�N���v�g���疳�G��Ԃ��m�F�ł���v���p�e�B
    public bool IsInvincible()
    {
        return isInvincible;
    }
}
