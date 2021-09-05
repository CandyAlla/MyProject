using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using XLuaTest;
using XLua;
using System.Linq;

[CustomEditor(typeof(LuaBehaviour))]
public class LuaBehaviourInspector : Editor
{
    private enum VariableType
    {
        Object,
        Integer,
        FoatType,
        BooleanType,
        String,
    }

    private ReorderableList reorderableList ;

    private LuaTable luatable;

    private Dictionary<string, Component> valueKey = new Dictionary<string, Component>();

    private Dictionary<string, GameObject> injectObjse = new Dictionary<string, GameObject>();

    private const string ObjectTypeName = "object";
    private const string IntegerTypeName = "int";
    private const string FloatTypeName = "float";
    private const string BooleanTypeName = "bool";
    private const string StringTypeName = "string";
    private static readonly Dictionary<string, VariableType> createValueType = new Dictionary<string, VariableType>()
        {
            { ObjectTypeName, VariableType.Object },
            { IntegerTypeName, VariableType.Integer },
            { FloatTypeName, VariableType.FoatType },
            { BooleanTypeName, VariableType.BooleanType },
            { StringTypeName, VariableType.String },
        };

    private void OnEnable()
    {
        //luatable = serializedObject.FindProperty("luaScript") as LuaTable;
        SerializedProperty prop = serializedObject.FindProperty("injections");
        reorderableList = new ReorderableList(serializedObject, prop, true, true, true, true);
        reorderableList.elementHeight = 50f;

        InitPropertyList(prop);

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
            GUI.backgroundColor = Color.green;
        };

        //头部
        reorderableList.drawHeaderCallback = (rect) =>
            EditorGUI.LabelField(rect, prop.displayName);
        
        reorderableList.drawElementCallback = DrawElement;

        reorderableList.onAddDropdownCallback = AddItem;

      
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        reorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

    private void InitPropertyList(SerializedProperty prop)
    {
        int count = reorderableList.count;
        valueKey.Clear();
        injectObjse.Clear();
        
        for(int i = 0; i < reorderableList.count; i++)
        {
            SerializedProperty name = reorderableList.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("name");
            SerializedProperty value = reorderableList.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("value");
            SerializedProperty component = reorderableList.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("component");
           
            string key = name.stringValue;
            GameObject v = (GameObject)value.objectReferenceValue;
            Component com = (Component)component.objectReferenceValue;

            valueKey.Add(key, com);
            injectObjse.Add(key, v);

        }
    }

    private void DrawElement(Rect rect,int index, bool isActive, bool isFocused)
    {

        if (index >= reorderableList.count)
            return;
        var luaBehaviour = target as LuaBehaviour;
        float x = rect.x;
        float y = rect.y + 2;
        float width = rect.width / 3;
        Rect nameRect = new Rect(rect.x,rect.y, width-3, 20);
        Rect objRect = new Rect(rect.x + width, rect.y, width-3, 20);
        Rect comRect = new Rect(rect.x + width * 2, rect.y, width - 3, 20);

        SerializedProperty type = reorderableList.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("type");
        SerializedProperty value = reorderableList.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("variable");

        if (type.intValue == 0)
        {
            //gameobject

            SerializedProperty name = reorderableList.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("name");

            if (string.IsNullOrEmpty(name.stringValue))
            {
                name.stringValue = "value" + index;
            }
            name.stringValue = EditorGUI.TextField(nameRect, name.stringValue);
            EditorGUI.PropertyField(objRect, value, GUIContent.none);

            var valueObj = value.objectReferenceValue;
            if (valueObj)
            {


                GameObject obj = (GameObject)valueObj;
                Component[] components = obj.GetComponents<Component>();
                string[] types = new string[components.Length];

                int selectedIndex = GetComponentIndex(components, GetCompByKey(name.stringValue));

                for (int i = 0; i < components.Length; i++)
                {
                    types[i] = GetTypeName(components[i]);
                }
                EditorGUI.BeginChangeCheck();
                int newSelectedIndex = EditorGUI.IntPopup(comRect, selectedIndex, types, null);
                //Debug.Log(selectedIndex);
                if (EditorGUI.EndChangeCheck())
                {
                    RefreshComponents(name.stringValue, components[newSelectedIndex]);

                    EditorGUI.IntPopup(comRect, newSelectedIndex, types, null);
                    Debug.Log(newSelectedIndex);

                }
            }
            else if (type.intValue == 1)
            {
                //others
            }

            //SerializedProperty name = reorderableList.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("name");

            //if(string.IsNullOrEmpty(name.stringValue))
            //{
            //    name.stringValue = "value" + index;
            //}
            //name.stringValue = EditorGUI.TextField(nameRect, name.stringValue);
            //EditorGUI.PropertyField(objRect, value, GUIContent.none);

            //var valueObj = value.objectReferenceValue;
            //if (valueObj)
            //{


            //    GameObject obj = (GameObject)valueObj;
            //    Component[] components = obj.GetComponents<Component>();
            //    string[] types = new string[components.Length];

            //    int selectedIndex = GetComponentIndex(components, GetCompByKey(name.stringValue));

            //    for (int i = 0; i < components.Length; i++)
            //    {
            //        types[i] = GetTypeName(components[i]);
            //    }
            //    EditorGUI.BeginChangeCheck();
            //    int newSelectedIndex = EditorGUI.IntPopup(comRect, selectedIndex, types,null);
            //    //Debug.Log(selectedIndex);
            //    if (EditorGUI.EndChangeCheck())
            //    {
            //        RefreshComponents(name.stringValue, components[newSelectedIndex]);

            //        EditorGUI.IntPopup(comRect, newSelectedIndex, types, null);
            //        Debug.Log(newSelectedIndex);

            //    }

            //}
        }
    }

    private void AddItem(Rect rect, ReorderableList list)
    {
        GenericMenu menu = new GenericMenu();
        foreach (var item in createValueType)
        {
            menu.AddItem(new GUIContent(item.Key), false, context =>
            {
                AddValue(item.Value, item.Key);
            }, null);
        }
        menu.ShowAsContext();
    }

    private void OnChang(ReorderableList list)
    {
        var luaBehaviour = target as LuaBehaviour;
        //luaBehaviour.UpdateSchema();
        //reorderableList.list = luaBehaviour.schema;
    }

    //根据component得到对应的type的名字
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

    private Component GetCompByKey(string key)
    {
        if (valueKey.ContainsKey(key))
        {
            return valueKey[key];
        }
        else
        {
            return null;
        }
    }

    private GameObject GetObjByKey(string key)
    {
        if (injectObjse.ContainsKey(key))
        {
            return injectObjse[key];
        }
        else
        {
            return null;
        }
    }

    private void RefreshInjectObjs(string key , GameObject value = null , bool isDelete = false)
    {
        if (isDelete)
        {
            if (injectObjse.ContainsKey(key))
            {
                injectObjse.Remove(key);
            }
            else
            {
                Debug.LogError("Did not save the key :" + key);
            }
        }
        else
        {
            if (injectObjse.ContainsKey(key))
            {
                Debug.LogError("the same key :" + key + ", update the value");
                injectObjse[key] = value;
            }
            else
            {
                injectObjse.Add(key, value);
            }
        }
    }

    private void RefreshComponents(string key, Component value = null, bool isDelete = false)
    {
        if (isDelete)
        {
            if (valueKey.ContainsKey(key))
            {
                valueKey.Remove(key);
            }
            else
            {
                Debug.LogError("Did not save the key :" + key);
            }
        }
        else
        {
            if (valueKey.ContainsKey(key))
            {
                Debug.LogError("the same key :" + key + ", update the value");
                valueKey[key] = value;
            }
            else
            {
                valueKey.Add(key, value);
            }
        }
    }

    private int GetComponentIndex(Component[] comList , Component curCom)
    {
        int index = 0;

        if (curCom)
        {
            for(int i = 0; i < comList.Length; i++)
            {
                if (comList[i] == curCom)
                    return i;
            }
        }

        return index;
    }

    private void AddValue(VariableType variableType, string typeName)
    {
        switch (variableType)
        {
            case VariableType.Object:
                AddObjectValue();
                break;
            case VariableType.Integer:
                AddIntValue();
                break;
            case VariableType.String:
                AddStringValue();
                break;
            default:
                break;
        }
    }

    private void AddObjectValue()
    {
        var luaBehaviour = target as LuaBehaviour;
        Injection newone = new Injection();
        string name = "value" + reorderableList.count;
        newone.name = name;
        newone.type = 0;
        newone.variable = new VariableGameObject();
        luaBehaviour.injections.Add(newone);
        RefreshComponents(name);
        RefreshInjectObjs(name);

    }

    private void AddIntValue()
    {
        var luaBehaviour = target as LuaBehaviour;
        luaBehaviour.injections.Add(new Injection());
    }

    private void AddNumberOrBooleanValue(string typeName)
    {
        var luaBehaviour = target as LuaBehaviour;
        
    }

    private void AddStringValue()
    {
        var luaBehaviour = target as LuaBehaviour;
        
    }


    


}
