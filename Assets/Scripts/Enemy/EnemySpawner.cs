using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public enum SpawnMode { Constant, Randomized }                          // スポーンのモードを定義
    [SerializeField] private SpawnMode spawnMode = SpawnMode.Constant;      // 現在のスポーンモード
    [SerializeField] GameObject[] enemies;
    [SerializeField] float constantSpawnTime = 3f;                          // 一定周期のスポーン間隔
    [SerializeField] float minSpawnTime = 2f;                               // スポーン間隔の最小値
    [SerializeField] float maxSpawnTime = 5f;                               // スポーン間隔の最大値
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] float verticalOffsetRange = 1f;                        // Y軸方向の乱数範囲

    private float gameplayStartTime;                                        // Gameplay開始時刻

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    void Update()
    {
        if (GameManager.Instance.currentState == GameState.Gameplay && gameplayStartTime == 0)
        {
            // GameStateがGameplayに変更された場合、Gameplay開始時刻を記録
            gameplayStartTime = Time.time; 
        }
        else if (GameManager.Instance.currentState != GameState.Gameplay)
        {
            // GameStateがGameplay外だった場合、gameplayStartTimeはリセット
            gameplayStartTime = 0; 
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if(GameManager.Instance.currentState != GameState.Gameplay)
            {
                // ゲームプレイ中でない場合、遅延をはさむ
                yield return new WaitForSeconds(1);
                continue;
            }

            // 現在のスポーンモードに応じて待機時間を設定
            float spawnDelay = CalculateSpawnDelay();
            yield return new WaitForSeconds(spawnDelay);

            // ランダムなスポーンポイントと敵のインデックスを設定
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            int enemyIndex = GetWeightedEnemyIndex();

            // ランダムなY軸方向のオフセットを設定
            Vector3 spawnPosition = spawnPoints[spawnPointIndex].position;
            spawnPosition.y += Random.Range(-verticalOffsetRange, verticalOffsetRange);

            // 敵を生成
            if(GameManager.Instance.currentState == GameState.Gameplay)
            {
                Instantiate(enemies[enemyIndex], spawnPosition, Quaternion.identity);
                Debug.Log("spown");
            }
        }
    }

    // スポーン間隔を取得するメソッド
    float CalculateSpawnDelay()
    {
        switch (spawnMode)
        {
            case SpawnMode.Constant:
                return constantSpawnTime;                           // 一定周期のスポーン間隔を返す
            case SpawnMode.Randomized:
                return Random.Range(minSpawnTime, maxSpawnTime);    // ランダムなスポーン間隔を返す
            default:
                return constantSpawnTime;                           // デフォルトは一定周期
        }
    }

    // 経過時間に応じて後半の敵が選ばれやすくなる
    int GetWeightedEnemyIndex()
    {
        float elapsedTime = Time.time - gameplayStartTime;                          // 経過時間
        float difficultyMaxTime = 300f;                                             // 難易度が最大になるまでの時間（例: 300秒）
        float difficultyFactor = Mathf.Clamp01(elapsedTime / difficultyMaxTime);    // 経過時間に応じた重みを設定（0.0〜1.0の範囲）

        // リストの前半が最初に有利で、徐々に後半が選ばれやすくなる
        float[] enemyWeights = CalculateWeights(difficultyFactor);
        float totalEnemyWeight = CalculateTotalWeight(enemyWeights);

        // 重み付けランダム選択
        return SelectEnemyByWeight(enemyWeights, totalEnemyWeight);
    }

    // 敵の重みを計算する
    float[] CalculateWeights(float difficultyFactor)
    {
        float[] weights = new float[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            weights[i] = Mathf.Lerp(1f, 10f, 1f - (float)i / (enemies.Length - 1)) * (1f - difficultyFactor)
                         + Mathf.Lerp(1f, 10f, (float)i / (enemies.Length - 1)) * difficultyFactor;
        }
        return weights;
    }

    // 全ての重みの合計を計算する
    float CalculateTotalWeight(float[] weights)
    {
        float totalWeight = 0f;
        foreach (float weight in weights)
        {
            totalWeight += weight;
        }
        return totalWeight;
    }

    // 重みを元に敵のインデックスを選択する
    int SelectEnemyByWeight(float[] weights, float totalWeight)
    {
        float randomWeight = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        for (int i = 0; i < weights.Length; i++)
        {
            cumulativeWeight += weights[i];
            if (randomWeight <= cumulativeWeight)
            {
                return i;
            }
        }

        // 万が一の場合、リストの最後の敵を返す
        return enemies.Length - 1;
    }
}
