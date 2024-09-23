using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    //[SerializeField] float spawnTime = 3f;
    [SerializeField] float minSpawnTime = 2f; // スポーン間隔の最小値
    [SerializeField] float maxSpawnTime = 5f; // スポーン間隔の最大値
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] float verticalOffsetRange = 1f; // Y軸方向の乱数範囲


    // Start is called before the first frame update
    void Start()
    {
        // 一定周期でスポーンさせる
        // InvokeRepeating("Spawn", spawnTime, spawnTime);

        // ある程度の周期でスポーンさせる
        StartCoroutine(SpawnEnemy());

    }

    void Spawn()
    {
        if (GameManager.isPlaying)
        {
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            int enemyIndex = Random.Range(0, enemies.Length);
            Instantiate(enemies[enemyIndex], spawnPoints[spawnPointIndex].position, Quaternion.identity);
        }
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            if(!GameManager.isPlaying)
            {
                yield return new WaitForSeconds(1);
                continue;
            }
            // ランダムな時間待機
            float spawnDelay = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(spawnDelay);

            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            // 経過時間に応じて後半の敵が選ばれやすくなる
            int enemyIndex = GetWeightedEnemyIndex();

            Vector3 spawnPosition = spawnPoints[spawnPointIndex].position;
            spawnPosition.y += Random.Range(-verticalOffsetRange, verticalOffsetRange);

            Instantiate(enemies[enemyIndex], spawnPosition, Quaternion.identity);
        }
    }

    int GetWeightedEnemyIndex()
    {
        float elapsedTime = Time.time; // 経過時間
        float totalTimeToMaxDifficulty = 300f; // 難易度が最大になるまでの時間（例: 300秒）

        // 経過時間に応じた重みを設定（0.0〜1.0の範囲）
        float difficultyFactor = Mathf.Clamp01(elapsedTime / totalTimeToMaxDifficulty);

        // リストの前半が最初に有利で、徐々に後半が選ばれやすくなる
        float[] weights = new float[enemies.Length];
        float totalWeight = 0f;

        for (int i = 0; i < enemies.Length; i++)
        {
            // difficultyFactorが小さいときはリストの前半が有利、大きくなると後半が有利
            weights[i] = Mathf.Lerp(1f, 10f, 1f - (float)i / (enemies.Length - 1)) * (1f - difficultyFactor)
                         + Mathf.Lerp(1f, 10f, (float)i / (enemies.Length - 1)) * difficultyFactor;
            totalWeight += weights[i];
            // Debug.Log($"i: {i}, weight: {weights[i]}");
        }

        // ランダムに選択
        float randomWeight = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        for (int i = 0; i < enemies.Length; i++)
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
