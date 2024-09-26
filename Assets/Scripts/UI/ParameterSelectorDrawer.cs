using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;

[CustomPropertyDrawer(typeof(SliderParameterSync.ParameterSelector))]
public class ParameterSelectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // ScriptableObjectの取得
        SerializedProperty scriptableObjectProperty = property.serializedObject.FindProperty("parameterData");
        if (scriptableObjectProperty == null || scriptableObjectProperty.objectReferenceValue == null)
        {
            EditorGUI.LabelField(position, "No ScriptableObject assigned.");
            return;
        }

        // ScriptableObjectからFieldInfoのリストを取得
        var scriptableObject = scriptableObjectProperty.objectReferenceValue;
        FieldInfo[] fields = scriptableObject.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        // プルダウンメニューの選択肢を作成
        string[] options = fields.Select(f => f.Name).ToArray();

        // インデックスプロパティの取得
        SerializedProperty indexProperty = property.FindPropertyRelative("parameterIndex");

        // 現在の選択されているインデックス
        int currentIndex = indexProperty.intValue;

        // プルダウンメニューを描画して、新しい選択されたインデックスを取得
        int selectedIndex = EditorGUI.Popup(position, label.text, currentIndex, options);

        // インデックスが変わった場合にインデックスを更新
        if (selectedIndex != currentIndex)
        {
            indexProperty.intValue = selectedIndex;
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}
