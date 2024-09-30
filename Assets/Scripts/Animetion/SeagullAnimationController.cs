using UnityEngine;

/// <summary>
/// カモメのアニメーションを制御するクラス。
/// Animator コンポーネントを取得し、カモメのアニメーションをランダムな位置から再生します。
/// </summary>
public class SeagullAnimationController : MonoBehaviour
{
    [Header("アニメーション設定")]
    [Tooltip("再生するアニメーションの名前を指定します。デフォルトはSeagull")]
    [SerializeField] private string animationName = "Seagull";  // 再生するアニメーションの名前

    private Animator animator;                                  // Animatorへの参照
    private float normalizedTime;                               // 再生開始位置(0~1の範囲のランダムな値)

    void Start()
    {
        // Animatorコンポーネントを取得
        animator = GetComponent<Animator>();

        // Animatorがアタッチされているか確認
        if(animator)
        {
            PlaySeagullAnimation(); // アニメーションを再生
        }
        else
        {
            Debug.LogWarning($"Animator が {gameObject.name} にアタッチされていません。アニメーション再生不可。");
        }
    }

    /// <summary>
    /// カモメ用のアニメーションを再生するメソッド。
    /// 0~1 の範囲のランダムな開始位置から "Seagull" アニメーションを再生します。
    /// </summary>
    private void PlaySeagullAnimation()
    {
        // 0~1 の範囲のランダムな値を生成し、normalizedTime に設定
        normalizedTime = Random.Range(0f, 1f);

        // Seagull アニメーションを指定の開始位置から再生
        animator.Play(animationName, 0, normalizedTime);
    }
}
