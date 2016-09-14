using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower : Unit
{

    public bool isBase()
    {
        return baseState.unitType == UNIT_TYPE.UNIT_TYPE_BASE;
    }

	// Use this for initialization
    public void Init(TowerState towerState)
    {
        base.Init();
        gameObject.transform.position = towerState.pos;
		AdjustStatusUIPos();

        baseState = towerState;
        stateFactor = new TowerFactor();
        currentState = new TowerState();
        currentState.Copy(baseState);

        AI = ActionQueue.GetAIDefaultTower();

	}
	
	// Update is called once per frame
	public void MyUpdate () {

        stateFactor = new TowerFactor();
        currentState.Update();
        buffs.Update(this);
        currentState.ApplyFactor(baseState, stateFactor);

        if (currentState.status == Status.STATE_ALIVE)
        {
            if (AI != null)
                AI.Perform(this);
        }

        if (status) 
            status.SetHP(currentState.hitPoint / baseState.hitPoint);
	}

    public void Ultimate(ArrayList obj)
    {
        LevelTower tower = (LevelTower)obj[1];
        SkillInfo skillInfo = GameInfoManager.Instance.GetSkillInfo(tower.activeSkill);
        if (skillInfo != null && skillInfo.skill.GetType() == typeof(Buff))
        {
            Buff buff = (Buff)skillInfo.skill.Clone();
            buffs.addBuff(buff);
        }
    }
}
