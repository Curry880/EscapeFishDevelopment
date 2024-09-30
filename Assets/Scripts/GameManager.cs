using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum GameState
{
    Title,
    Gameplay,
    Result
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState currentState;

    public GameObject player;                    // �v���C���[�I�u�W�F�N�g
    public float moveDuration = 1.0f;            // �v���C���[���ړ������鎞��
    public PlayerInvincibility invincibility;    // �v���C���[�̖��G�X�N���v�g

    private Vector3 targetPosition = Vector3.zero;  // �v���C���[�̖ڕW�ʒu

    private float score = 0;

    private void Awake()
    {
        // �V���O���g���p�^�[���̐ݒ�
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = GameState.Title;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState == GameState.Title && Input.GetKeyDown(KeyCode.Return))
        {
            currentState = GameState.Gameplay;
            score = 0;
            StartCoroutine(StartGame());
        }
        score += Time.deltaTime;
        Debug.Log(score);
    }

    IEnumerator StartGame()
    {
        // �v���C���[�̈ʒu���擾
        Vector3 startPosition = player.transform.position;

        // �v���C���[�̈ʒu��(0, 0)�łȂ��ꍇ�A1�b�����Ĉړ�����
        if (startPosition != targetPosition)
        {
            float elapsedTime = 0f;
            while (elapsedTime < moveDuration)
            {
                // ���`��Ԃ��g���Ĉړ�
                player.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            // �ڕW�ʒu�ɂ҂�����ƍ��킹��
            player.transform.position = targetPosition;
        }

        // ���G��Ԃ��J�n
        invincibility.StartCoroutine("BecomeInvincible");
    }
}
