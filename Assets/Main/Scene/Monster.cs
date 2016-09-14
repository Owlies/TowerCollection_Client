using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monster : Unit
{
	// Use this for initialization
    public void Init(MonsterState monsterState)
    {
        base.Init();
        gameObject.transform.transform.position = new Vector3(Random.Range(100, 200), 0, 500);
		AdjustStatusUIPos();

        baseState = monsterState;
        stateFactor = new MonsterFactor();
        currentState = new MonsterState();
        currentState.Copy(baseState);

        AI = ActionQueue.GetAIDefaultMonster();
	}
	
	// Update is called once per frame
	public void MyUpdate () {

        stateFactor = new MonsterFactor();
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
}
