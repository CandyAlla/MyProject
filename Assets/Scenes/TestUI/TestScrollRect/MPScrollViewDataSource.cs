using System;
using System.Collections;
using System.Collections.Generic;
using MP.UI;
using UnityEngine;
using XLua;
using XLuaTest;

[LuaCallCSharp]
public class MPScrollViewDataSource : LuaBehaviour , IRecyclableScrollRectDataSource
{
    [CSharpCallLua]
    public delegate int ActGetCount();
    [CSharpCallLua]
    public delegate void ActSetCell(ICell cell, int index);
    [CSharpCallLua]
    public delegate void ActOnPressTheRoomInfoBtn(int index, object obj);

    private ActGetCount getCount;
    private ActSetCell setCell;
    private ActOnPressTheRoomInfoBtn pressBtn;
    
    private void Start()
    {
        getCount = Global.luaEnv.Global.Get<ActGetCount>("GetItemCount");
        setCell = Global.luaEnv.Global.Get<ActSetCell>("SetCell");
        pressBtn = Global.luaEnv.Global.Get<ActOnPressTheRoomInfoBtn>("OnPressTheRoomInfoBtn");
    }
    
    public RecyclableScrollRect _recyclableScrollRect;
    [HideInInspector] public int _dataLength;
    public int GetItemCount()
    {
        return getCount();
    }

    public void SetCell(ICell cell, int index)
    {
        setCell(cell, index);
    }

    public void OnPressTheRoomInfoBtn(int index, object obj = null)
    {
        pressBtn(index, obj);
    }
}
