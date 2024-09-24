using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(PlayerParameters))]
public class PlayerParametersEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // PlayerParameters ���Q��
        PlayerParameters playerParameters = (PlayerParameters)target;

        // �����̃L�[�����X�g�ɃR�s�[
        List<string> keys = new List<string>(playerParameters.parameters.Keys);

        // �R�s�[�������X�g�Ń��[�v�����s
        foreach (var key in keys)
        {
            float value = playerParameters.parameters[key];

            // ���x���Ƃ��ăL�[����\�����A�l����̓t�B�[���h�ŕҏW�\�ɂ���
            playerParameters.parameters[key] = EditorGUILayout.FloatField(key, value);
        }

        // �l���ύX���ꂽ�ꍇ�ɁA�ύX���L�^����
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
