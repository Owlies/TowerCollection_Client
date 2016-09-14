using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneManager : MonoBehaviour {

    private static SceneManager m_instance;
    private SceneManager() { }

    public static SceneManager Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = GameObject.FindObjectOfType(typeof(SceneManager)) as SceneManager;
            return m_instance;
        }
    }

    public LevelInfo currentLevelInfo;
    public List<Monster> monsters;
    public List<Tower> towers;
    public Tower baseTower;

    bool levelEnabled = false;
    public int ElementCrystalNum;
	private float levelTime = 120;

	// Use this for initialization
    void Start()
    {
        monsters = new List<Monster>();
        towers = new List<Tower>();
	}
	
    public void Init()
    {
        LoadLevel(1);
        levelEnabled = true;
        ElementCrystalNum = 0;

    }

	// Update is called once per frame
	void Update () {
        if (!levelEnabled)
            return;

        UpdateLevel();

		UIManager.Instance.SetLevelTime((int)levelTime);
		levelTime -= Time.deltaTime;
		levelTime = levelTime < 0 ? 0 : levelTime;

        //tmp
        for (int i = monsters.Count - 1; i >= 0; i--)
		{
            if (monsters[i].currentState.status == Status.STATE_DEAD)
            {
                monsters[i].Remove();
                GameManager.Instance.SendEvent(EVT_TYPE.EVT_TYPE_UNIT_DIE, monsters[i], true);
                monsters.RemoveAt(i);
            }
			else
			{
				monsters[i].MyUpdate();
			}
		}

        for (int i = towers.Count - 1; i >= 0; i--)
		{
            if (towers[i].currentState.status == Status.STATE_DEAD)
            {
                towers[i].Remove();
                towers.RemoveAt(i);
            }
			else
			{
				towers[i].MyUpdate();
			}
		}

		JudgeVictory();
	}

	public void JudgeVictory()
	{
		if(levelTime > 0)
		{
			if(baseTower.currentState.status == Status.STATE_DEAD)
			{
				levelEnabled = false;
				UIManager.Instance.GameOver(false);
			}
		}
		else
		{
			levelEnabled = false;
			UIManager.Instance.GameOver(true);
		}
	}

    public void UpdateLevel()
    {
        foreach(LevelMonster levelMonster in currentLevelInfo.monsters)
            levelMonster.Update(Time.deltaTime);
    }

    public void LoadLevel(int levelID)
    {
        currentLevelInfo = GameInfoManager.Instance.GetLevelInfo(levelID);
        foreach (LevelMonster levelMonster in currentLevelInfo.monsters)
            levelMonster.Start();

        if (currentLevelInfo.towers != null)
        {
            foreach (LevelTower levelTower in currentLevelInfo.towers)
                levelTower.Start();
        }

        foreach(Tower tower in towers)
            if (tower.isBase())
                baseTower = tower;

        if (baseTower == null)
            Debug.LogError("no base in the tower set!!!! Plz add one base tower in the levelInfo");
    }

    public void CreateMonster(LevelMonster levelMonster)
    {
        MonsterInfo monsterInfo = GameInfoManager.Instance.GetMonsterInfo(levelMonster.monsterID);
        GameObject monsterGO = GameObject.Instantiate(GameManager.Instance.GetResourceObject(GameConstant.MODEL_PATH + monsterInfo.prefabName));
        monsterGO.AddComponent<Monster>();
        monsterGO.transform.SetParent(this.transform);
        Monster monster = monsterGO.GetComponent<Monster>();
        MonsterState state = new MonsterState(levelMonster);
        monster.Init(state);

        monsters.Add(monster);
    }

	public GameObject CreateTower(LevelTower levelTower)
    {
        TowerInfo towerInfo = GameInfoManager.Instance.GetTowerInfo(levelTower.towerID);
        GameObject towerGO = GameObject.Instantiate(GameManager.Instance.GetResourceObject(GameConstant.MODEL_PATH + towerInfo.prefabName));
        towerGO.AddComponent<Tower>();
        towerGO.transform.SetParent(this.transform);
        Tower tower = towerGO.GetComponent<Tower>();
        TowerState state = new TowerState(levelTower);
        tower.Init(state);

        towers.Add(tower);
		return towerGO;
    }
}
