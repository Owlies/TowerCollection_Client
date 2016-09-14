using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelMonster
{
    public int monsterID;
    public int[] levelItemID;
    public int count;
    public float startTime;
    public float interval;

    public int[] passiveSkills;

    private float timer;
    private float localTime;
    private int currentCount;
    public void Start()
    {
        localTime = 0;
        timer = startTime;
        currentCount = 0;
    }

    public void Update(float time)
    {
        if (currentCount >= count)
            return;

        localTime += time;
        if (localTime > timer)
        {
            localTime -= timer;
            timer = interval;
            currentCount += 1;

            SceneManager.Instance.CreateMonster(this);
        }
    }
    
}

public class LevelTower
{
    public int towerID;
    public int[] levelItemID;
    public Vector3 pos;

    public int activeSkill = 0;             // tower can only have only one active skill
    public int[] passiveSkills;

    public void Start()
    {
        SceneManager.Instance.CreateTower(this);
    }

}

public class LevelInfo {
    public LevelMonster[] monsters;

    public LevelTower[] towers;
}
