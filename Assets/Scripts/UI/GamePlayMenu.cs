using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GamePlayMenu : MonoBehaviour {
    [SerializeField]
    Button btnAddAI = null;
    [SerializeField]
    Button btnCleraAI = null;
    [SerializeField]
    Button btnExit = null;
	[SerializeField]
    Button btnQuit = null;
    [SerializeField]
    Button btnClose = null;
    void Awake()
    {
        if (GlobalVariables.hostType == HostType.Server)
        {
            btnAddAI.onClick.AddListener(this.OnAddAIClick);
            btnCleraAI.onClick.AddListener(this.OnClearAIClick);

            btnAddAI.gameObject.SetActive(true);
            btnCleraAI.gameObject.SetActive(true);

        }
        else if(GlobalVariables.hostType == HostType.Client)
        {
            btnAddAI.gameObject.SetActive(false);
            btnCleraAI.gameObject.SetActive(false);
        }
        btnExit.onClick.AddListener(this.OnExitClick);
		btnQuit.onClick.AddListener(this.OnQuitClick);
        btnClose.onClick.AddListener(this.OnCloseClick);
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDestroy()
    {
        if (GlobalVariables.hostType == HostType.Server)
        {
            btnAddAI.onClick.RemoveListener(this.OnAddAIClick);
            btnCleraAI.onClick.RemoveListener(this.OnClearAIClick);
        }
        else if (GlobalVariables.hostType == HostType.Client)
        {

        }

        btnExit.onClick.RemoveListener(this.OnExitClick);
		btnQuit.onClick.RemoveListener(this.OnQuitClick);
        btnClose.onClick.RemoveListener(this.OnCloseClick);
    }

    void OnAddAIClick()
    {
        if (GlobalVariables.hostType == HostType.Server)
        {
            ServerAgent sa = UnityHelper.GetServerAgent();
            if (sa)
            {
                sa.AddAI();
            }
        }
    }

    void OnClearAIClick()
    {
        if (GlobalVariables.hostType == HostType.Server)
        {
            ServerAgent sa = UnityHelper.GetServerAgent();
            if (sa)
            {
                sa.ClearAI();
            }
        }
    }

    void OnExitClick()
    {
        UIManager um = UnityHelper.GetUIManager();
        um.MessageBox("回到大厅?", true, this.MsgBoxCb);
    }
	void OnQuitClick()
	{
		UIManager um = UnityHelper.GetUIManager();
        um.MessageBox("退出进程?", true, this.MsgBoxCbQuit);
	}
    void MsgBoxCb(bool confirm)
    {
        if (confirm)
        {
            UnityHelper.LoadSceneAsync(StringAssets.mainMenuSceneName);
        }
    }
	void MsgBoxCbQuit(bool confirm)
    {
        if (confirm)
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
    void OnCloseClick()
    {
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        GlobalVariables.menuOpened = true;
    }

    void OnDisable()
    {
        GlobalVariables.menuOpened = false;
    }
}
