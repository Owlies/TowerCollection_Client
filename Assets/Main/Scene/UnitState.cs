using UnityEngine;
using System.Collections;

public class UnitState
{
    public int[] levelItemID;
    //defense properties
    public float hitPoint;            //hit point or health point
    public float defense;            //defense

    //attack properties
    public float attackDamage;            //attack damage
    public float attackSpeed;            //attack speed
    public float attackInterval;            //attack interval
    public float attackRange;            //attack range

    public int[] passiveSkills;

    public Unit target = null;
    public bool targetable = true;
    public bool deadable = true;
    public bool attackable = true;
    public float attackTimer = 0;


    public string rarity;
    public UNIT_TYPE unitType;

    public Status status;

    virtual public void Update()
    {
        attackTimer += Time.deltaTime;
        if (status == Status.STATE_DEAD)
        {
           target = null;
           targetable = false;
           attackable = false;
        }
    }

    virtual public void Copy(UnitState state)
    {
        if (state.levelItemID != null)
        {
            levelItemID = new int[state.levelItemID.Length];
            state.levelItemID.CopyTo(levelItemID, 0);
        }
        hitPoint = state.hitPoint;
        defense = state.defense;
        attackDamage = state.attackDamage;
        attackSpeed = state.attackSpeed;
        attackRange = state.attackRange;
        attackInterval = state.attackInterval;
        rarity = state.rarity;
        unitType = state.unitType;
        status = state.status;

        target = state.target;
        targetable = state.targetable;
        deadable = state.deadable;
        attackable = state.attackable;
    }

    virtual public void ApplyFactor(UnitState US, UnitFactor UF)
    {
        defense = (US.defense + UF.defenseDelta) * UF.defenseFactor;
        attackDamage = (US.attackDamage + UF.attackDamageDelta) * UF.attackDamageFactor;
        attackSpeed = (US.attackSpeed + UF.attackSpeedDelta) * UF.attackSpeedFactor;
        attackRange = (US.attackRange + UF.attackRangeDelta) * UF.attackRangeFactor;

        attackInterval = 1 / defense;
    }
}

public class TowerState : UnitState
{
    public override void Copy(UnitState state)
    {
        base.Copy(state);

        towerID = (state as TowerState).towerID;
        attackType = (state as TowerState).attackType;
        pos = (state as TowerState).pos;
    }

    public override void Update()
    {
        base.Update();
    }

    public TowerState() { }

    public TowerState(LevelTower levelTower)
    {
        towerID = levelTower.towerID;
        TowerInfo towerInfo = GameInfoManager.Instance.GetTowerInfo(levelTower.towerID);
        hitPoint = towerInfo.hitPoint;
        defense = towerInfo.defense;

        attackDamage = towerInfo.attackDamage;
        attackSpeed = towerInfo.attackSpeed;
        attackRange = towerInfo.attackRange;
        attackType = towerInfo.attackType;
        attackInterval = 1.0f / attackSpeed;
        rarity = towerInfo.rarity;
        unitType = towerInfo.unitType;
        status = Status.STATE_ALIVE;

        pos = levelTower.pos;
        activeSkill = levelTower.activeSkill;
        passiveSkills = levelTower.passiveSkills;
    }

    public void ApplyFactor(UnitState US, UnitFactor UF)
    {
        base.ApplyFactor(US, UF);
    }

    public int towerID;
    public int attackType;            //attack type
    public Vector3 pos;

    public int activeSkill;             // tower can only have only one active skill
}

public class MonsterState : UnitState
{
    public override void Copy(UnitState state)
    {
        base.Copy(state);

        monsterID = (state as MonsterState).monsterID;
        defenseType = (state as MonsterState).defenseType;
        viewRange = (state as MonsterState).viewRange;
        moveSpeed = (state as MonsterState).moveSpeed;
        elementCrystalValue = (state as MonsterState).elementCrystalValue;
        moveable = (state as MonsterState).moveable;
    }

    public override void Update()
    {
        base.Update();
        if (status == Status.STATE_DEAD)
        {
            moveable = false;
        }
    }

    public MonsterState() {}

    public MonsterState(LevelMonster levelMonster)
    {
        monsterID = levelMonster.monsterID;
        MonsterInfo monsterInfo = GameInfoManager.Instance.GetMonsterInfo(levelMonster.monsterID);
        hitPoint = monsterInfo.hitPoint;
        defense = monsterInfo.defense;
        defenseType = monsterInfo.defenseType;

        attackDamage = monsterInfo.attackDamage;
        attackSpeed = monsterInfo.attackSpeed;
        attackRange = monsterInfo.attackRange;
        attackInterval = 1.0f / attackSpeed;
        rarity = monsterInfo.rarity;
        unitType = monsterInfo.unitType;
        status = Status.STATE_ALIVE;

        viewRange = monsterInfo.viewRange;
        moveSpeed = monsterInfo.moveSpeed;
        elementCrystalValue = monsterInfo.elementCrystalValue;

        passiveSkills = levelMonster.passiveSkills;
    }
    public void ApplyFactor(UnitState US, UnitFactor UF)
    {
        base.ApplyFactor(US, UF);
        moveSpeed = ((US as MonsterState).moveSpeed + (UF as MonsterFactor).moveSpeedDelta) * (UF as MonsterFactor).moveSpeedFactor;
        viewRange = ((US as MonsterState).viewRange + (UF as MonsterFactor).viewRangeDelta) * (UF as MonsterFactor).viewRangeFactor;
    }

    public int   monsterID;
    public int   defenseType;                    //defense type
    public float viewRange;                      //view range
    public float moveSpeed;                      //move speed 
    public int   elementCrystalValue;            //element crystal

    public bool  moveable = true;
}