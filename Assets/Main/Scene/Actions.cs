using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Actions {

    static float Distance2D(Unit unit1, Unit unit2)
    {
        Vector3 pos1 = unit1.transform.position;
        Vector3 pos2 = unit2.transform.position;
        Vector3 rerfPos = pos2 - pos1;
        return Mathf.Sqrt(rerfPos.x * rerfPos.x + rerfPos.z * rerfPos.z);
    }

    static Vector3 Direction2D(Unit src, Unit dest)
    {
        Vector3 pos1 = src.transform.position;
        Vector3 pos2 = dest.transform.position;
        Vector3 rerfPos = pos2 - pos1;
        rerfPos.y = 0;
        rerfPos.Normalize();

        return rerfPos;
    }

    static public bool BasicAction(Unit unit)
    {
        if (unit.currentState.hitPoint <= 0 && unit.currentState.deadable)
            unit.currentState.status = Status.STATE_DEAD;

        if (unit.currentState.status == Status.STATE_ALIVE)
            return false;
        else
            return true;
    }

    static public bool FindTarget(Unit unit)
    {
        if (unit is Monster)
        {
            List<Tower> towers = SceneManager.Instance.towers;

            foreach(Tower tower in towers)
            {
                if (!tower.currentState.targetable)
                    continue;

                float dis = Vector3.Distance(unit.gameObject.transform.position, tower.gameObject.transform.position);
                if (dis < (unit.currentState as MonsterState).viewRange)
                    unit.currentState.target = tower;
            }

            if (unit.currentState.target == null)
                unit.currentState.target = SceneManager.Instance.baseTower;
        }
        else if (unit is Tower && unit.currentState.target == null)
        {
            List<Monster> monsters = SceneManager.Instance.monsters;
            float minDis = unit.currentState.attackRange;
            int index = -1;
            for (int i = 0; i < monsters.Count; i++)
            {
                if (!monsters[i].currentState.targetable)
                    continue;

                float dis = Distance2D(unit, monsters[i]);
                if (minDis > dis)
                {
                    index = i;
                    minDis = dis;
                }
            }

            if (index != -1)
                unit.currentState.target = monsters[index];
        }

        return false;
    }

    static public bool AttackTarget(Unit unit)
    {
        if (!unit.currentState.attackable)
            return true;
        Unit target = unit.currentState.target;
        if (target == null)
            return false;
        float dis = Distance2D(unit, target);
        if (dis > unit.currentState.attackRange)
            return false;
        if (unit.currentState.attackTimer > unit.currentState.attackInterval)
        {
            float value = unit.currentState.attackDamage - target.currentState.defense;
            if (unit is Tower)
                value *= GameInfoManager.Instance.GetTypeFactor((unit.currentState as TowerState).attackType, (target.currentState as MonsterState).defenseType);
            target.currentState.hitPoint -= Mathf.Max(value,1);
            unit.currentState.attackTimer = 0;
        }
        return true;
    }

    static public bool MoveToTarget(Unit unit)
    {
        if (unit is Monster)
        {
            if (unit.currentState.target != null && (unit.currentState as MonsterState).moveable)
            {
                Unit target = unit.currentState.target;
                if (target == null)
                    return false;
                Vector3 dir = Direction2D(unit, target);

                unit.transform.Translate(dir * (unit.currentState as MonsterState).moveSpeed * Time.deltaTime);
            }
        }
        return true;
    }

    static public bool Idle(Unit unit)
    {
        return false;
    }
}
