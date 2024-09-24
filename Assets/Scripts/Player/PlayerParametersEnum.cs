using UnityEngine;
using System.Collections.Generic;

public enum PlayerParameter
{
    SwimForce,
    MoveSpeed,
    CurrentForce
}

public class PlayerParametersEnum : MonoBehaviour
{
    // �p�����[�^���i�[����N���X
    [System.Serializable]
    public class Parameter
    {
        public PlayerParameter parameterType;  // �p�����[�^�̎��
        public float value;  // �p�����[�^�̒l
    }

    // �p�����[�^�̃��X�g���쐬�i�C���X�y�N�^�[�Őݒ�\�j
    public List<Parameter> parameters = new List<Parameter>();

    private void Awake()
    {
        SingletonManager.RegisterInstance(this); // �V���O���g���Ǘ��N���X��ʂ��ēo�^
    }

    // �p�����[�^�ɒl��ݒ肷�郁�\�b�h
    public void SetParameter(PlayerParameter parameterType, float value)
    {
        // ���X�g�̒������v����p�����[�^�������Ēl��ݒ�
        foreach (var param in parameters)
        {
            if (param.parameterType == parameterType)
            {
                param.value = value;
                Debug.Log(parameterType.ToString() + " �� " + value + " �ɐݒ肳��܂���");
                return;
            }
        }

        Debug.LogWarning("�w�肳�ꂽ�p�����[�^��������܂���: " + parameterType);
    }

    // �p�����[�^���擾���郁�\�b�h
    public float GetParameter(PlayerParameter parameterType)
    {
        foreach (var param in parameters)
        {
            if (param.parameterType == parameterType)
            {
                return param.value;
            }
        }

        Debug.LogWarning("�w�肳�ꂽ�p�����[�^��������܂���: " + parameterType);
        return 0;
    }
    // �����̃p�����[�^���ꊇ�Ŏ擾���郁�\�b�h
    public Dictionary<PlayerParameter, float> GetAllParameters()
    {
        Dictionary<PlayerParameter, float> allParameters = new Dictionary<PlayerParameter, float>();

        // ���X�g���̂��ׂẴp�����[�^��Dictionary�ɒǉ�
        foreach (var param in parameters)
        {
            allParameters.Add(param.parameterType, param.value);
        }

        return allParameters;
    }
}
public static class SingletonManager
{
    public static MonoBehaviour Instance { get; private set; }
    public static void RegisterInstance<T>(T instance) where T : MonoBehaviour
    {
        if (Instance == null)
        {
            Instance = instance;
            Object.DontDestroyOnLoad(instance.gameObject);
        }
        else
        {
            Object.Destroy(instance.gameObject);
        }
    }
}

