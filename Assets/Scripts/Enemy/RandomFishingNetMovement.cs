using UnityEngine;

/// <summary>
/// 釣り網の動きを制御するクラス。
/// 初期位置を基準に、指定された範囲内でランダムに動く。
/// </summary>
public class RandomFishingNetMovement : MonoBehaviour
{
    [Header("移動設定")]
    [Tooltip("移動範囲の大きさ")]
    [SerializeField] float randomMovementRange = 0.05f; // ランダム移動範囲の大きさ

    private Vector2 initialPosition;              // 釣り網の初期位置

    void Start()
    {
        // 現在の位置を初期位置として保存
        initialPosition = transform.position;
    }

    void Update()
    {
        // 初期位置にランダムなオフセットを加えた新しい位置を設定
        transform.position = initialPosition + Random.insideUnitCircle * randomMovementRange;
    }
}
