using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class SliderParameterSync : MonoBehaviour
{
    [System.Serializable]
    public class ParameterSelector
    {
        public int parameterIndex;  // インデックスを保持
    }

    [SerializeField] ParameterSelector parameterSelector;
    [SerializeField] Slider slider;  // スライダーを指定
    [SerializeField] ScriptableObject parameterData;  // ScriptableObject参照

    void Start()
    {
        // スライダーの初期値に応じてパラメータを更新
        UpdateParameter(slider.value);

        // スライダーの値が変わるたびにパラメータを更新
        slider.onValueChanged.AddListener(delegate { UpdateParameter(slider.value); });
    }

    // スライダーの値をキャラクターのパラメータに反映するメソッド
    void UpdateParameter(float newValue)
    {
        // PlayerData のフィールドを取得
        FieldInfo[] fields = parameterData.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (parameterSelector.parameterIndex >= 0 && parameterSelector.parameterIndex < fields.Length)
        {
            FieldInfo field = fields[parameterSelector.parameterIndex];

            // フィールドの型が一致するか確認してから値を更新
            if (field.FieldType == typeof(float))
            {
                field.SetValue(parameterData, newValue);
                Debug.Log($"{field.Name} updated to: {newValue}");
            }
            else
            {
                Debug.LogError($"Type mismatch: Cannot assign {newValue.GetType()} to {field.Name} of type {field.FieldType}");
            }
        }
        else
        {
            Debug.LogError($"Invalid parameter index: {parameterSelector.parameterIndex}");
        }
    }
}
