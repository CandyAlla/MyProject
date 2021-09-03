using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using XLuaTest;

[CustomEditor(typeof(LuaBehaviour))]
public class LuaBehaviourInspector : Editor
{
    private ReorderableList reorderableList ;
    private void OnEnable()
    {
        SerializedProperty prop = serializedObject.FindProperty("injections");
        reorderableList = new ReorderableList(serializedObject, prop, true, true, true, true);
        reorderableList.elementHeight = 50f;
        
        //绘制单个元素
        reorderableList.drawElementCallback =
            (rect, index, isActive, isFocused) => {
                var element = prop.GetArrayElementAtIndex(index);
                rect.height -= 4;
                rect.y += 2;
                EditorGUI.PropertyField(rect, element);
            };

        //背景色
        reorderableList.drawElementBackgroundCallback = (rect, index, isActive, isFocused) => {
            GUI.backgroundColor = Color.yellow;
        };

        //头部
        reorderableList.drawHeaderCallback = (rect) =>
            EditorGUI.LabelField(rect, prop.displayName);
        
        reorderableList.drawElementCallback = DrawElement;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        reorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawElement(Rect rect,int index, bool isActive, bool isFocused)
    {
        if(index >= reorderableList.count)
            return;
        var luaBehaviour = target as LuaBehaviour;
        float x = rect.x;
        float y = rect.y + 2;
        float width = rect.width / 3;
        Rect nameRect = new Rect(rect.x,rect.y, width-3, 20);
        Rect objRect = new Rect(rect.x + width, rect.y, width-3, 20);
        
        SerializedProperty name = reorderableList.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("name");
        SerializedProperty value =reorderableList.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("value");
        
        name.stringValue = EditorGUI.TextField(nameRect, name.stringValue);
        EditorGUI.PropertyField(objRect, value, GUIContent.none);

        // GameObject obj = (GameObject) value.serializedObject.targetObject;
        // Component[] components = obj.GetComponents<Component>();
    }
    
    private string GetTypeName(object obj)
    {
        if (obj == null)
        {
            return "";
        }

        if (obj is LuaBehaviour)
        {
            return "LuaTable";
        }

        var fullName = obj.GetType().FullName;
        if (obj is UnityEditor.Animations.AnimatorController)
        {
            fullName = typeof(RuntimeAnimatorController).FullName;
        }

        return fullName.Replace("UnityEngine.", "");
    }
}
