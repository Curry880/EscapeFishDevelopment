using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class SliderParameterSync : MonoBehaviour
{
    [System.Serializable]
    public class ParameterSelector
    {
        public int parameterIndex;  // �C���f�b�N�X��ێ�
    }

    [SerializeField] ParameterSelector parameterSelector;
    [SerializeField] Slider slider;  // �X���C�_�[���w��
    [SerializeField] ScriptableObject parameterData;  // ScriptableObject�Q��

    void Start()
    {
        // �X���C�_�[�̏����l�ɉ����ăp�����[�^���X�V
        UpdateParameter(slider.value);

        // �X���C�_�[�̒l���ς�邽�тɃp�����[�^���X�V
        slider.onValueChanged.AddListener(delegate { UpdateParameter(slider.value); });
    }

    // �X���C�_�[�̒l���L�����N�^�[�̃p�����[�^�ɔ��f���郁�\�b�h
    void UpdateParameter(float newValue)
    {
        // PlayerData �̃t�B�[���h���擾
        FieldInfo[] fields = parameterData.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (parameterSelector.parameterIndex >= 0 && parameterSelector.parameterIndex < fields.Length)
        {
            FieldInfo field = fields[parameterSelector.parameterIndex];

            // �t�B�[���h�̌^����v���邩�m�F���Ă���l���X�V
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
