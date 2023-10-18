﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    Button btnJoin = null;
    [SerializeField]
    Button btnCreate = null;
    [SerializeField]
    InputField inputIp = null;
    [SerializeField]
    Button btnQuit = null;

	[SerializeField]
    Text txtInfo = null;
    
	void Awake()
    {
        btnJoin.onClick.AddListener(this.OnJoinClick);
        btnCreate.onClick.AddListener(this.OnCreateClick);
        btnQuit.onClick.AddListener(this.OnQuitClick);
    }

    void OnDestroy()
    {
        btnJoin.onClick.RemoveAllListeners();
        btnCreate.onClick.RemoveAllListeners();
        btnQuit.onClick.RemoveAllListeners();
    }

    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
		txtInfo.text = string.Format("分辩率:{0}x{1}", Screen.width, Screen.height);
    }

    bool useDefaultIpAndPort = false;
    void OnJoinClick()
    {
        string ip = inputIp.text;
        if (CommonHelper.IpLegal(ip))
        {
            Client.serverIp = ip;
        }
        else
        {
            if (useDefaultIpAndPort)
            {
                Client.serverIp = "192.168.1.88";
            }
            else
            {
                UIManager uiManager = transform.parent.GetComponent<UIManager>();
                uiManager.MessageBox("illegal ip", false, null);
                return;
            }
        }

        UInt16 port = Server.localPort;
        //try
        //{
        //    port = UInt16.Parse(inputPort.text);
        //}
        //catch
        //{
        //    if (useDefaultIpAndPort)
        //    {
        //        port = 7887;
        //    }
        //    else
        //    {
        //        UIManager uiManager = transform.parent.GetComponent<UIManager>();
        //        uiManager.MessageBox("illegal port", false, null);
        //        return;
        //    }
        //}
        Client.serverPort = port;

        GlobalVariables.hostType = HostType.Client;
        LoginManager lm = GameObject.Find("LoginManager").GetComponent<LoginManager>();
        lm.StartClient();
    }

    void OnCreateClick()
    {
        if (GlobalVariables.hostType == HostType.Client)
        {
            LoginManager lm = GameObject.Find("LoginManager").GetComponent<LoginManager>();
            lm.StopClient();

            GlobalVariables.hostType = HostType.Server;
        }
        UnityHelper.LoadSceneAsync(StringAssets.gamePlaySceneName);
    }

    void OnQuitClick()
    {
			if(Application.isEditor) {
#if UNITY_EDITOR
				UnityEditor.EditorApplication.ExitPlaymode();
#endif
			} else {
				Application.Quit();
			}
    }
}
