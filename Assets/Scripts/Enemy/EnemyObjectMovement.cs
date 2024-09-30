using UnityEngine;

/// <summary>
/// 敵キャラクターの移動と画面外に出た際の挙動を制御するクラス
/// </summary>
public class EnemyObjectMovement : MonoBehaviour
{
    [Header("移動設定")]
    [Tooltip("敵キャラクターの移動速度")]
    [SerializeField] float moveSpeed = 2.0f;            // 敵の移動速度
    [Tooltip("画面端のオフセット量（外に出る距離）")]
    [SerializeField] float warpOffset = 5f;             // ワープ位置のオフセット
    [Header("画面外処理設定")]
    [Tooltip("画面外に出た際にループするかどうか")]
    [SerializeField] bool shouldLoop = false;           // ループするかどうか

    private float leftBoundary;                         // 画面左端のワープ閾値
    private Vector2 rightWarpPosition = Vector2.zero;　 // ワープ後の位置(画面右端)

    void Start()
    {
        // カメラを参照
        Camera mainCamera = Camera.main;

        // 画面左端のワープする閾値を設定
        leftBoundary = mainCamera.ViewportToWorldPoint(new Vector2(0, 0)).x - warpOffset;

        // ワープ先の位置を設定
        rightWarpPosition.x = mainCamera.ViewportToWorldPoint(new Vector2(1, 1)).x + warpOffset;
        rightWarpPosition.y = transform.position.y;
    }

    void Update()
    {
        // 左方向へ移動
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        // 画面左端に到達したかどうか
        if (IsOutOfLeftBoundary())
        {
            if(shouldLoop)
            {
                // ループ設定が有効な場合、画面右端にワープさせる
                transform.position = rightWarpPosition;
            }
            else
            {
                // ループしない場合、オブジェクトを破壊する
                Destroy(gameObject);
            }
        }
    }

    // <summary>
    /// 敵キャラクターが画面左端を超えたかを判定する
    /// </summary>
    /// <returns>画面左端を超えたかどうか</returns>
    private bool IsOutOfLeftBoundary()
    {
        return transform.position.x < leftBoundary;
    }
}
