using UnityEngine;
using System.Collections;

public class UnitFactor
{
    public float defenseFactor = 1;
    public float defenseDelta = 0;

    public float attackDamageFactor = 1;
    public float attackDamageDelta = 0;
    public float attackSpeedFactor = 1;
    public float attackSpeedDelta = 0;
    public float attackRangeFactor = 1;
    public float attackRangeDelta = 0;
}

public class TowerFactor : UnitFactor
{

}

public class MonsterFactor : UnitFactor
{
    public float viewRangeFactor = 1;
    public float viewRangeDelta = 0;
    public float moveSpeedFactor = 1;
    public float moveSpeedDelta = 0;
}