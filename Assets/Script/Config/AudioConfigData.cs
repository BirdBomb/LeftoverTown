using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioConfigData 
{
    /// <summary>
    /// 1000-1999 UIAudio
    /// 2000-2999 ItemAudio
    /// 3000-3999 BuildingAudio
    /// 4000-4999 ActorAudio
    /// 9000-9999 BGM
    /// </summary>
    public readonly static List<AudioConfig> audioConfigs = new List<AudioConfig>()
    {
        new AudioConfig(){ Audio_ID = 1000,Audio_Name = "UIAudio_Succse"},
        new AudioConfig(){ Audio_ID = 1001,Audio_Name = "UIAudio_Fail"},
        new AudioConfig(){ Audio_ID = 1002,Audio_Name = "UIAudio_Siuuuu"},
        new AudioConfig(){ Audio_ID = 1003,Audio_Name = "UIAudio_PickUp"},
        new AudioConfig(){ Audio_ID = 1004,Audio_Name = "UIAudio_Build"},

        new AudioConfig(){ Audio_ID = 2000,Audio_Name = "ItemAudio_Dull_0",Audio_MaxDistance = 20},
        new AudioConfig(){ Audio_ID = 2001,Audio_Name = "ItemAudio_Shoot_0",Audio_MaxDistance = 200},
        new AudioConfig(){ Audio_ID = 2002,Audio_Name = "ItemAudio_Shoot_1",Audio_MaxDistance = 200},
        new AudioConfig(){ Audio_ID = 2003,Audio_Name = "ItemAudio_LoadRounds_0",Audio_MaxDistance = 20},
        new AudioConfig(){ Audio_ID = 2004,Audio_Name = "ItemAudio_Eat",Audio_MaxDistance = 5},
        new AudioConfig(){ Audio_ID = 2005,Audio_Name = "ItemAudio_Drink",Audio_MaxDistance = 5},
        new AudioConfig(){ Audio_ID = 2006,Audio_Name = "ItemAudio_Hoe",Audio_MaxDistance = 5},
        new AudioConfig(){ Audio_ID = 2007,Audio_Name = "ItemAudio_Spray",Audio_MaxDistance = 5},

        new AudioConfig(){ Audio_ID = 3000,Audio_Name = "BuildingAudio_WoodHit",Audio_MaxDistance = 20},
        new AudioConfig(){ Audio_ID = 3002,Audio_Name = "BuildingAudio_RockHit",Audio_MaxDistance = 20},
        new AudioConfig(){ Audio_ID = 3003,Audio_Name = "BuildingAudio_RockBomb",Audio_MaxDistance = 20},
        new AudioConfig(){ Audio_ID = 3004,Audio_Name = "BuildingAudio_DoorOpen",Audio_MaxDistance = 20},
        new AudioConfig(){ Audio_ID = 3005,Audio_Name = "BuildingAudio_DoorClose",Audio_MaxDistance = 20},

        new AudioConfig(){ Audio_ID = 4000,Audio_Name = "HumanAudio_Step",Audio_MaxDistance = 10},

        new AudioConfig(){ Audio_ID = 40010,Audio_Name = "HumanAudio_StepOnGround0",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 40011,Audio_Name = "HumanAudio_StepOnGround1",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 40012,Audio_Name = "HumanAudio_StepOnGround2",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 40013,Audio_Name = "HumanAudio_StepOnGround3",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 40014,Audio_Name = "HumanAudio_StepOnGround4",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 40015,Audio_Name = "HumanAudio_StepOnGround5",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 40016,Audio_Name = "HumanAudio_StepOnGround6",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 40017,Audio_Name = "HumanAudio_StepOnGround7",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 40018,Audio_Name = "HumanAudio_StepOnGround8",Audio_MaxDistance = 10},

        new AudioConfig(){ Audio_ID = 40020,Audio_Name = "HumanAudio_StepOnGrass0",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 40021,Audio_Name = "HumanAudio_StepOnGrass1",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 40022,Audio_Name = "HumanAudio_StepOnGrass2",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 40023,Audio_Name = "HumanAudio_StepOnGrass3",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 40024,Audio_Name = "HumanAudio_StepOnGrass4",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 40025,Audio_Name = "HumanAudio_StepOnGrass5",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 40026,Audio_Name = "HumanAudio_StepOnGrass6",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 40027,Audio_Name = "HumanAudio_StepOnGrass7",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 40028,Audio_Name = "HumanAudio_StepOnGrass8",Audio_MaxDistance = 10},

        new AudioConfig(){ Audio_ID = 40030,Audio_Name = "HumanAudio_StepOnStone0",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 40031,Audio_Name = "HumanAudio_StepOnStone1",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 40032,Audio_Name = "HumanAudio_StepOnStone2",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 40033,Audio_Name = "HumanAudio_StepOnStone3",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 40034,Audio_Name = "HumanAudio_StepOnStone4",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 40035,Audio_Name = "HumanAudio_StepOnStone5",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 40036,Audio_Name = "HumanAudio_StepOnStone6",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 40037,Audio_Name = "HumanAudio_StepOnStone7",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 40038,Audio_Name = "HumanAudio_StepOnStone8",Audio_MaxDistance = 10},


        new AudioConfig(){ Audio_ID = 9000,Audio_Name = "BGM_LeftoverLand",Audio_MaxDistance = 200},
};
}
[Serializable]
public struct AudioConfig
{
    [SerializeField]/*±àºÅ*/
    public int Audio_ID;
    [SerializeField]/*Ãû×Ö*/
    public string Audio_Name;
    [SerializeField]/*×î´ó¾àÀë*/
    public float Audio_MaxDistance;
}
