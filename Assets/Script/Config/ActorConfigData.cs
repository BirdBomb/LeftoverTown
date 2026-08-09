using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActorConfigData 
{
    public static ActorConfig GetActorConfig(int ID)
    {
        return actorConfigs.Find((x) => { return x.ID == ID; });
    }
    public readonly static List<ActorConfig> actorConfigs = new List<ActorConfig>()
    {
        /*假人*/
        new ActorConfig(0),
        #region//市民
        /*村民*/
        new ActorConfig(100,500,30,0,0,StatusType.Human_Common,200,
            new LootRandomInfo[]{new LootRandomInfo(0,1,1,500)},
            new LootRandomInfo[]{new LootRandomInfo(0,1,1,500)},
            null,
            null,0,
            null,
            new LootRandomInfo[]{new LootRandomInfo(9901, 20,100,500)},1
            ),
        /*矿石商人*/
        new ActorConfig(101,500,30,0,0,StatusType.Human_Common,200,
            new LootRandomInfo[]{ new LootRandomInfo(0,1,1,500)},
            new LootRandomInfo[]{ new LootRandomInfo(0,1,1,500)},
            null,
            new LootRandomInfo[]{ new LootRandomInfo(1011,5,5,800), new LootRandomInfo(1012,5,5,800),new LootRandomInfo(1013,5,5,800),new LootRandomInfo(1014,5,5,800) },4,
            null,
            new LootRandomInfo[]{ new LootRandomInfo(9901,20,100,500)},1
            ),

        /*镇长*/
        new ActorConfig(102,500,30,0,0,StatusType.Human_Common,200,
            new LootRandomInfo[]{ new LootRandomInfo(0,1,1,500)},
            new LootRandomInfo[]{ new LootRandomInfo(5701,1,1,500)},
            null,
            new LootRandomInfo[]{ new LootRandomInfo(9912,1,1,1000)},1,
            null,
            new LootRandomInfo[]{ new LootRandomInfo(9901,20,100,500)},1
            ),
        /*伐木工*/
        new ActorConfig(103,500,30,0,0,StatusType.Human_Common,200,
            new LootRandomInfo[]{ new LootRandomInfo(0,1,1,500)},
            new LootRandomInfo[]{ new LootRandomInfo(5708,1,1,500)},
            new LootFixedInfo[]{ new LootFixedInfo(2012,1,1) },
            new LootRandomInfo[]{ new LootRandomInfo(1000,5,5,800),new LootRandomInfo(1001,5,5,800),new LootRandomInfo(1100,5,5,800) },1,
            null,
            new LootRandomInfo[]{ new LootRandomInfo(9901,20,100,500)},1
            ),
        /*猎人*/
        new ActorConfig(104,500,30,0,0,StatusType.Human_Common,200,
            new LootRandomInfo[]{ new LootRandomInfo(5207,1,1,500)},
            new LootRandomInfo[]{ new LootRandomInfo(0,1,1,500)},
            new LootFixedInfo[]{ new LootFixedInfo(2201, 1,1) },
            new LootRandomInfo[]{ new LootRandomInfo(9000,5,5,800),new LootRandomInfo(9001,5,5,800),new LootRandomInfo(1022,5,5,800) },1,
            null,
            new LootRandomInfo[]{ new LootRandomInfo(9901,20,100,500)},1
            ),
        /*护卫*/
        new ActorConfig(105,500,30,0,0,StatusType.Human_Common,200,
            new LootRandomInfo[]{ new LootRandomInfo(5002,1,1,500)},
            new LootRandomInfo[]{ new LootRandomInfo(5402,1,1,500)},
            new LootFixedInfo[]{ new LootFixedInfo(2303, 1,1) },
            null,0,
            new LootFixedInfo[]{ new LootFixedInfo(9010,5,20) },
            null,0
            ),
        /*农夫*/
        new ActorConfig(106,500,30,0,0,StatusType.Human_Common,200,
            new LootRandomInfo[]{ new LootRandomInfo(5200,1,1,500)},
            new LootRandomInfo[]{ new LootRandomInfo(5700,1,1,500)},
            null,
            new LootRandomInfo[]{ new LootRandomInfo(6100,5,5,800),new LootRandomInfo(6101,5,5,800),new LootRandomInfo(6102,5,5,800) },4,
            null,
            new LootRandomInfo[]{ new LootRandomInfo(6100,5,5,800),new LootRandomInfo(6101,5,5,800),new LootRandomInfo(6102,5,5,800) },1
            ),

        /*厨师*/
        new ActorConfig(107,500,30,0,0,StatusType.Human_Common,200,
            new LootRandomInfo[]{ new LootRandomInfo(5206,1,1,500)},
            new LootRandomInfo[]{ new LootRandomInfo(5707,1,1,500)},
            null,
            new LootRandomInfo[]{ new LootRandomInfo(4001,1,1,800), new LootRandomInfo(4002,1,1,800), new LootRandomInfo(4008,1,1,800) },4,
            null,
            new LootRandomInfo[]{ new LootRandomInfo(9901,20,100,500) },1
            ),
        /*酒保*/
        new ActorConfig(108,500,30,0,0,StatusType.Human_Common,200,
            new LootRandomInfo[]{ new LootRandomInfo(0,1,1,500)},
            new LootRandomInfo[]{ new LootRandomInfo(5706,1,1,500)},
            null,
            new LootRandomInfo[]{ new LootRandomInfo(4300,1,1,800), new LootRandomInfo(4300,1,1,800), new LootRandomInfo(4300,1,1,800) },4,
            null,
            new LootRandomInfo[]{ new LootRandomInfo(9901,20,100,500) },1
            ),
        /*银行家*/
        new ActorConfig(109,500,30,0,0,StatusType.Human_Common,1000,
            new LootRandomInfo[]{ new LootRandomInfo(0,1,1,500)},
            new LootRandomInfo[]{ new LootRandomInfo(5701,1,1,500)},
            null,
            new LootRandomInfo[]{ new LootRandomInfo(9910,1,1,1000),new LootRandomInfo(9911,1,1, 1000) },3,
            null,
            new LootRandomInfo[]{ new LootRandomInfo(9901,20,100,500) },1
            ),
        /*土匪*/
        new ActorConfig(110,500,30,0,0,StatusType.Human_Common,1000,
            new LootRandomInfo[]{ new LootRandomInfo(5202,1,1,500)},
            new LootRandomInfo[]{ new LootRandomInfo(5402,1,1,500)},
            new LootFixedInfo[]{ new LootFixedInfo(2304,1,1) },
            null,0,
            new LootFixedInfo[]{ new LootFixedInfo(9010,5,20) },
            new LootRandomInfo[]{ new LootRandomInfo(9901,20,100,500) },1
            ),
        #endregion
        #region//动物
        /*野兔*/
        new ActorConfig(200,500,40,0,0,StatusType.Animal_Common,0,
            null,
            null,
            null,
            null,0,
            new LootFixedInfo[]{ new LootFixedInfo(3101,1,1) },
            null,0
            ),
        /*野鸡*/
        new ActorConfig(201,500,40,0,0,StatusType.Animal_Common,0,
            null,
            null,
            null,
            null,0,
            new LootFixedInfo[]{ new LootFixedInfo(3102,1,1) },
            null,0
            ),
        /*牛*/
        new ActorConfig(202,500,20,0,0,StatusType.Animal_Common,0,
            null,
            null,
            null,
            null,0,
            new LootFixedInfo[]{ new LootFixedInfo(3100,1,1) },
            null,0
            ),
        /*马*/
        new ActorConfig(203,500,50,0,0,StatusType.Animal_Common,0,
            null,
            null,
            null,
            null,0,
            new LootFixedInfo[]{ new LootFixedInfo(3100,1,1) },
            null,0
            ),
        /*狼*/
        new ActorConfig(204,500,40,0,0,StatusType.Monster_Common,0,
            null,
            null,
            null,
            null,0,
            new LootFixedInfo[]{ new LootFixedInfo(3100,1,1) },
            null,0
            ),
        #endregion
        #region//怪物
        /*史莱姆*/
        new ActorConfig(){ID = 300,
            Bag_List = new LootFixedInfo[]{},
            Goods_List = new LootRandomInfo[]{},
            LootFixed_List = new LootFixedInfo[]{ new LootFixedInfo(3101,1,1) },
            LootRandom_List = new LootRandomInfo[]{},
        },
        #endregion
        #region//僵尸
        /*普通僵尸*/
        new ActorConfig(400,1000,35,0,0,StatusType.Monster_Common,0,
            null,
            null,
            null,
            null,0,
            new LootFixedInfo[]{ new LootFixedInfo(3104,1,1) },
            null,0
            ),
        /*奔跑僵尸*/
        new ActorConfig(401,1500,45,0,0,StatusType.Monster_Common,0,
            null,
            null,
            null,
            null,0,
            new LootFixedInfo[]{ new LootFixedInfo(3104,2,2) },
            null,0
            ),
        /*喷射僵尸*/
        new ActorConfig(402,2000,40,0,0,StatusType.Monster_Common,0,
            null,
            null,
            null,
            null,0,
            new LootFixedInfo[]{ new LootFixedInfo(3104,2,2) },
            null,0
            ),
        #endregion
        #region//机械
        /*投掷机器人*/
        new ActorConfig(500,5000,35,0,0,StatusType.Monster_Common,0,
            null,
            null,
            null,
            null,0,
            new LootFixedInfo[]{ new LootFixedInfo(1113,2,4),new LootFixedInfo(1114,4,6),new LootFixedInfo(1200,1,4) },
            null,0
            ),
        /*喷火机器人*/
        new ActorConfig(501,5000,35,0,0,StatusType.Monster_Common,0,
            null,
            null,
            null,
            null,0,
            new LootFixedInfo[]{ new LootFixedInfo(1113,2,4),new LootFixedInfo(1114,4,6),new LootFixedInfo(1200,1,4) },
            null,0
            ),
        #endregion
        #region//载具
        /*木筏*/
        new ActorConfig(600,500,20,0,0,StatusType.Animal_Common,0,
            null,
            null,
            null,
            null,0,
            new LootFixedInfo[]{ new LootFixedInfo(3100,1,1) },
            null,0
            ),
        #endregion
        #region//特殊人物
        /*背大包的商人*/
        new ActorConfig(700,1000,0,99,99,StatusType.Default,200,
            null,
            null,
            null,
            new LootRandomInfo[]
            {   
                new LootRandomInfo(1000,5,5,800), 
                new LootRandomInfo(1010,5,5,800),
                new LootRandomInfo(6000,1,1,1000),
            }
            ,6,
            null,
            null,0
            ),
        #endregion
        #region//Boss
        /*铁钩僵尸*/
        new ActorConfig(900,10000,30,0,0,StatusType.Monster_Common,0,
            null,
            null,
            null,
            null,0,
            new LootFixedInfo[]{ new LootFixedInfo(3104,8,12) },
            null,0
            ),
        #endregion
    };
}
public struct ActorConfig 
{
    public int ID;
    public short Hp;
    public short Speed;
    public short Armor;
    public short Resistance;
    public StatusType Status;
    public int Coin;
    /// <summary>
    /// 帽子列表
    /// </summary>
    public LootRandomInfo[] Hat_List;
    /// <summary>
    /// 衣服列表
    /// </summary>
    public LootRandomInfo[] Clothes_List;
    /// <summary>
    /// 背包列表
    /// </summary>
    public LootFixedInfo[] Bag_List;
    /// <summary>
    /// 商品列表
    /// </summary>
    public LootRandomInfo[] Goods_List;
    /// <summary>
    /// 商品数量
    /// </summary>
    public int Goods_Count;
    /// <summary>
    /// 固定掉落物
    /// </summary>
    public LootFixedInfo[] LootFixed_List;
    /// <summary>
    /// 掉落物列表
    /// </summary>
    public LootRandomInfo[] LootRandom_List;
    /// <summary>
    /// 掉落物数量
    /// </summary>
    public int LootRandom_Count;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="hp">HP</param>
    /// <param name="speed"></param>
    /// <param name="armor"></param>
    /// <param name="resistance"></param>
    /// <param name="status"></param>
    /// <param name="coin"></param>
    /// <param name="hatList">帽子</param>
    /// <param name="clothesList">衣服</param>
    /// <param name="bagList"></param>
    /// <param name="goodList"></param>
    /// <param name="goodCount"></param>
    /// <param name="lootFixed"></param>
    /// <param name="lootRandomList"></param>
    /// <param name="lootRandomCount"></param>
    public ActorConfig(int id, short hp = 1000, short speed = 0, short armor = 0, short resistance = 0, StatusType status = StatusType.Default,int coin = 0,
        LootRandomInfo[] hatList = null, LootRandomInfo[] clothesList = null, LootFixedInfo[] bagList = null,
        LootRandomInfo[] goodList = null, int goodCount = 0, LootFixedInfo[] lootFixed = null, LootRandomInfo[] lootRandomList = null, int lootRandomCount = 0)
    {
        ID = id;
        Hp = hp;
        Speed = speed;
        Armor = armor;
        Resistance = resistance;
        Status = status;
        Coin = coin;
        Hat_List = hatList;
        Clothes_List = clothesList;
        Bag_List = bagList;
        Goods_List = goodList;
        Goods_Count = goodCount;
        LootFixed_List = lootFixed;
        LootRandom_List = lootRandomList;
        LootRandom_Count = lootRandomCount;
    }
}
