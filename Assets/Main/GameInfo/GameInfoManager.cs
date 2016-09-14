using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameInfoManager : MonoBehaviour {

    private static GameInfoManager m_instance;
    private GameInfoManager() { }

    public static GameInfoManager Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = GameObject.FindObjectOfType(typeof(GameInfoManager)) as GameInfoManager;
            return m_instance;
        }
    }

    List<MonsterInfo> monsterInfos;
    List<TowerInfo> towerInfos;
    List<LevelInfo> levelInfos;
    List<SkillInfo> skillINfos;
    List<ItemInfo> itemInfos;

	// Use this for initialization
	void Start () {
        DBRowObject DBObject = new DBRowObject();
        DBObject.Init();
        monsterInfos = DBObject.LoadList<MonsterInfo>("meta_Monster");
        towerInfos = DBObject.LoadList<TowerInfo>("meta_Tower");
        skillINfos = DBObject.LoadList<SkillInfo>("meta_Skill");
        levelInfos = DBObject.LoadList<LevelInfo>("meta_Level");
	}
    public SKILL_TYPE GetSkillType(int skillID)
    {
        if (skillID >= 1)
            return skillINfos[skillID - 1].skill.type;
        else
            return SKILL_TYPE.SKILL_NONE;
    }
    public SkillInfo GetSkillInfo(int skillID) { if (skillID == 0) return null; return skillINfos[skillID - 1]; }
    public MonsterInfo GetMonsterInfo(int monsterID) { if (monsterID == 0) return null; return monsterInfos[monsterID - 1]; }
    public TowerInfo GetTowerInfo(int towerID) { if (towerID == 0) return null; return towerInfos[towerID - 1]; }
    public LevelInfo GetLevelInfo(int levelID) { if (levelID == 0) return null; return levelInfos[levelID - 1]; }
    public ItemInfo GetItemInfo(int itemID) { if (itemID == 0) return null; return itemInfos[itemID - 1]; }
    public float GetTypeFactor(int AttackType, int DefenseType) { return GameConstant.TYPE_FACTOR[AttackType,DefenseType]; }

    public void Init()
    {
        //levelInfos = Util.Deserialize<List<LevelInfo>>(GameManager.Instance.GetResourceTextAsset("Data/GameData/LevelInfo").ToString());
    }

	// Update is called once per frame
	void Update () {
	
	}
}
