using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using XLuaTest;
using XLua;
using System.Linq;

namespace  XLuaTest
{
    [CustomEditor(typeof(LuaBehaviour))]
public class LuaBehaviourInspector : Editor
{
    private enum VariableType
    {
        Object = 0,
        Integer,
        FoatType,
        BooleanType,
        String,
    }

    private ReorderableList reorderableList ;

    private LuaTable luatable;

    // private Dictionary<string, Component> valueKey = new Dictionary<string, Component>();
    //
    // private Dictionary<string, GameObject> injectObjse = new Dictionary<string, GameObject>();

    private Dictionary<string, object> savedKeyValue = new Dictionary<string, object>();

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
    private static readonly Dictionary<VariableType,string> valueToStr = new Dictionary<VariableType,string>()
    {
        { VariableType.Object,ObjectTypeName },
        { VariableType.Integer,IntegerTypeName },
        {  VariableType.FoatType,FloatTypeName },
        {  VariableType.BooleanType,BooleanTypeName },
        {  VariableType.String,StringTypeName },
    };

    private LuaBehaviour luaBehaviour;

    private int selectedIndex;

    private void Awake()
    {
        luaBehaviour = target as LuaBehaviour;
    }

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
            if (index == selectedIndex)
            {
                GUI.backgroundColor = Color.red;
            }
            else
            {
                GUI.backgroundColor = Color.green;
            }
           
        };

        //头部
        reorderableList.drawHeaderCallback = (rect) =>
            EditorGUI.LabelField(rect, prop.displayName);
        
        reorderableList.drawElementCallback = DrawElement;

        reorderableList.onAddDropdownCallback = AddItem;

        reorderableList.onRemoveCallback = OnRemoveItem;

        reorderableList.onChangedCallback = OnChang;

        reorderableList.onSelectCallback = (ReorderableList list) =>
        {
            selectedIndex = list.index;
        };
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        reorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
        // GUILayout.BeginVertical();
        if (GUILayout.Button("Get Comment", GUILayout.MaxWidth(1000)))
        {
            GUIUtility.systemCopyBuffer = GetComponent();
        }
    }

    private void InitPropertyList(SerializedProperty prop)
    {
        savedKeyValue.Clear();
        
        for(int i = 0; i < reorderableList.count; i++)
        {
            SerializedProperty name = reorderableList.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("name");
            string key = name.stringValue;
            
            savedKeyValue.Add(key,luaBehaviour.injections[i]);
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

        SerializedProperty type = reorderableList.serializedProperty.GetArrayElementAtIndex(index)
            .FindPropertyRelative("type");

        if (type.intValue == 0)
        {
            //gameobject
            SerializedProperty name = reorderableList.serializedProperty.GetArrayElementAtIndex(index)
                .FindPropertyRelative("name");

            SerializedProperty obj = reorderableList.serializedProperty.GetArrayElementAtIndex(index)
                .FindPropertyRelative("obj");
            
            SerializedProperty comp = reorderableList.serializedProperty.GetArrayElementAtIndex(index)
                .FindPropertyRelative("component");

            if (string.IsNullOrEmpty(name.stringValue))
            {
                name.stringValue = "value" + index;
            }
            EditorGUI.BeginChangeCheck();
            string oriName = name.stringValue;
            name.stringValue = EditorGUI.TextField(nameRect, name.stringValue);
            if (EditorGUI.EndChangeCheck())
            {
                if(oriName == name.stringValue)
                    return;
                string curName = name.stringValue;
                if (savedKeyValue.ContainsKey(curName))
                    name.stringValue = oriName;
                RefreshKeyName(oriName, name.stringValue);
            }
            
            EditorGUI.ObjectField(objRect, obj, GUIContent.none);

            var valueObj = obj.objectReferenceValue;
            if (valueObj)
            {
                GameObject o = (GameObject)valueObj;
                Component[] components = o.GetComponents<Component>();
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
        }
        else 
        {
            //others
            SerializedProperty name = reorderableList.serializedProperty.GetArrayElementAtIndex(index)
                .FindPropertyRelative("name");

            SerializedProperty vType = reorderableList.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("type");
            
            SerializedProperty str = reorderableList.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("value");
            
            if (string.IsNullOrEmpty(name.stringValue))
            {
                name.stringValue = "value" + index;
            }
            EditorGUI.BeginChangeCheck();
            string oriName = name.stringValue;
            name.stringValue = EditorGUI.TextField(nameRect, name.stringValue);
            if (EditorGUI.EndChangeCheck())
            {
                if(oriName == name.stringValue)
                    return;
                string curName = name.stringValue;
                if (savedKeyValue.ContainsKey(curName))
                    name.stringValue = oriName;
                RefreshKeyName(oriName, name.stringValue);
            }
            EditorGUI.LabelField(objRect,valueToStr[(VariableType)vType.intValue]);
            
            EditorGUI.BeginChangeCheck();
            str.stringValue = EditorGUI.TextField(comRect, str.stringValue);
            if (EditorGUI.EndChangeCheck())
            {
                if (vType.intValue == (int)VariableType.BooleanType)
                {
                    if (str.stringValue != "false" && str.stringValue != "true")
                    {
                        str.stringValue = "false";
                    }
                }
            }
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

    private void OnRemoveItem(ReorderableList list)
    {
        ReorderableList.defaultBehaviours.DoRemoveButton(list);
        Dictionary<string, object> tmp = new Dictionary<string, object>();
        for (int i = 0; i < list.count; i++)
        {
            string key = list.serializedProperty.GetArrayElementAtIndex(i)
                .FindPropertyRelative("name").stringValue;
            if (savedKeyValue.ContainsKey(key))
            {
                tmp.Add(key,savedKeyValue[key]);
            }
        }
        
        savedKeyValue.Clear();
        savedKeyValue = tmp;
    }

    private void OnChang(ReorderableList list)
    {
        
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
        if (savedKeyValue.ContainsKey(key))
        {
            Injection value = (Injection)savedKeyValue[key];
            if(value!=null)
                return value.component;
            else
                return null;
        }
        else
        {
            return null;
        }
    }

    private GameObject GetObjByKey(string key)
    {
        if (savedKeyValue.ContainsKey(key))
        {
            Injection value = (Injection)savedKeyValue[key];
            return value.obj;
        }
        else
        {
            return null;
        }
    }

    private void RefreshInjectObjs(string key ,object value , bool isDelete = false)
    {
        if (isDelete)
        {
            if (savedKeyValue.ContainsKey(key))
            {
                savedKeyValue.Remove(key);
            }
            else
            {
                Debug.LogError("Did not save the key :" + key);
            }
        }
        else
        {
            if (savedKeyValue.ContainsKey(key))
            {
                savedKeyValue[key] = value;
            }
        }
    }

    private void RefreshKeyName(string oldKey, string newKey)
    {
        if (savedKeyValue.ContainsKey(oldKey))
        {
            var value = savedKeyValue[oldKey];
            savedKeyValue.Remove(oldKey);
            savedKeyValue.Add(newKey,value);
        }
        else
        {
            Debug.LogError("rename fail");
        }
    }
    
    private void RefreshComponents(string key , Component curCom)
    {
        if (savedKeyValue.ContainsKey(key))
        {
            Injection info = (Injection) savedKeyValue[key];
            info.component = curCom;
        }
        else
        {
            Debug.LogError("Did not save the key :" + key);
        }
    }
    
    private void AddInjectObjs(string key,object value)
    {
        if (savedKeyValue.ContainsKey(key))
        {
            Debug.LogError("Already have : " + key);
        }
        else
        {
            savedKeyValue[key] = value;
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
                AddIntValue(1);
                break;
            case VariableType.FoatType:
                AddIntValue(2);
                break;
            case VariableType.BooleanType:
                AddBooleanValue();
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
        Injection newone = new Injection();
        string name = "value" + reorderableList.count;
        if (savedKeyValue.ContainsKey(name))
            name += "V1";
        newone.name = name;
        newone.type = 0;
        luaBehaviour.injections.Add(newone);
        AddInjectObjs(name,null);

    }

    private void AddIntValue(int type)
    {
        Injection newone = new Injection();
        string name = "value" + reorderableList.count;
        newone.name = name;
        newone.type = type;
        newone.value = "0";
        luaBehaviour.injections.Add(newone);
        AddInjectObjs(name,0);
    }
    
    private void AddStringValue()
    {
        Injection newone = new Injection();
        string name = "value" + reorderableList.count;
        newone.name = name;
        newone.type = (int)VariableType.String;
        newone.value = "";
        luaBehaviour.injections.Add(newone);
        AddInjectObjs(name,"");
    }

    private void AddBooleanValue()
    {
        Injection newone = new Injection();
        string name = "value" + reorderableList.count;
        newone.name = name;
        newone.type = (int)VariableType.BooleanType;
        newone.value = "false";
        luaBehaviour.injections.Add(newone);
        AddInjectObjs(name,"false");
    }

    private string GetComponent()
    {
        string buffer = "---@class " + luaBehaviour.luaScript.name + "\n";
        for (int i = 0; i < luaBehaviour.injections.Count; i++)
        {
            var item = luaBehaviour.injections[i];
            if (item.type == 0)
            {
                buffer += "---@field " + item.name + " " + item.component.GetType().FullName;
            }
            else
            {
                buffer += "---@field " + item.name + " " + valueToStr[(VariableType) item.type];
            }
            
            if (i < luaBehaviour.injections.Count - 1)
            {
                buffer += "\n";
            }
        }
        buffer += "\n" + "---The following are custom member variables";
        return buffer;
    }
    


}

}
