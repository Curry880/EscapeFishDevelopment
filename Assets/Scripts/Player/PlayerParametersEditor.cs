using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(PlayerParameters))]
public class PlayerParametersEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // PlayerParameters を参照
        PlayerParameters playerParameters = (PlayerParameters)target;

        // 辞書のキーをリストにコピー
        List<string> keys = new List<string>(playerParameters.parameters.Keys);

        // コピーしたリストでループを実行
        foreach (var key in keys)
        {
            float value = playerParameters.parameters[key];

            // ラベルとしてキー名を表示し、値を入力フィールドで編集可能にする
            playerParameters.parameters[key] = EditorGUILayout.FloatField(key, value);
        }

        // 値が変更された場合に、変更を記録する
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
