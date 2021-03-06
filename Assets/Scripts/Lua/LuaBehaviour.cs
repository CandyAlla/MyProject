/*
 * Tencent is pleased to support the open source community by making xLua available.
 * Copyright (C) 2016 THL A29 Limited, a Tencent company. All rights reserved.
 * Licensed under the MIT License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://opensource.org/licenses/MIT
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XLua;
using System;
using Object = UnityEngine.Object;

namespace XLuaTest
{
    [Serializable]
    public class Injection
    {
        [SerializeField]
        public string name;
        [SerializeField]
        public int type;
        [SerializeField]
        public GameObject obj;
        [SerializeField]
        public Component component;
        [SerializeField]
        public string value;
    }

 
   

    [LuaCallCSharp]
    public class LuaBehaviour : MonoBehaviour
    {
        [SerializeField]
        public TextAsset luaScript;
        
        public List<Injection> injections = new List<Injection>();
        

        private  LuaEnv luaEnv = Global.luaEnv;
        float lastGCTime = 0;
        private float GCInterval = LuaManager.Instance.GCInterval;

        private Action luaStart;
        private Action luaUpdate;
        private Action luaOnDestroy;

        protected LuaTable scriptEnv;

        protected void Awake()
        {
            scriptEnv = luaEnv.NewTable();

            // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
            LuaTable meta = luaEnv.NewTable();
            meta.Set("__index", luaEnv.Global);
            scriptEnv.SetMetaTable(meta);
            meta.Dispose();

            scriptEnv.Set("self", this);
            foreach (var injection in injections)
            {
                if (injection.type == 0)
                {
                    scriptEnv.Set(injection.name, injection.component);
                }
                else
                {
                    scriptEnv.Set(injection.name, injection.value);
                }
                
            }
            // todo 没有lua脚本 添加默认lua脚本， 进行数据存储
            if(luaScript == null)
                return;
            luaEnv.DoString(luaScript.text);
            
            
            // luaEnv.DoString("print('InMemory.ccc=', require('InMemory').ccc)");

            Action luaAwake = scriptEnv.Get<Action>("Awake");
            scriptEnv.Get("Start", out luaStart);
            scriptEnv.Get("Update", out luaUpdate);
            scriptEnv.Get("OnDestroy", out luaOnDestroy);

            if (luaAwake != null)
            {
                luaAwake();
            }
        }

        // Use this for initialization
        void Start()
        {
            if (luaStart != null)
            {
                luaStart();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (luaUpdate != null)
            {
                luaUpdate();
            }
            if (Time.time - lastGCTime > GCInterval)
            {
                luaEnv.Tick();
                lastGCTime = Time.time;
            }
        }

        void OnDestroy()
        {
            if (luaOnDestroy != null)
            {
                luaOnDestroy();
            }
            luaOnDestroy = null;
            luaUpdate = null;
            luaStart = null;
            scriptEnv.Dispose();
            injections = null;
        }
    }
}
