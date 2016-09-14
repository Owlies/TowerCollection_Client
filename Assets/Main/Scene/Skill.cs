using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary; 
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; 

[Serializable]
public class Skill : ICloneable 
{
    public int cost = 0;
    public float coolDownTime = 0;
    public SKILL_TYPE type = SKILL_TYPE.SKILL_SELF;
    public SKILL_TRIGGER trigger = SKILL_TRIGGER.SKILL_TRIGGER_NONE;

    virtual public void Activate(Unit unit, object data)
    {
    }

    virtual public void Enter()
    {
    }

    virtual public void Exit()
    {
    }

public object Clone()   {
    MemoryStream ms = new MemoryStream();
    BinaryFormatter bf = new BinaryFormatter();
    bf.Serialize(ms, this);
    ms.Seek(0, 0);
    object obj = bf.Deserialize(ms);
    ms.Close();
    return obj;
}   
}

[Serializable]
public class BaseSkill : Skill
{
    public string fieldName;
    public float deltaValue;

    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void Activate(Unit unit, object data)
    {
        UnitFactor factor = unit.stateFactor;
        FieldInfo fieldInfo = factor.GetType().GetField(fieldName);
        float value_Old = (float)fieldInfo.GetValue(factor);
        fieldInfo.SetValue(factor, value_Old + deltaValue);
    }
}

[Serializable]
public class CircleSkill : Skill
{
    public float range = 0;
    public string fieldName;
    public float deltaValue;

    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void Activate(Unit unit, object data)
    {
        UnitFactor factor = unit.stateFactor;
        FieldInfo fieldInfo = factor.GetType().GetField(fieldName);
        float value_Old = (float)fieldInfo.GetValue(factor);
        fieldInfo.SetValue(factor, value_Old + deltaValue);
    }
}