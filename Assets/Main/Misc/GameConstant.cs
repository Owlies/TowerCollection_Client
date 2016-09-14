using UnityEngine;
using System.Collections;

public enum SKILL_TYPE
{
    SKILL_SELF = 0,
    SKILL_CIRCLE,
    SKILL_LINE,
    SKILL_OTHER,
    SKILL_NONE
}

public enum SKILL_TRIGGER
{
    SKILL_TRIGGER_NONE = 0,
    SKILL_TRIGGER_GAME_START,
    SKILL_TRIGGER_UNIT_BORN,
    SKILL_TRIGGER_UNIT_DEAD,
    SKILL_TRIGGER_UNIT_ATTACKING,
    SKILL_TRIGGER_UNIT_ATTACKED
}

public enum RESOURCE_TYPE
{
    RESOURCE_PREFAB = 0,
    RESOURCE_TEXTURE,
    RESOURCE_TEXTASSET
}

public enum MOVE_TYPE
{
    MOVE_TYPE_DEFAULT = 0,
    MOVE_TYPE_FLY,
    MOVE_TYPE_RUN,
    MOVE_TYPE_DASH,
    MOVE_TYPE_STAY
}

public enum UNIT_TYPE
{
    UNIT_TYPE_MONSTER = 0,
    UNIT_TYPE_TOWER,
    UNIT_TYPE_BASE,
    UNIT_TYPE_BOSS,
}

public enum Status
{
    STATE_ALIVE,
    STATE_DEAD,
    STATE_DYING
}

public class GameConstant {
    static public float MAX_DISTANCE = 10000000.0f;

    static public string MODEL_PATH = "Model/Prefabs/";

    static public float[,] TYPE_FACTOR = {
                                            //重型护甲0，轻型皮甲1， 法师长袍2，  英雄甲3
                                            {0.75f, 	1, 		    1.25f, 	    0.75f},//普通攻击 0
                                            {1.25f, 	0.75f, 	    1,		    0.75f},//破甲攻击 1
                                            {1,			1.25f,	    0.75f, 	    0.75f},//魔法攻击 2
                                        };
}
