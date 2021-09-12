using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitScene : MonoBehaviour
{
    public SceneMgr _sceneMgr;

    public Button _enterBtn;

    private void Start()
    {
        _enterBtn.onClick.AddListener(OnEnterClick);
        DontDestroyOnLoad(_sceneMgr.gameObject);
    }

    private void OnEnterClick()
    {
        _sceneMgr.SwitchScene("MainCity",true);
    }
}
