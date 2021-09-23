#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class MPUIRecyclableScrollRectWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(MP.UI.RecyclableScrollRect);
			Utils.BeginObjectRegister(type, L, translator, 0, 3, 6, 6);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Initialize", _m_Initialize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnValueChangedListener", _m_OnValueChangedListener);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReloadData", _m_ReloadData);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Segments", _g_get_Segments);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DataSource", _g_get_DataSource);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsGrid", _g_get_IsGrid);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PrototypeCell", _g_get_PrototypeCell);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SelfInitialize", _g_get_SelfInitialize);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Direction", _g_get_Direction);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "Segments", _s_set_Segments);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "DataSource", _s_set_DataSource);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "IsGrid", _s_set_IsGrid);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PrototypeCell", _s_set_PrototypeCell);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SelfInitialize", _s_set_SelfInitialize);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Direction", _s_set_Direction);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new MP.UI.RecyclableScrollRect();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to MP.UI.RecyclableScrollRect constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Initialize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MP.UI.RecyclableScrollRect gen_to_be_invoked = (MP.UI.RecyclableScrollRect)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    MP.UI.IRecyclableScrollRectDataSource _dataSource = (MP.UI.IRecyclableScrollRectDataSource)translator.GetObject(L, 2, typeof(MP.UI.IRecyclableScrollRectDataSource));
                    
                    gen_to_be_invoked.Initialize( _dataSource );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnValueChangedListener(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MP.UI.RecyclableScrollRect gen_to_be_invoked = (MP.UI.RecyclableScrollRect)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector2>(L, 2)) 
                {
                    UnityEngine.Vector2 _normalizedPos;translator.Get(L, 2, out _normalizedPos);
                    
                    gen_to_be_invoked.OnValueChangedListener( _normalizedPos );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.OnValueChangedListener(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to MP.UI.RecyclableScrollRect.OnValueChangedListener!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReloadData(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MP.UI.RecyclableScrollRect gen_to_be_invoked = (MP.UI.RecyclableScrollRect)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.ReloadData(  );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<MP.UI.IRecyclableScrollRectDataSource>(L, 2)) 
                {
                    MP.UI.IRecyclableScrollRectDataSource _dataSource = (MP.UI.IRecyclableScrollRectDataSource)translator.GetObject(L, 2, typeof(MP.UI.IRecyclableScrollRectDataSource));
                    
                    gen_to_be_invoked.ReloadData( _dataSource );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to MP.UI.RecyclableScrollRect.ReloadData!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Segments(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MP.UI.RecyclableScrollRect gen_to_be_invoked = (MP.UI.RecyclableScrollRect)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.Segments);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DataSource(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MP.UI.RecyclableScrollRect gen_to_be_invoked = (MP.UI.RecyclableScrollRect)translator.FastGetCSObj(L, 1);
                translator.PushAny(L, gen_to_be_invoked.DataSource);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsGrid(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MP.UI.RecyclableScrollRect gen_to_be_invoked = (MP.UI.RecyclableScrollRect)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsGrid);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PrototypeCell(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MP.UI.RecyclableScrollRect gen_to_be_invoked = (MP.UI.RecyclableScrollRect)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PrototypeCell);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SelfInitialize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MP.UI.RecyclableScrollRect gen_to_be_invoked = (MP.UI.RecyclableScrollRect)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.SelfInitialize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Direction(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MP.UI.RecyclableScrollRect gen_to_be_invoked = (MP.UI.RecyclableScrollRect)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Direction);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Segments(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MP.UI.RecyclableScrollRect gen_to_be_invoked = (MP.UI.RecyclableScrollRect)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Segments = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DataSource(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MP.UI.RecyclableScrollRect gen_to_be_invoked = (MP.UI.RecyclableScrollRect)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.DataSource = (MP.UI.IRecyclableScrollRectDataSource)translator.GetObject(L, 2, typeof(MP.UI.IRecyclableScrollRectDataSource));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_IsGrid(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MP.UI.RecyclableScrollRect gen_to_be_invoked = (MP.UI.RecyclableScrollRect)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.IsGrid = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PrototypeCell(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MP.UI.RecyclableScrollRect gen_to_be_invoked = (MP.UI.RecyclableScrollRect)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PrototypeCell = (UnityEngine.RectTransform)translator.GetObject(L, 2, typeof(UnityEngine.RectTransform));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SelfInitialize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MP.UI.RecyclableScrollRect gen_to_be_invoked = (MP.UI.RecyclableScrollRect)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SelfInitialize = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Direction(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MP.UI.RecyclableScrollRect gen_to_be_invoked = (MP.UI.RecyclableScrollRect)translator.FastGetCSObj(L, 1);
                MP.UI.RecyclableScrollRect.DirectionType gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.Direction = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
