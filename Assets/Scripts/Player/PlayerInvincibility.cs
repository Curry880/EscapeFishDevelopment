using System.Collections;
using UnityEngine;

public class PlayerInvincibility : MonoBehaviour
{
    public float invincibilityDuration = 3.0f;   // 無敵時間の長さ（秒）
    private Renderer playerRenderer;              // プレイヤーのレンダラー（点滅に使用）
    private Color blinkColor = Color.clear;       // 点滅時の色（透明）
    private Color normalColor = Color.white;      // 通常時の色
    public Behaviour[] disableWhileInvincible;   // 無敵中に無効にするコンポーネント（例: PlayerMovementスクリプト）

    private bool isInvincible = false;
    private Rigidbody2D playerRigidbody;           // プレイヤーの Rigidbody コンポーネント

    void Start()
    {
        // Rigidbody を取得
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerRenderer = GetComponent<Renderer>();
    }

    public IEnumerator BecomeInvincible()
    {
        // 無敵状態に設定
        isInvincible = true;

        // プレイヤーの速度をゼロにする
        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector2.zero;    // 速度を Vector2.zero に設定
            playerRigidbody.angularVelocity = 0;
        }

        // 無敵時に無効にするコンポーネントを無効化
        foreach (var component in disableWhileInvincible)
        {
            component.enabled = false;
        }

        // 点滅開始
        float elapsedTime = 0;
        while (elapsedTime < invincibilityDuration)
        {
            // 点滅（通常色と透明色を交互に切り替え）
            playerRenderer.material.color = (elapsedTime % 0.2f < 0.1f) ? blinkColor : normalColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 無敵状態解除
        isInvincible = false;

        // 点滅終了後、通常の色に戻す
        playerRenderer.material.color = normalColor;

        // 無効化していたコンポーネントを再有効化
        foreach (var component in disableWhileInvincible)
        {
            component.enabled = true;
        }
    }

    // 他のスクリプトから無敵状態を確認できるプロパティ
    public bool IsInvincible()
    {
        return isInvincible;
    }
}
