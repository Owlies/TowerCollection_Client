using UnityEngine;
using System.Collections;

public class GameLoopEventHandler{
    public static void EnterGame(Event evt)
    {
        // init the whole the system
        //UIManager.Instance.Init();
        GameInfoManager.Instance.Init();
        SceneManager.Instance.Init();
		UIManager.Instance.Init();

		// Close current screen for now...
    }

    public static void UnitDie(Event evt)
    {
        Unit unit = evt.evt_obj[0] as Unit;
        if (unit is Monster)
        {
            GameObject crystalUI = GameObject.Instantiate(ResourceManager.Instance.GetResourceObject("Model/Prefabs/CrystalUI"));
            crystalUI.transform.position = unit.transform.position + Vector3.up * 20;
            crystalUI.GetComponent<CrystalUI>().SetCrystalValue((unit.currentState as MonsterState).elementCrystalValue);
        }
    }

    public static void ChangeElementCrystal(Event evt)
    {
        int delta = (int)evt.evt_obj[0];
        SceneManager.Instance.ElementCrystalNum += delta;
        if (SceneManager.Instance.ElementCrystalNum > 100)
            SceneManager.Instance.ElementCrystalNum = 100;
        else if (SceneManager.Instance.ElementCrystalNum < 0)
        {
            SceneManager.Instance.ElementCrystalNum = 0;
            Debug.LogWarning("how can <0 happen?");
        }
        UIManager.Instance.AddCrystalUI(delta);
    }

	public static void ReleaseSkill(Event evt)
	{
		Vector3 position = (Vector3)evt.evt_obj[0];
		LevelTower tower = (LevelTower)evt.evt_obj[1];
        GameObject gameObject = (GameObject)evt.evt_obj[2];
        Tower towerUnit = gameObject.GetComponent<Tower>();
        towerUnit.Ultimate(evt.evt_obj);
		Debug.Log("tower : " + tower.towerID + " release skill at : " + position);
	}
		
}
