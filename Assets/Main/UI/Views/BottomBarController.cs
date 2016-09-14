using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BottomBarController : ViewController {

	public GameObject m_towerButton = null;
	public GameObject m_buttonContainer = null;

	void Awake()
	{
		Init();
	}

	void Init()
	{
		LevelTower [] towers = new LevelTower[4];
		towers[0] = new LevelTower();
		towers[1] = new LevelTower();
		towers[2] = new LevelTower();
		towers[3] = new LevelTower();
		//towers[2] = new LevelTower();
		towers[0].towerID = 1;
        towers[0].activeSkill = 1;
        towers[1].towerID = 2;
        towers[1].activeSkill = 2;
		towers[2].towerID = 1;
		towers[2].activeSkill = 1;
		towers[3].towerID = 2;
		towers[3].activeSkill = 2;
		if(m_buttonContainer != null)
		{
			for(int i = 0;i<towers.Length;i++)
			{
				GameObject towerButton = Instantiate(m_towerButton);
				towerButton.transform.SetParent(m_buttonContainer.transform, false);
				TowerButton tower = towerButton.GetComponent<TowerButton>();
				tower.Init(towers[i]);
				tower.m_enabled = false;
			}
		}
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void CloseAll()
	{
	}
}
