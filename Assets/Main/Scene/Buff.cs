using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

public class Buffs
{
    List<Buff> buffPool = new List<Buff>();

    public void addBuff(Buff buff)
    {
        buff.Enter();
        buffPool.Add(buff);
    }

    static public void addDefenceBUff(Unit unit)
    {
        Buff defenseBuff = new Buff();
        defenseBuff.cost = 20;
        defenseBuff.coolDownTime = 15.0f;
        defenseBuff.type = SKILL_TYPE.SKILL_SELF;
        defenseBuff.trigger = SKILL_TRIGGER.SKILL_TRIGGER_NONE;
        defenseBuff.duration = 10.0f;
        defenseBuff.fieldName = "defenseFactor";
        defenseBuff.deltaValue = 0.4f;

        unit.buffs.addBuff(defenseBuff);
    }

    static public void addAttackBuff(Unit unit)
    {
        Buff attackBuff = new Buff();
        attackBuff.cost = 20;
        attackBuff.coolDownTime = 15.0f;
        attackBuff.type = SKILL_TYPE.SKILL_SELF;
        attackBuff.trigger = SKILL_TRIGGER.SKILL_TRIGGER_NONE;
        attackBuff.duration = 10.0f;
        attackBuff.fieldName = "attackDamageFactor";
        attackBuff.deltaValue = 0.4f;

        unit.buffs.addBuff(attackBuff);
    }

    public void Update(Unit unit)
    {
        for (int i = buffPool.Count - 1; i >= 0; i--)
        {
            buffPool[i].Activate(unit,null);
            if (buffPool[i].needExit)
            {
                buffPool[i].Exit();
                buffPool.RemoveAt(i);
            }
        }
    }
}

[Serializable]
public class Buff : Skill
{
    public float duration = 0;
    public float intervalTime = 0;
    public string fieldName = "";
    public float deltaValue = 0;


    // none preset fields
    private float time = 0;
    public bool needExit = false;

    public void Enter()
    {
        Debug.Log(fieldName + " Start!");
    }

    public void Exit()
    {
        Debug.Log(fieldName + " End!");
    }

    public void Activate(Unit unit, object data)
    {
        time += Time.deltaTime;
        if (time > duration)
            needExit = true;

        UnitFactor factor = unit.stateFactor;
        FieldInfo fieldInfo = factor.GetType().GetField(fieldName);
        if (fieldInfo != null)
        {
            float value_Old = (float)fieldInfo.GetValue(factor);
            fieldInfo.SetValue(factor, value_Old + deltaValue);
        }

    }

}
