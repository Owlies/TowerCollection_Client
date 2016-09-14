using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIDebugManager : ViewController {

	public InputField m_inputPrefab;
	public Button m_buttonPrefab;

	public GameObject m_emptyPanel;

	public Button m_preButton;
	public Button m_nextButton;
	public Button m_exitButton;

	// Just for sending events
	private InputField m_eventInput;


	private Dictionary<string, GameObject> m_panels;
	//private List<GameObject> m_panelList;
	private GameObject m_currentPanel;

	// Use this for initialization
	void Start () {
		m_panels = new Dictionary<string, GameObject>();
		//m_button.onClick.AddListener(SendDebugEvent);

		m_eventInput = AddObject("SendEvent", m_inputPrefab.gameObject).GetComponent<InputField>();
		AddButton("SendEvent", "send it", SendDebugEvent);

		AddObject("NewEvent", m_inputPrefab.gameObject).GetComponent<InputField>();
		AddObject("NewEvent", m_inputPrefab.gameObject).GetComponent<InputField>();

		AddButton("AnotherEvent", "a it", SendDebugEvent);
		AddButton("AnotherEvent", "b it", SendDebugEvent);
		AddButton("AnotherEvent", "c it", SendDebugEvent);

		m_preButton.onClick.AddListener(()=>{MovePanel(false);});
		m_nextButton.onClick.AddListener(()=>{MovePanel(true);});
		m_exitButton.onClick.AddListener(()=>{UIManager.Instance.SetDebugUIEnable(false);});

	}

	void MovePanel(bool toNext)
	{
		if(m_panels.Values.Count <= 0)
			return;
		
		List<GameObject> panels = new List<GameObject>(m_panels.Values);
		if(panels == null || panels.Count == 0)
		{
			return;
		}

		if(m_currentPanel == null)
		{
			m_currentPanel = panels[0];
			m_currentPanel.SetActive(true);
			return;
		}
			
		m_currentPanel.SetActive(false);
		if(toNext)
		{
			for(int i = 0;i<panels.Count;i++)
			{
				if(m_currentPanel == panels[i])
				{
					m_currentPanel = i == panels.Count-1 ? panels[0] : panels[i+1];
					break;
				}
			}
		}
		else
		{
			for(int i = panels.Count-1;i>=0;i--)
			{
				if(m_currentPanel == panels[i])
				{
					m_currentPanel = i == 0 ? panels[panels.Count-1] : panels[i-1];
					break;
				}
			}
		}
		m_currentPanel.SetActive(true);

	}

	GameObject AddObject(string panelName, GameObject newObject)
	{
		AddPanelInDirectory(panelName);

		GameObject panel = m_panels[panelName];
		GameObject obj = Instantiate(newObject);
		obj.transform.SetParent(panel.transform, false);
		panel.GetComponent<VerticalLayoutGroup>().padding.bottom -= 40;

		return obj;
	}

	Button AddButton(string panelName, string buttonName, UnityAction action)
	{
		AddPanelInDirectory(panelName);

		GameObject panel = m_panels[panelName];
		Button button = Instantiate(m_buttonPrefab);
		button.transform.SetParent(panel.transform, false);
		button.GetComponentInChildren<Text>().text = buttonName;
		button.onClick.AddListener(action);
		panel.GetComponent<VerticalLayoutGroup>().padding.bottom -= 40;

		return button;
	}

	void AddPanelInDirectory(string panelName)
	{
		if(!m_panels.ContainsKey(panelName))
		{
			GameObject newpanel = Instantiate(m_emptyPanel);
			newpanel.name = panelName;
			newpanel.transform.SetParent(transform, false);
			m_panels.Add(panelName, newpanel);

			newpanel.SetActive(false);			
			if(m_currentPanel == null)
			{
				newpanel.SetActive(true);
				m_currentPanel = newpanel;
			}
		}
	}

	void SendDebugEvent()
	{
		string debugInput = m_eventInput.text;
		EVT_TYPE evt = (EVT_TYPE) System.Enum.Parse(typeof(EVT_TYPE), debugInput);
		SendEvent(evt);
	}
}
