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
    InputField inputPort = null;

    void Awake()
    {
        btnJoin.onClick.AddListener(this.OnJoinClick);
        btnCreate.onClick.AddListener(this.OnCreateClick);
    }

    void OnDestroy()
    {
        btnJoin.onClick.RemoveAllListeners();
        btnCreate.onClick.RemoveAllListeners();
    }

    // Use this for initialization
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {

    }

    bool useDefaultIpAndPort = true;
    void OnJoinClick()
    {
        if(GlobalVariables.hostType == HostType.Client)
        {
            return;
        }

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

        UInt16 port;
        try
        {
            port = UInt16.Parse(inputPort.text);
        }
        catch
        {
            if (useDefaultIpAndPort)
            {
                port = 8888;
            }
            else
            {
                UIManager uiManager = transform.parent.GetComponent<UIManager>();
                uiManager.MessageBox("illegal port", false, null);
                return;
            }
        }
        Client.serverPort = port;

        GlobalVariables.hostType = HostType.Client;
        //SceneManager.LoadScene(StringAssets.gamePlaySceneName);
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
        SceneManager.LoadScene(StringAssets.gamePlaySceneName);
    }
}
