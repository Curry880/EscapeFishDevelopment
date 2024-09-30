using UnityEngine;

/// <summary>
/// 敵キャラクターのトリガーイベントを処理するクラス。
/// プレイヤーが敵キャラクターの当たり判定領域に侵入した際に、プレイヤーを削除します。
/// </summary>
public class EnemyTriggerHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 侵入したオブジェクトのTagがPlayerだった場合、そのオブジェクトを削除する
            Destroy(collision.gameObject);
        }
    }
}
