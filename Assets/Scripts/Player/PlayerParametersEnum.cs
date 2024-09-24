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
    // パラメータを格納するクラス
    [System.Serializable]
    public class Parameter
    {
        public PlayerParameter parameterType;  // パラメータの種類
        public float value;  // パラメータの値
    }

    // パラメータのリストを作成（インスペクターで設定可能）
    public List<Parameter> parameters = new List<Parameter>();

    private void Awake()
    {
        SingletonManager.RegisterInstance(this); // シングルトン管理クラスを通じて登録
    }

    // パラメータに値を設定するメソッド
    public void SetParameter(PlayerParameter parameterType, float value)
    {
        // リストの中から一致するパラメータを見つけて値を設定
        foreach (var param in parameters)
        {
            if (param.parameterType == parameterType)
            {
                param.value = value;
                Debug.Log(parameterType.ToString() + " が " + value + " に設定されました");
                return;
            }
        }

        Debug.LogWarning("指定されたパラメータが見つかりません: " + parameterType);
    }

    // パラメータを取得するメソッド
    public float GetParameter(PlayerParameter parameterType)
    {
        foreach (var param in parameters)
        {
            if (param.parameterType == parameterType)
            {
                return param.value;
            }
        }

        Debug.LogWarning("指定されたパラメータが見つかりません: " + parameterType);
        return 0;
    }
    // 複数のパラメータを一括で取得するメソッド
    public Dictionary<PlayerParameter, float> GetAllParameters()
    {
        Dictionary<PlayerParameter, float> allParameters = new Dictionary<PlayerParameter, float>();

        // リスト内のすべてのパラメータをDictionaryに追加
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

