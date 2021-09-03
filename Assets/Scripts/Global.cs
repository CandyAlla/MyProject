using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class Global
{
    private static LuaEnv luaenv;
    public static LuaEnv luaEnv
    {
        get
        {
            if (null == luaenv)
                luaenv = new LuaEnv();  //all lua behaviour shared one luaenv only!
            return luaenv;
        }
    }
    
}
