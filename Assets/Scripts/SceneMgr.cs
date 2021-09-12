using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    private AsyncOperation asyn;

    private List<string> sceneStack = new List<string>();
    private int curSwitchIndex = 0;
    
    IEnumerator LoadScene()
    {
        asyn = SceneManager.LoadSceneAsync(sceneStack[curSwitchIndex]);
        yield return asyn;
    }

    public void SwitchScene(string sceneName , bool isNeedLoading = false)
    {
        sceneStack .Clear();
        curSwitchIndex = 0;
        if (isNeedLoading)
        {
            sceneStack.Add("Loading");
        }
        sceneStack.Add(sceneName);
        CallLoadScene();

    }

    private void CallLoadScene()
    {
        StartCoroutine("LoadScene");
    }
    private void Update()
    {
        if(null ==  asyn)
            return;
        if (asyn.isDone)
        {
            asyn = null;
            if (curSwitchIndex+1 < sceneStack.Count)
            {
                curSwitchIndex++;
                CallLoadScene();
            }
        }
    }
}
