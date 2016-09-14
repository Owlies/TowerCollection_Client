using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;


public delegate bool Action(Unit unit);

public class ActionQueue {
    static Dictionary<string, ActionQueue> AIPool = new Dictionary<string, ActionQueue>();

    private List<Action> actions = new List<Action>();

    public void Perform(Unit unit)
    {
        foreach (Action action in actions)
        {
            if (action(unit))
                break;
        }
    }

    static public ActionQueue GetAIDefaultMonster()
    {
        string name = System.Reflection.MethodInfo.GetCurrentMethod().Name;
        if (AIPool.ContainsKey(name))
            return AIPool[name];

        ActionQueue AQ = new ActionQueue();
        AQ.actions.Add(Actions.BasicAction);
        AQ.actions.Add(Actions.FindTarget);
        AQ.actions.Add(Actions.AttackTarget);
        AQ.actions.Add(Actions.MoveToTarget);
        AQ.actions.Add(Actions.Idle);

        AIPool.Add(name, AQ);
        return AQ;
    }

    static public ActionQueue GetAIDefaultTower()
    {
        string name = System.Reflection.MethodInfo.GetCurrentMethod().Name;
        if (AIPool.ContainsKey(name))
            return AIPool[name];

        ActionQueue AQ = new ActionQueue();
        AQ.actions.Add(Actions.BasicAction);
        AQ.actions.Add(Actions.FindTarget);
        AQ.actions.Add(Actions.AttackTarget);
        AQ.actions.Add(Actions.Idle);

        AIPool.Add(name, AQ);
        return AQ;
    }
}
