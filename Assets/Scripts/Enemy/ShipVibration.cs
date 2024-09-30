using UnityEngine;

/// <summary>
/// 船のオブジェクトを上下に単振動させるクラス。
/// 周波数、振幅をパラメータとして調整可能。
/// </summary>
public class ShipVibration : MonoBehaviour
{
    [Header("振動の設定")]
    [Tooltip("振動の高さを調整します。")]
    [SerializeField] float amplitude = 1.0f;            // 振幅 (振動の高さ)
    [Tooltip("振動の速さを調整します。")]
    [SerializeField] float frequency = 1.0f;            // 周波数 (振動の速さ)

    private Vector2 vibrationDirection = Vector3.up;    // 振動の方向
    private Vector2 initialPosition;                    // 初期位置を保存

    void Start()
    {
        // ゲーム開始時のオブジェクトの初期位置を取得
        initialPosition = transform.position;
    }

    void Update()
    {
        // 新しい位置を計算し、オブジェクトを移動させる
        UpdatePosition();
    }

    /// <summary>
    /// 振動に基づいてオブジェクトの位置を更新する。
    /// </summary>
    private void UpdatePosition()
    {
        Vector2 newPosition = initialPosition + vibrationDirection.normalized * CalculateOscillation();
        transform.position = new Vector3(transform.position.x, newPosition.y, transform.position.z);
    }

    /// <summary>
    /// 単振動の値を計算する。
    /// </summary>
    /// <returns>現在の時刻に基づいた振動の値。</returns>
    private float CalculateOscillation()
    {
        return amplitude * Mathf.Sin(2 * Mathf.PI * frequency * Time.time);
    }
}
