using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;

[CustomPropertyDrawer(typeof(SliderParameterSync.ParameterSelector))]
public class ParameterSelectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // ScriptableObject�̎擾
        SerializedProperty scriptableObjectProperty = property.serializedObject.FindProperty("parameterData");
        if (scriptableObjectProperty == null || scriptableObjectProperty.objectReferenceValue == null)
        {
            EditorGUI.LabelField(position, "No ScriptableObject assigned.");
            return;
        }

        // ScriptableObject����FieldInfo�̃��X�g���擾
        var scriptableObject = scriptableObjectProperty.objectReferenceValue;
        FieldInfo[] fields = scriptableObject.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        // �v���_�E�����j���[�̑I�������쐬
        string[] options = fields.Select(f => f.Name).ToArray();

        // �C���f�b�N�X�v���p�e�B�̎擾
        SerializedProperty indexProperty = property.FindPropertyRelative("parameterIndex");

        // ���݂̑I������Ă���C���f�b�N�X
        int currentIndex = indexProperty.intValue;

        // �v���_�E�����j���[��`�悵�āA�V�����I�����ꂽ�C���f�b�N�X���擾
        int selectedIndex = EditorGUI.Popup(position, label.text, currentIndex, options);

        // �C���f�b�N�X���ς�����ꍇ�ɃC���f�b�N�X���X�V
        if (selectedIndex != currentIndex)
        {
            indexProperty.intValue = selectedIndex;
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}
