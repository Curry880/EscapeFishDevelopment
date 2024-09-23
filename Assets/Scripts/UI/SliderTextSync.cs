using UnityEngine;
using UnityEngine.UI;
using TMPro;  // TextMeshProの名前空間を追加

public class SliderTextSync : MonoBehaviour
{
    [SerializeField] PlayerParameter playerParameter;
    [SerializeField] Slider slider;  // スライダーを指定
    [SerializeField] TextMeshProUGUI valueText; // TextMeshProUGUIを指定

    void Start()
    {
        // スライダーの初期値に応じてテキストを更新
        UpdateTextAndParameter(slider.value);

        // スライダーの値が変わるたびにテキストを更新
        slider.onValueChanged.AddListener(delegate { UpdateTextAndParameter(slider.value); });
    }

    // スライダーの値をテキストとキャラクターのパラメータに反映するメソッド
    void UpdateTextAndParameter(float value)
    {
        // テキストにスライダーの値を反映（例：小数点2桁表示）
        valueText.text = value.ToString("0.00");

        // キャラクターのパラメータをスライダーの値に応じて変更
        PlayerParameters playerParametersInstance = (PlayerParameters)SingletonManager.Instance;
        playerParametersInstance.SetParameter(playerParameter, value);
    }
}


