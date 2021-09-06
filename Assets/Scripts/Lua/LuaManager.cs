using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaManager 
{
    
    private static LuaManager instance;

    public static LuaManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LuaManager();
            }
            return instance;
        }
    }

    public float GCInterval => 1f;

   
}
