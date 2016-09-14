using UnityEngine;
using System.Collections;

public class UnitInfo {
    public int ID;
    public string name = "";
    public string desc = "";
    public string prefabName = "";

    //defense properties
    public float hitPoint;              //hit point or health point
    public float defense;               //defense

    //attack properties
    public float attackDamage;          //attack damage
    public float attackSpeed;           //attack speed
    public float attackRange;           //attack range

    public string rarity;               //unit rarity
    public UNIT_TYPE unitType;          //unit type
}

public class TowerInfo : UnitInfo
{
    public int attackType;              //attack type
    public int elementCrystalCost;      //element crystal cost
    public int deployCoolDown;          //deploy CoolDown
}

public class MonsterInfo : UnitInfo
{
    public int   defenseType;          //defense type
    public float viewRange;            //view range
    public float moveSpeed;            //move speed 
    public int   elementCrystalValue;  //element crystal
}