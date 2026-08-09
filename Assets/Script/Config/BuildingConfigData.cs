using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingConfigData : MonoBehaviour
{
    public static BuildingConfig GetBuildingConfig(int ID)
    {
        return buildConfigs.Find((x) => { return x.Building_ID == ID; });
    }
    public readonly static List<BuildingConfig> buildConfigs = new List<BuildingConfig>()
    {
        /*空*/new BuildingConfig()
        {
            Building_ID = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*占位*/new BuildingConfig()
        {
            Building_ID = 99,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        #region//自然树木1000

        new BuildingConfig()//森林青树
        {
            Building_ID = 1000,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//森林伞树
        {
            Building_ID = 1001,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//雪原树
        {
            Building_ID = 1002,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//沙漠面包树
        {
            Building_ID = 1003,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//矮脚树
        {
            Building_ID = 1004,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//水生树
        {
            Building_ID = 1005,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        #endregion
        #region//自然矿石1100
        /*岩石*/new BuildingConfig()
        {
            Building_ID = 1110,Building_Hp = 200,Building_Armor = 20,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*岩石(煤)*/new BuildingConfig()
        {
            Building_ID = 1111,Building_Hp = 200,Building_Armor = 20,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*岩石(硝)*/new BuildingConfig()
        {
            Building_ID = 1112,Building_Hp = 200,Building_Armor = 20,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*岩石(铜)*/new BuildingConfig()
        {
            Building_ID = 1113,Building_Hp = 200,Building_Armor = 20,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*岩石(铁)*/new BuildingConfig()
        {
            Building_ID = 1114,Building_Hp = 200,Building_Armor = 20,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*岩石(金)*/new BuildingConfig()
        {
            Building_ID = 1115,Building_Hp = 200,Building_Armor = 20,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*磐石*/new BuildingConfig()
        {
            Building_ID = 1120,Building_Hp = 200,Building_Armor = 20,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*沼泽石*/new BuildingConfig()
        {
            Building_ID = 1121,Building_Hp = 200,Building_Armor = 20,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },

        #endregion
        #region//自然植物1200
        new BuildingConfig()//森林长草丛
        {
            Building_ID = 1200,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//柑橘灌木
        {
            Building_ID = 1201,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//仙人掌
        {
            Building_ID = 1202,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//花丛
        {
            Building_ID = 1203,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//空心树桩
        {
            Building_ID = 1204,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//菌菇丛
        {
            Building_ID = 1205,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//水生甘蔗
        {
            Building_ID = 1206,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        #endregion
        #region//自然作物1300
        /*小麦*/new BuildingConfig()
        {
            Building_ID = 1300,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*薯果*/new BuildingConfig()
        {
            Building_ID = 1301,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*辣椒*/new BuildingConfig()
        {
            Building_ID = 1302,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        #endregion
        #region//非自然建筑2000
        #region//重要地点生成
        new BuildingConfig()//太阳祭坛
        {
            Building_ID = 2000,Building_Hp = int.MaxValue,Building_Armor = int.MaxValue,Building_Size = AreaSize._3X3,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        #endregion
        #region//动物出生点生成
        new BuildingConfig()//兔子生成
        {
            Building_ID = 2100,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//山鸡生成
        {
            Building_ID = 2101,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//牛生成
        {
            Building_ID = 2102,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//马生成
        {
            Building_ID = 2103,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        #endregion
        #region//角色出生点生成
        new BuildingConfig()//向导生成
        {
            Building_ID = 2200,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//猎人生成
        {
            Building_ID = 2201,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//农民生成
        {
            Building_ID = 2202,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//矿物商人生成
        {
            Building_ID = 2203,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//伐木工生成
        {
            Building_ID = 2204,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._2X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//警卫生成
        {
            Building_ID = 2205,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._2X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//厨师生成
        {
            Building_ID = 2206,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//镇长生成
        {
            Building_ID = 2207,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//酒保生成
        {
            Building_ID = 2208,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//富豪生成
        {
            Building_ID = 2209,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//村民生成
        {
            Building_ID = 2210,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },

       #endregion
        #region//罪犯生成
        new BuildingConfig()//夜班土匪生成
        {
            Building_ID = 2300,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        #endregion
        #region//怪物出生点生成
        new BuildingConfig()//僵尸生成
        {
            Building_ID = 2400,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//鬣狗生成
        {
            Building_ID = 2401,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._2X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//史莱姆生成
        {
            Building_ID = 2402,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._2X2,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        #endregion

        #region//Boss出生点生成
        new BuildingConfig()//铁钩僵尸生成
        {
            Building_ID = 2500,Building_Hp = int.MaxValue,Building_Armor = int.MaxValue,Building_Size = AreaSize._3X3,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//废弃检查站
        {
            Building_ID = 2501,Building_Hp = int.MaxValue,Building_Armor = int.MaxValue,Building_Size = AreaSize._3X3,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        #endregion
        #region//交易点
        new BuildingConfig()//木制货架_五金店
        {
            Building_ID = 2601,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//木制货架_熟食店
        {
            Building_ID = 2602,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//木制货架_甜品店
        {
            Building_ID = 2603,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//木制货架_服装店
        {
            Building_ID = 2604,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//木制货架_录像带店
        {
            Building_ID = 2605,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//木制货架_书店
        {
            Building_ID = 2606,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },

        #endregion
        #region//其他建筑
        new BuildingConfig()//石头灯柱
        {
            Building_ID = 2900,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//岗哨
        {
            Building_ID = 2901,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//柜台
        {
            Building_ID = 2902,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//木桶
        {
            Building_ID = 2903,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//木箱废墟
        {
            Building_ID = 2904,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//装饰操作台
        {
            Building_ID = 2905,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//装饰金属架子
        {
            Building_ID = 2906,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//骷髅
        {
            Building_ID = 2907,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()//上锁的箱子
        {
            Building_ID = 2908,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },

        #endregion

        #endregion

        #region//木制建筑3000
        #region//结构体
        new BuildingConfig()//木墙
        {
            Building_ID = 3000,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,Building_Age = AgeGroup.Wood,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1000,2) },
        },
        new BuildingConfig()//木门
        {
            Building_ID = 3001,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,Building_Age = AgeGroup.Wood,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,2) },
        },
        new BuildingConfig()//木栅栏
        {
            Building_ID = 3002,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,Building_Age = AgeGroup.Wood,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,2) },
        },
        #endregion
        #region//家具
        new BuildingConfig()//木箱
        {
            Building_ID = 3100,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,Building_Age = AgeGroup.Wood,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4) },
        },
        new BuildingConfig()//木桌子
        {
            Building_ID = 3101,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,Building_Age = AgeGroup.Wood,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4) },
        },
        new BuildingConfig()//木床
        {
            Building_ID = 3102,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._2X1,Building_Type = BuildingType.Furniture,Building_Age = AgeGroup.Wood,
            Building_Group = new List<short>{ 10010,10011,10012 },
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4) },
        },
        new BuildingConfig()//木椅子
        {
            Building_ID = 3103,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,Building_Age = AgeGroup.Wood,
            Building_Group = new List<short>{ 10020, 10021, 10022, 10023 },
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4) },
        },
        new BuildingConfig()//木制花盆
        {
            Building_ID = 3104,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,Building_Age = AgeGroup.Wood,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4) },
        },
        new BuildingConfig()//拒马
        {
            Building_ID = 3105,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Furniture,Building_Age = AgeGroup.Wood,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4) },
        },
        new BuildingConfig()//书架
        {
            Building_ID = 3106,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Furniture,Building_Age = AgeGroup.Wood,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4) },
        },
        new BuildingConfig()//木制便器
        {
            Building_ID = 3107,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Furniture,Building_Age = AgeGroup.Wood,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4) },
        },

        #endregion
        #region//设施
        new BuildingConfig()/*木加工台*/
        {
            Building_ID = 3201,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._2X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.Wood,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1000,3) },
        },

        #endregion
        #endregion
        #region//石头建筑4000
        #region//结构体
        new BuildingConfig()//砖墙
        {
            Building_ID = 4000,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,Building_Age = AgeGroup.Stone,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1010,4) },
        },
        #endregion
        #region//家具
        new BuildingConfig()//石头灯箱
        {
            Building_ID = 4100,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.Stone,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1010,4),new ItemRaw(1011,4) },
        },
        #endregion
        #region//设施
        new BuildingConfig()//石头熔炉
        {
            Building_ID = 4200,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.Stone,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1010,4),new ItemRaw(1011,4) },
        },
        new BuildingConfig()//烹饪台
        {
            Building_ID = 4201,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.Stone,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1010,4),new ItemRaw(1011,4) },
        },
        new BuildingConfig()//石头井
        {
            Building_ID = 4202,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._2X2,Building_Type =BuildingType.Machine,Building_Age = AgeGroup.Stone,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1010,4),new ItemRaw(1011,4) },
        },

        #endregion

        #endregion
        #region//金属建筑5000
        #region//结构体
        new BuildingConfig()//金属墙
        {
            Building_ID = 5000,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,Building_Age = AgeGroup.Iron,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1114,2) },
        },
        new BuildingConfig()//铁门
        {
            Building_ID = 5001,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,Building_Age = AgeGroup.Iron,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1114,4) },
        },
        new BuildingConfig()//铁栅栏
        {
            Building_ID = 5002,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,Building_Age = AgeGroup.Iron,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1114,2) },
        },
        #endregion
        #region//家具
        new BuildingConfig()//冰箱
        {
            Building_ID = 5100,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,Building_Age = AgeGroup.Iron,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1114,4),new ItemRaw(1200,2) },
        },
        new BuildingConfig()//日光灯
        {
            Building_ID = 5101,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Furniture,Building_Age = AgeGroup.Iron,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1114,4),new ItemRaw(1200,2) },
        },
        new BuildingConfig()//烧煤电视机
        {
            Building_ID = 5102,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Furniture,Building_Age = AgeGroup.Iron,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1011,4), new ItemRaw(1114, 4), new ItemRaw(1201,2) },
        },

        #endregion
        #region//设施
        new BuildingConfig()//铁制加工台
        {
            Building_ID = 5200,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._2X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.Iron,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1114, 4), new ItemRaw(1200,2) },
        },
        new BuildingConfig()//枪械加工台
        {
            Building_ID = 5201,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._2X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.Iron,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1114, 4), new ItemRaw(1200, 2) },
        },
        new BuildingConfig()//粉碎机
        {
            Building_ID = 5202,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.Iron,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1114, 4), new ItemRaw(1200, 2) },
        },
        new BuildingConfig()//饮料机
        {
            Building_ID = 5203,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,Building_Age = AgeGroup.Iron,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1114, 4), new ItemRaw(1200, 2) },
        },
        new BuildingConfig()//钻井
        {
            Building_ID = 5204,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._2X2,Building_Type = BuildingType.Structure,Building_Age = AgeGroup.Iron,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1113, 20), new ItemRaw(1114, 20), new ItemRaw(1200, 5) },
        },
        #endregion
        #endregion
        #region//灵能建筑6000
        #endregion
        #region//太阳建筑7000
        #endregion
        #region//衍生建筑10000
        /*左木床*/
        new BuildingConfig()
        {
            Building_ID = 10010,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._2X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*右木床*/new BuildingConfig()
        {
            Building_ID = 10011,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._2X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*上木床*/new BuildingConfig()
        {
            Building_ID = 10012,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X2,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*下木椅*/new BuildingConfig()
        {
            Building_ID = 10020,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*左木椅*/new BuildingConfig()
        {
            Building_ID = 10021,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*右木椅*/new BuildingConfig()
        {
            Building_ID = 10022,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*上木椅*/new BuildingConfig()
        {
            Building_ID = 10023,Building_Hp = 200,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },

        #endregion
    };
}
[Serializable]
public struct BuildingConfig
{
    [SerializeField]/*建筑编号*/
    public short Building_ID;
    [SerializeField]/*建筑生命值*/
    public int Building_Hp;
    [SerializeField]/*建筑护甲*/
    public int Building_Armor;
    [SerializeField]/*建筑尺寸*/
    public AreaSize Building_Size;
    [SerializeField]/*建筑类别*/
    public BuildingType Building_Type;
    [SerializeField]/*建筑时代*/
    public AgeGroup Building_Age;
    [SerializeField]/*建筑原料*/
    public List<ItemRaw>Building_Raw;
    [SerializeField]/*建筑群*/
    public List<short> Building_Group;
}
/// <summary>
/// 建筑尺寸
/// </summary>
public enum AreaSize
{
    _1X1,
    _1X2,
    _2X1,
    _2X2,
    _3X3,
}
/// <summary>
/// 建筑类别
/// </summary>
public enum BuildingType
{
    /// <summary>
    /// 所有
    /// </summary>
    All,
    /// <summary>
    /// 结构
    /// </summary>
    Structure,
    /// <summary>
    /// 家具
    /// </summary>
    Furniture,
    /// <summary>
    /// 设施
    /// </summary>
    Machine,
    /// <summary>
    /// 其他
    /// </summary>
    Other,
    /// <summary>
    /// 地面
    /// </summary>
    Ground,
}
/// <summary>
/// 时代
/// </summary>
public enum AgeGroup
{
    /// <summary>
    /// 自然造物
    /// </summary>
    Nature,
    /// <summary>
    /// 非自然造物
    /// </summary>
    UnNature,
    /// <summary>
    /// 木制
    /// </summary>
    Wood, 
    /// <summary>
    /// 石头
    /// </summary>
    Stone,
    /// <summary>
    /// 铁器
    /// </summary>
    Iron,
    /// <summary>
    /// 灵能
    /// </summary>
    Magic,
    /// <summary>
    /// 太阳碎片
    /// </summary>
    SunPiece,
    /// <summary>
    /// 超魔法时代
    /// </summary>
    MagicAge_1,
}