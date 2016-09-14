using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreenController : ViewController {

    public Text percentText;
	public Button startButton;
	public GameObject loginPanel;
	public Button emailAccount;
	public Button googleAccount;
	public Button facebookAccount;
	public RectTransform accountPanel;
	public Text accountType;
	public Button createAccount;

	public bool needLogin = true;

	private float accountPanelProcess = 0;
	private string nextAccountType = "";

	// Use this for initialization
	void Start () {
        GameManager.Instance.BindEvent(EVT_TYPE.EVT_TYPE_PRELOAD_PARTIAL_FINISH, new Handler(UpdatePercent));
        GameManager.Instance.BindEvent(EVT_TYPE.EVT_TYPE_PRELOAD_TOTAL_FINISH, new Handler(FinishLoad));
		startButton.interactable = false;
		startButton.onClick.AddListener(()=>{
			//SendEvent(EVT_TYPE.EVT_TYPE_ENTER_GAME);
			GameManager.Instance.SendEvent(EVT_TYPE.EVT_TYPE_CHANGE_SCREEN, "GameScreen");
		});

		emailAccount.onClick.AddListener(()=>{
			ToggleAccountPanel("Email Login");
			emailAccount.interactable = false;
			googleAccount.interactable = true;
			facebookAccount.interactable = true;
		});

		googleAccount.onClick.AddListener(()=>{
			ToggleAccountPanel("Google Login");
			emailAccount.interactable = true;
			googleAccount.interactable = false;
			facebookAccount.interactable = true;
		});

		facebookAccount.onClick.AddListener(()=>{
			ToggleAccountPanel("Facebook Login");
			emailAccount.interactable = true;
			googleAccount.interactable = true;
			facebookAccount.interactable = false;
		});

		// todo: check account info
		createAccount.onClick.AddListener(()=>{
			startButton.interactable = true;
		});
	}
	
    private void UpdatePercent(Event evt)
    {
        percentText.text = (float)evt.evt_obj[1] * 100 + "%";
        //Debug.Log((float)evt.evt_obj[1] * 100 + "%");
    }

    private void FinishLoad(Event evt)
    {
		//startButton.interactable = true;
		CheckNeedLogin();
    }

	// Update is called once per frame
	void Update () {
	
	}

	public void CheckNeedLogin()
	{
		startButton.interactable = !needLogin;
		if(needLogin)
		{
			loginPanel.SetActive(true);
		}
		else
		{
			loginPanel.SetActive(false);
		}

	}

	public void ToggleAccountPanel(string account)
	{
		// it's opened.
		//if(accountPanelProcess > 0)
		accountPanel.gameObject.SetActive(true);
		accountType.text = "Enter " + account + " info:";
	}
}
