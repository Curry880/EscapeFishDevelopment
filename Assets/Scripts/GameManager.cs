using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Title,
    Gameplay,
    Result
}

public class GameManager : MonoBehaviour
{
    public GameObject[] displayObjects;          // ゲームスタート時に表示させるオブジェクトたち
    public GameObject goalObject;
    public Text scoreText;
    public Slider scoreSlider;
    public int clearScore;
    public static GameManager Instance;
    public GameState currentState;

    public GameObject player;                    // プレイヤーオブジェクト
    public float moveDuration = 1.0f;            // プレイヤーを移動させる時間
    public PlayerInvincibility invincibility;    // プレイヤーの無敵スクリプト

    private Vector3 targetPosition = Vector3.zero;  // プレイヤーの目標位置

    private float score = 0;

    private void Awake()
    {
        // シングルトンパターンの設定
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
        scoreSlider.maxValue = clearScore;
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
       
        if (currentState == GameState.Gameplay)
        {
            score += Time.deltaTime;
            scoreSlider.value = score;
            scoreText.text = score.ToString("F2");
            // Debug.Log(score);
            if (score > clearScore)
            {
                currentState = GameState.Result;
                Instantiate(goalObject);
            }
        }
    }

    IEnumerator StartGame()
    {
        // プレイヤーの位置を取得
        Vector3 startPosition = player.transform.position;

        // プレイヤーの位置が(0, 0)でない場合、1秒かけて移動する
        if (startPosition != targetPosition)
        {
            float elapsedTime = 0f;
            while (elapsedTime < moveDuration)
            {
                // 線形補間を使って移動
                player.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            // 目標位置にぴったりと合わせる
            player.transform.position = targetPosition;
        }
        foreach(GameObject displayObject in displayObjects)
        {
            displayObject.SetActive(true);
        }
        
        // 無敵状態を開始
        invincibility.StartCoroutine("BecomeInvincible");
    }

    public IEnumerator EndGame()
    {
        // プレイヤーの位置を取得
        Vector3 startPosition = player.transform.position;
        Vector3 goalPosition = new Vector3(13, 0, 0);
        if (startPosition != goalPosition)
        {
            float elapsedTime = 0f;
            while (elapsedTime < moveDuration)
            {
                // 線形補間を使って移動
                player.transform.position = Vector3.Lerp(startPosition, goalPosition, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            // 目標位置にぴったりと合わせる
            player.transform.position = goalPosition;
        }
        player.SetActive(false);
    }
}
