using Fusion;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerCoreLocal : MonoBehaviour
{
    [SerializeField]
    private PlayerCoreNet playerCoreNet;
    //[HideInInspector]
    public bool bool_Local, bool_State = false;

    private void Start()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_AllClient_SomeoneMove>().Subscribe(_ =>
        {
            if (bool_Local && _.moveActor == actorManager_Bind)
            {
                Local_UpdateMapInView(_.movePos);
                Local_UpdateNearbyTile(_.movePos);
            }
        }).AddTo(this);
        #region//背包物体
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_ItemBag_Add>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.Local_ItemBag_Add(_.index, _.itemData,_.itemFrom);
                Local_SaveActorData();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_ItemBag_Expend>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.Local_ItemBag_Expend(_.itemID, _.itemCount);
                Local_SaveActorData();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_ItemBag_Change>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.Local_ItemBag_Change(_.index,_.itemData);
                Local_SaveActorData();
            }
        }).AddTo(this);

        #endregion
        #region//手部物体
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_ItemHand_Add>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.Local_ItemHand_Add(_.item);
                Local_SaveActorData();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_ItemHand_Sub>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.Local_ItemHand_Sub(_.item);
                Local_SaveActorData();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_ItemHand_Change>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.Local_ItemHand_Change(_.oldItem, _.newItem);
                Local_SaveActorData();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_ItemHand_Switch>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actionManager.ItemHand_Switch(_.index);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_ItemHand_PutAway>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                ItemData itemNew = new ItemData(0);
                actorManager_Bind.actorNetManager.Local_ItemHand_Add(itemNew);
            }
        }).AddTo(this);
        #endregion
        #region//头部物体
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_ItemHead_Add>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.Local_ItemHead_Add(_.item);
                Local_SaveActorData();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_ItemHead_Sub>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.Local_ItemHead_Sub(_.item);
                Local_SaveActorData();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_ItemHead_Change>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.Local_ItemHead_Change(_.oldItem, _.newItem);
                Local_SaveActorData();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_ItemHead_Switch>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actionManager.ItemHead_Switch(_.index);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_ItemHead_PutAway>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                ItemData itemNew = new ItemData(0);
                actorManager_Bind.actorNetManager.Local_ItemHead_Add(itemNew);
            }
        }).AddTo(this);
        #endregion
        #region//身体物体
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_ItemBody_Add>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.Local_ItemBody_Add(_.item);
                Local_SaveActorData();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_ItemBody_Sub>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.Local_ItemBody_Sub(_.item);
                Local_SaveActorData();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_ItemBody_Change>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.Local_ItemBody_Change(_.oldItem, _.newItem);
                Local_SaveActorData();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_ItemBody_Switch>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actionManager.ItemBody_Switch(_.index);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_ItemBody_PutAway>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                ItemData itemNew = new ItemData(0);
                actorManager_Bind.actorNetManager.Local_ItemBody_Add(itemNew);
            }
        }).AddTo(this);
        #endregion
        #region//饰品物体
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_ItemAccessory_Add>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.Local_ItemAccessory_Add(_.item);
                Local_SaveActorData();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_ItemAccessory_Sub>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.Local_ItemAccessory_Sub(_.item);
                Local_SaveActorData();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_ItemAccessory_Change>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.Local_ItemAccessory_Change(_.oldItem, _.newItem);
                Local_SaveActorData();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_ItemAccessory_Switch>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actionManager.ItemAccessory_Switch(_.index);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_ItemAccessory_PutAway>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                ItemData itemNew = new ItemData(0);
                actorManager_Bind.actorNetManager.Local_ItemAccessory_Add(itemNew);
            }
        }).AddTo(this);

        #endregion
        #region//耗材物体
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_ItemConsumables_Add>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.Local_ItemConsumables_Add(_.item);
                Local_SaveActorData();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_ItemConsumables_Sub>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.Local_ItemConsumables_Sub(_.item);
                Local_SaveActorData();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_ItemConsumables_Change>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.Local_ItemConsumables_Change(_.oldItem, _.newItem);
                Local_SaveActorData();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_ItemConsumables_Switch>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actionManager.ItemConsumables_Switch(_.index);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_ItemConsumables_PutAway>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                ItemData itemNew = new ItemData(0);
                actorManager_Bind.actorNetManager.Local_ItemConsumables_Add(itemNew);
            }
        }).AddTo(this);
        #endregion
        #region//Buff
        #endregion
        #region//Level
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_AddExp>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                Local_ExpUp(_.exp);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_LevelUp>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                Local_LevelUp(_.count);
            }
        }).AddTo(this);
        #endregion
        #region//Skill
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_AddSkill>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                Local_AddSkill(_.id);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_ClearSkill>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                Local_ClearSkill();
            }
        }).AddTo(this);
        #endregion
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryDropItem>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                MessageBroker.Default.Publish(new GameEvent.GameEvent_Local_SpawnItem()
                {
                    itemData = _.item,
                    itemOwner = actorManager_Bind.actorNetManager.Object.Id,
                    pos = actorManager_Bind.transform.position - new Vector3(0, 0.1f, 0),
                });
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TrySpawnActor>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                MessageBroker.Default.Publish(new GameEvent.GameEvent_Local_SpawnActor()
                {
                    name = _.name,
                    pos = actorManager_Bind.transform.position
                });
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_SendEmoji>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actionManager.AllClient_SendEmoji((short)_.emoji, 1, false, 10);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_SendText>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actionManager.AllClient_SendText(_.text, (int)Emoji.Yell, 1, false, 1);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_QuestComplete>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                Local_CompleteQuest(_.id);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryEarn>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actionManager.EarnCoin(_.coin);
            }
        }).AddTo(this);
    }
    private void Update()
    {
        if (bool_Local && actorManager_Bind != null)
        {
            Local_PlayerInputKey();
            Local_FindNearestNearbyTile();
        }
    }
    #region//角色初始化
    public void AllClinet_InitPlayer(bool input, bool state)
    {
        bool_Local = input;
        bool_State = state;
    }
    #endregion
    #region//角色绑定
    public ActorManager actorManager_Bind
    {
        get 
        {
            return actorManager_Bind_Net;
        }
        set 
        {
            actorManager_Bind_Net = value;
        }
    }
    public ActorManager actorManager_Bind_Net = null;
    public void AllClinet_BindActor_Net(ActorManager actor)
    {
        actorManager_Bind = actor;
        actorManager_Bind.AllClient_BindPlayer(bool_State, bool_Local, playerCoreNet.Object.InputAuthority);
        if (bool_Local)
        {
            Local_BindActor();
            Local_SetActorData();
        }
        if (bool_State)
        {
            State_BindActor();
        }
    }
    public void Local_BindActor()
    {

        CameraManager.Instance.FollowTarget(actorManager_Bind.transform);
        MessageBroker.Default.Publish(new GameEvent.GameEvent_Local_BindLocalPlayer()
        {
            playerCore = this
        });
    }
    public void State_BindActor()
    {
        
    }
    #endregion
    #region//玩家专有信息
    #region//Skill
    /// <summary>
    /// 玩家技能
    /// </summary>
    private List<short> playerSkillList { get; } = new List<short>();
    /// <summary>
    /// 玩家等级
    /// </summary>
    private int playerLevel { get; set; }
    /// <summary>
    /// 玩家经验值
    /// </summary>
    private int playerExp { get; set; }
    public int Local_GetLevel()
    {
        return playerLevel;
    }
    public int Local_GetExp()
    {
        return playerExp;
    }
    public List<short> Local_GetSkillList()
    {
        return new List<short>(playerSkillList);
    }
    public void Local_SetLevelAndExp(int level, int exp)
    {
        Local_LevelUp(level - playerLevel);
        Local_ExpUp(exp - playerExp);
    }
    public void Local_SetSkillList(List<short> skillDatas)
    {
        Local_ClearSkill();
        for (int i = 0; i < skillDatas.Count; i++)
        {
            Local_AddSkill(skillDatas[i]);
        }

    }
    public void Local_ExpUp(int val)
    {
        int levelUp = 0;
        int expCur = playerExp + val;
        int expCapacity = (playerLevel + levelUp) * 5 + 45;
        while (expCur > expCapacity)
        {
            levelUp++;
            expCur = expCur - expCapacity;
            expCapacity = (playerLevel + levelUp) * 5 + 45;
        }
        Local_LevelUp(levelUp);
        playerExp = expCur;
        int skillPoint = playerLevel;
        for (int i = 0; i < playerSkillList.Count; i++)
        {
            skillPoint -= SkillConfigData.GetStatusConfig(playerSkillList[i]).Skill_Cost;
        }
        MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateSkill()
        {
            Skills = new List<short>(playerSkillList),
            Point = skillPoint
        });
        MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateExpData()
        {
            Level = playerLevel,
            Exp_Cur = playerExp,
            Exp_Capacity = expCapacity,
        });
    }
    public void Local_LevelUp(int val)
    {
        playerLevel += val;
        int skillPoint = playerLevel;
        int expCapacity = playerLevel * 5 + 45;

        for (int i = 0; i < playerSkillList.Count; i++)
        {
            skillPoint -= SkillConfigData.GetStatusConfig(playerSkillList[i]).Skill_Cost;
        }
        MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateSkill()
        {
            Skills = new List<short>(playerSkillList),
            Point = skillPoint
        });
        MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateExpData()
        {
            Level = playerLevel,
            Exp_Cur = playerExp,
            Exp_Capacity = expCapacity,
        });
    }
    public void Local_AddSkill(short skill)
    {
        if (!playerSkillList.Contains(skill))
        {
            playerSkillList.Add(skill);

            int skillPoint = playerLevel;
            for (int i = 0; i < playerSkillList.Count; i++)
            {
                skillPoint -= SkillConfigData.GetStatusConfig(playerSkillList[i]).Skill_Cost;
            }
            actorManager_Bind.actorNetManager.Local_AddBuff(skill, 0, Vector3Int.zero);
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateSkill()
            {
                Skills = new List<short>(playerSkillList),
                Point = skillPoint
            });
        }
    }

    #endregion
    #region//Quest
    /// <summary>
    /// 玩家已完成任务
    /// </summary>
    private List<int> playerCompleteQuestList { get; } = new List<int>();
    private short Local_QuestCurLevel;
    private int Local_QuestId;
    public List<int> Local_GetQuestList()
    {
        return new List<int>(playerCompleteQuestList);
    }
    public void Local_ClearSkill()
    {
        for (int i = 0; i < playerSkillList.Count; i++)
        {
            //Local_AddSkill(skillDatas[i]);
        }
        playerSkillList.Clear();
        int skillPoint = playerLevel;
        for (int i = 0; i < playerSkillList.Count; i++)
        {
            skillPoint -= SkillConfigData.GetStatusConfig(playerSkillList[i]).Skill_Cost;
        }
        MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateSkill()
        {
            Skills = new List<short>(playerSkillList),
            Point = skillPoint
        });

    }
    /// <summary>
    /// 获取当前任务等级
    /// </summary>
    /// <returns></returns>
    public short Local_GetQuestLevel()
    {
        return Local_QuestCurLevel;
    }
    /// <summary>
    /// 任务完成
    /// </summary>
    /// <param name="questID"></param>
    public void Local_CompleteQuest(int questID)
    {
        playerCompleteQuestList.Add(questID);
        Local_UpdateQuestList(new List<int>(playerCompleteQuestList));
    }
    /// <summary>
    /// 更新任务等级
    /// </summary>
    /// <param name="level"></param>
    public void Local_UpdateQuestLevel(short level)
    {
        Local_QuestCurLevel = level;
    }
    /// <summary>
    /// 更新已完成任务
    /// </summary>
    /// <param name="questDatas"></param>
    public void Local_UpdateQuestList(List<int> questDatas)
    {
        playerCompleteQuestList.Clear();
        for (int i = 0; i < questDatas.Count; i++)
        {
            playerCompleteQuestList.Add(questDatas[i]);
        }
        Local_UpdateCurQuest();
    }
    /// <summary>
    /// 更新当前任务
    /// </summary>
    public void Local_UpdateCurQuest()
    {
        List<QuestConfig> temp = QuestConfigData.questConfigs.FindAll((x) => { return x.QuestLevel == Local_GetQuestLevel(); });
        if (!Local_GetRandomQuest(temp) && Local_GetQuestLevel() < 9)
        {
            Local_UpdateQuestLevel((short)(Local_GetQuestLevel() + 1));
            Local_UpdateCurQuest();
        }
    }
    /// <summary>
    /// 获得随机任务
    /// </summary>
    /// <param name="temp"></param>
    /// <returns></returns>
    private bool Local_GetRandomQuest(List<QuestConfig> temp)
    {
        bool success = false;
        List<QuestConfig> random = new List<QuestConfig>();
        for (int i = 0; i < temp.Count; i++)
        {
            if (!Local_GetQuestList().Contains(temp[i].QuestID))
            {
                random.Add(temp[i]);
                success = true;
            }
        }
        if (success)
        {
            QuestConfig quest = random[new System.Random().Next(0, random.Count)];
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateQuest()
            {
                Quest = quest
            });
        }
        return success;
    }

    #endregion
    #endregion
    #region//玩家信息操作
    [HideInInspector]
    public PlayerData playerData_Local;
    
    /// <summary>
    /// 设置角色信息
    /// </summary>
    public void Local_SetActorData()
    {
        GameDataManager.Instance.LoadPlayer(out playerData_Local);
        if (playerData_Local != null)
        {
            Debug.Log("--玩家信息获取成功--" + playerCoreNet.Object.InputAuthority);
            for (int i = 0; i < actorManager_Bind.actorNetManager.Local_BagCapacity; i++)
            {
                if(i < playerData_Local.BagItems.Count)
                {
                    actorManager_Bind.actorNetManager.Local_ItemBag_Change(i, playerData_Local.BagItems[i]);
                }
                else
                {
                    actorManager_Bind.actorNetManager.Local_ItemBag_Change(i, new ItemData());
                }
            }
            Local_SetLevelAndExp(playerData_Local.Level_Cur, playerData_Local.Exp_Cur);
            Local_UpdateQuestList(playerData_Local.Quest_List);
            Local_UpdateQuestLevel(playerData_Local.Quest_Level);
            Local_SetSkillList(playerData_Local.Skills_List);
            actorManager_Bind.actorNetManager.Local_SetBuffList(playerData_Local.Buff_List);
            //Debug.Log("--初始化玩家手部");
            actorManager_Bind.actorNetManager.Local_ItemHand_Add(playerData_Local.HandItem);
            //Debug.Log("--初始化玩家头部");
            actorManager_Bind.actorNetManager.Local_ItemHead_Add(playerData_Local.HeadItem);
            //Debug.Log("--初始化玩家身体");
            actorManager_Bind.actorNetManager.Local_ItemBody_Add(playerData_Local.BodyItem);
            //Debug.Log("--初始化玩家身体");
            actorManager_Bind.actorNetManager.Local_ItemAccessory_Add(playerData_Local.ItemAccessory);
            //Debug.Log("--初始化玩家耗材");
            actorManager_Bind.actorNetManager.Local_ItemConsumables_Add(playerData_Local.ItemConsumables);
            //Debug.Log("--初始化玩家数据");
            actorManager_Bind.actorNetManager.RPC_LocalInput_InitPlayerCommonData(Local_CreatePlayerNetData(playerData_Local), playerData_Local.Name);
            Debug.Log("--初始化玩家位置");
            actorManager_Bind.actorNetManager.RPC_Local_SetNetworkTransform(Vector3Int.zero);
            Local_UpdateMapInView(Vector3Int.zero);
        }
        else
        {
            Debug.Log("--玩家信息获取失败--");
        }
    }
    /// <summary>
    /// 保存人物数据
    /// </summary>
    public void Local_SaveActorData()
    {
        playerData_Local.HandItem = actorManager_Bind.actorNetManager.Local_ItemHand;
        playerData_Local.HeadItem = actorManager_Bind.actorNetManager.Local_ItemHead;
        playerData_Local.BodyItem = actorManager_Bind.actorNetManager.Local_ItemBody;
        playerData_Local.ItemAccessory = actorManager_Bind.actorNetManager.Local_ItemAccessory;
        playerData_Local.ItemConsumables = actorManager_Bind.actorNetManager.Local_ItemConsumables;
        playerData_Local.BagItems = actorManager_Bind.actorNetManager.Local_ItemBag_Get();

        playerData_Local.Hp_Cur = actorManager_Bind.actorNetManager.Net_HpCur;
        playerData_Local.Hp_Max = actorManager_Bind.actorNetManager.Local_HpMax;
        playerData_Local.Food_Cur = actorManager_Bind.actorNetManager.Net_FoodCur;
        playerData_Local.Food_Max = actorManager_Bind.actorNetManager.Local_FoodMax;
        playerData_Local.San_Cur = actorManager_Bind.actorNetManager.Net_SanCur;
        playerData_Local.San_Max = actorManager_Bind.actorNetManager.Local_SanMax;
        playerData_Local.Armor_Cur = 0;
        playerData_Local.Resistance_Cur = 0;
        playerData_Local.Coin_Cur = actorManager_Bind.actorNetManager.Local_Coin;
        playerData_Local.Fine_Cur = actorManager_Bind.actorNetManager.Local_Fine;

        playerData_Local.Level_Cur = Local_GetLevel();
        playerData_Local.Exp_Cur = Local_GetExp();
        playerData_Local.Skills_List = Local_GetSkillList();
        playerData_Local.Quest_List = Local_GetQuestList();
        playerData_Local.Quest_Level = Local_GetQuestLevel();

        playerData_Local.Hair_ID = actorManager_Bind.actorNetManager.Local_HairID;
        playerData_Local.Hair_Color = actorManager_Bind.actorNetManager.Local_HairColor;
        playerData_Local.Eye_ID = actorManager_Bind.actorNetManager.Local_EyeID;
        playerData_Local.Buff_List = actorManager_Bind.actorNetManager.Local_GetBuffList();
        GameDataManager.Instance.SavePlayer(playerData_Local);
    }
    /// <summary>
    /// 重置人物数据
    /// </summary>
    public void Local_ResetActorData()
    {
        playerData_Local.Hp_Cur = (short)(playerData_Local.Hp_Max / 2);
        playerData_Local.Food_Cur = (short)(playerData_Local.Food_Max / 2);
        playerData_Local.San_Cur = (short)(playerData_Local.San_Max / 2);
        playerData_Local.Fine_Cur = 0;
        playerData_Local.Buff_List.Clear();

        GameDataManager.Instance.SavePlayer(playerData_Local);
    }
    /// <summary>
    /// 创建人物网络数据
    /// </summary>
    /// <param name="playerData"></param>
    /// <returns></returns>
    private PlayerNetData Local_CreatePlayerNetData(PlayerData playerData)
    {
        PlayerNetData playerNetData = new PlayerNetData();

        playerNetData.Eye_ID = playerData.Eye_ID;
        playerNetData.Hair_ID = playerData.Hair_ID;
        playerNetData.Hair_Color = playerData.Hair_Color;

        playerNetData.Speed_Common = playerData.Speed_Common;
        playerNetData.Hp_Cur = playerData.Hp_Cur;
        playerNetData.Hp_Max = playerData.Hp_Max;
        playerNetData.Food_Cur = playerData.Food_Cur;
        playerNetData.Food_Max = playerData.Food_Max;
        playerNetData.San_Cur = playerData.San_Cur;
        playerNetData.San_Max = playerData.San_Max;
        playerNetData.Armor_Cur = playerData.Armor_Cur;
        playerNetData.Resistance_Cur = playerData.Resistance_Cur;
        playerNetData.Coin_Cur = playerData.Coin_Cur;

        return playerNetData;
    }

    #endregion
    #region//玩家输入
    /// <summary>
    /// 切换索引
    /// </summary>
    private int temp_SwitchIndex = 0;
    /// <summary>
    /// 切换索引归零倒计时
    /// </summary>
    private float temp_SwitchTimer = 0;
    private void Local_PlayerInputKey()
    {
        if (!actorManager_Bind) return;
        if (temp_SwitchIndex > 0 && temp_SwitchTimer > 0)
        {
            temp_SwitchTimer -= Time.deltaTime;
            if (temp_SwitchTimer <= 0) { temp_SwitchIndex = 0; temp_SwitchTimer = 0; }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            actorManager_Bind.inputManager.InputNumKeycode(temp_SwitchIndex);
            temp_SwitchIndex += 1;
            if (temp_SwitchIndex > 9) temp_SwitchIndex = 0;
            temp_SwitchTimer = 1;
            actorManager_Bind.inputManager.InputNumKeycode(temp_SwitchIndex);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            actorManager_Bind.inputManager.InputNumKeycode(temp_SwitchIndex);
            temp_SwitchIndex -= 1;
            if (temp_SwitchIndex < 0) temp_SwitchIndex = 9;
            temp_SwitchTimer = 1;
            actorManager_Bind.inputManager.InputNumKeycode(temp_SwitchIndex);
        }
        if (!Input.anyKeyDown) return;
        if (Input.GetKeyDown(KeyCode.Alpha1)) actorManager_Bind.inputManager.InputNumKeycode(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) actorManager_Bind.inputManager.InputNumKeycode(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) actorManager_Bind.inputManager.InputNumKeycode(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) actorManager_Bind.inputManager.InputNumKeycode(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) actorManager_Bind.inputManager.InputNumKeycode(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) actorManager_Bind.inputManager.InputNumKeycode(5);
        if (Input.GetKeyDown(KeyCode.Alpha7)) actorManager_Bind.inputManager.InputNumKeycode(6);
        if (Input.GetKeyDown(KeyCode.Alpha8)) actorManager_Bind.inputManager.InputNumKeycode(7);
        if (Input.GetKeyDown(KeyCode.Alpha9)) actorManager_Bind.inputManager.InputNumKeycode(8);
        if (Input.GetKeyDown(KeyCode.Alpha0)) actorManager_Bind.inputManager.InputNumKeycode(9);

        if (Input.GetKeyDown(KeyCode.F)) actorManager_Bind.inputManager.InputKeycode(KeyCode.F);
        if (Input.GetKeyDown(KeyCode.R)) actorManager_Bind.inputManager.InputKeycode(KeyCode.R);
        if (Input.GetKeyDown(KeyCode.L)) actorManager_Bind.inputManager.InputKeycode(KeyCode.L);
        if (Input.GetKeyDown(KeyCode.Space)) actorManager_Bind.inputManager.InputKeycode(KeyCode.Space);
        if (Input.GetKeyDown(KeyCode.Escape)) actorManager_Bind.inputManager.InputKeycode(KeyCode.Escape);
    }

    #endregion
    #region//地图绘制
    /// <summary>
    /// 附近地块
    /// </summary>
    private List<BuildingTile> buildingTiles_Nearby = new List<BuildingTile>();
    /// <summary>
    /// 高亮地块
    /// </summary>
    private BuildingTile buildingTiles_HighLight = null;
    /// <summary>
    /// 最近地块
    /// </summary>
    private BuildingTile buildingTiles_Nearest = null;
    /// <summary>
    /// 最近距离
    /// </summary>
    private float distance_Nearset;
    /// <summary>
    /// 当前地图绘制中心
    /// </summary>
    private NetPos vector3Int_MapCenter = new NetPos(short.MinValue, short.MinValue);
    /// <summary>
    /// 地图视野范围
    /// </summary>
    private const float config_MapView = 10;
    /// <summary>
    /// 更新地图绘制
    /// </summary>
    public void Local_UpdateMapInView(Vector3Int pos)
    {
        GameUI_MiniMap.Instance.ChangePlayerPos((Vector2Int)pos);
        ShadowManager.Instance.UpdateCenter((Vector2Int)pos);
        MapManager.Instance.CheckPlayerPosInMapGrid(pos);
    }
    /// <summary>
    /// 更新附近的地块
    /// </summary>
    private void Local_UpdateNearbyTile(Vector3Int pos)
    {
        List<BuildingTile> newNearbyTiles = MapManager.Instance.GetNearbyBuildings(pos, Vector3Int.zero,DirectionType.Eight);

        // 移除无效引用并通知远处的瓦片
        for (int i = buildingTiles_Nearby.Count - 1; i >= 0; i--)
        {
            BuildingTile tile = buildingTiles_Nearby[i];
            if (tile == null)
            {
                buildingTiles_Nearby.RemoveAt(i);
                continue;
            }

            // 如果这个瓦片不在新的附近列表中，通知它变远
            if (!newNearbyTiles.Contains(tile))
            {
                tile.FarawayTileByPlayer(this);
                buildingTiles_Nearby.RemoveAt(i);
            }
        }
        // 通知新进入范围的瓦片
        foreach (BuildingTile tile in newNearbyTiles)
        {
            if (tile != null && !buildingTiles_Nearby.Contains(tile))
            {
                tile.NearbyTileByPlayer(this);
                if (tile.CanHighlight())
                {
                    buildingTiles_Nearby.Add(tile);
                }
            }
        }

        //for (int i = 0; i < buildingTiles_Nearby.Count; i++)
        //{
        //    if (buildingTiles_Nearby[i] != null)
        //    {
        //        buildingTiles_Nearby[i].FarawayTileByPlayer(this);
        //    }
        //}
        //for (int i = 0; i < temp.Count; i++)
        //{
        //    if(temp[i] != null)
        //    {
        //        temp[i].NearbyTileByPlayer(this);
        //    }
        //}
        //buildingTiles_Nearby.Clear();
        //for (int i = 0; i < temp.Count; i++)
        //{
        //    if (temp[i] != null && temp[i].CanHighlight())
        //    {
        //        buildingTiles_Nearby.Add(temp[i]);
        //    }
        //}
    }
    /// <summary>
    /// 获取最近地块
    /// </summary>
    private void Local_FindNearestNearbyTile()
    {
        distance_Nearset = float.MaxValue;
        buildingTiles_Nearest = null;
        for (int i = 0; i < buildingTiles_Nearby.Count; i++)
        {
            Vector3 myPos = actorManager_Bind.transform.position + (Vector3)playerCoreNet.Net_MouseLocation * 0.75f;
            Vector3 tilePos = buildingTiles_Nearby[i].tilePos + new Vector3(0.5f, 0.5f, 0);
            float val = Vector3.Distance(myPos, tilePos);
            if (val < distance_Nearset)
            {
                distance_Nearset = val;
                buildingTiles_Nearest = buildingTiles_Nearby[i];
            }
        }
        if (buildingTiles_HighLight != buildingTiles_Nearest)
        {
            Local_HighLightTile(buildingTiles_Nearest);
        }
    }
    /// <summary>
    /// 高亮地块
    /// </summary>
    /// <param name="tile"></param>
    public void Local_HighLightTile(BuildingTile tile)
    {
        if (buildingTiles_HighLight != null)
        {
            buildingTiles_HighLight.HighlightTileByPlayer(false);
            actorManager_Bind.inputManager.Local_RemoveInputKeycodeAction(buildingTiles_HighLight.Local_ActorInputKeycode);
        }
        if (tile != null && (int)actorManager_Bind.bodyController.bodyAction_Cur.bodyActionType < 3)
        {
            tile.HighlightTileByPlayer(true);
            actorManager_Bind.inputManager.Local_AddInputKeycodeAction(tile.Local_ActorInputKeycode);
        }
        buildingTiles_HighLight = tile;
    }
    #endregion
}
/// <summary>
/// 玩家网络数据
/// </summary>
public struct PlayerNetData : INetworkStruct
{
    public short Hp_Cur;
    public short Hp_Max;
    public short Food_Cur;
    public short Food_Max;
    public short San_Cur;
    public short San_Max;
    public short Armor_Cur;
    public short Resistance_Cur;
    public short Speed_Common;

    public int Coin_Cur;
    public int Fine_Cur;

    public short Hair_ID;
    public Color32 Hair_Color;
    public short Eye_ID;
}
