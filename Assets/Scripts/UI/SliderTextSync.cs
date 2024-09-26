using UnityEngine;
using UnityEngine.UI;
using TMPro;  // TextMeshPro�̖��O��Ԃ�ǉ�

public class SliderTextSync : MonoBehaviour
{
    [SerializeField] Slider slider;  // �X���C�_�[���w��
    [SerializeField] TextMeshProUGUI valueText; // TextMeshProUGUI���w��

    void Start()
    {
        // �X���C�_�[�̏����l�ɉ����ăe�L�X�g���X�V
        UpdateText(slider.value);

        // �X���C�_�[�̒l���ς�邽�тɃe�L�X�g���X�V
        slider.onValueChanged.AddListener(delegate { UpdateText(slider.value); });
    }

    // �X���C�_�[�̒l���e�L�X�g�ƃL�����N�^�[�̃p�����[�^�ɔ��f���郁�\�b�h
    void UpdateText(float value)
    {
        // �e�L�X�g�ɃX���C�_�[�̒l�𔽉f�i��F�����_2���\���j
        valueText.text = value.ToString("0.00");
    }
}


