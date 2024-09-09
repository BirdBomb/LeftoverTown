using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffConfigData : MonoBehaviour
{
    public static BuffConfig GetBuffConfig(int ID)
    {
        return buffConfigs.Find((x) => { return x.Buff_ID == ID; });
    }
    public readonly static List<BuffConfig> buffConfigs = new List<BuffConfig>()
    {
        new BuffConfig(){Buff_ID=9000,Buff_Cost=1,Buff_Name="身强力壮",Buff_Desc="力量+1"},
        new BuffConfig(){Buff_ID=9001,Buff_Cost=1,Buff_Name="大块头",Buff_Desc="力量+2，敏捷-1"},
        new BuffConfig(){Buff_ID=9002,Buff_Cost=1,Buff_Name="全神贯注",Buff_Desc="专注+1"},
        new BuffConfig(){Buff_ID=9003,Buff_Cost=1,Buff_Name="机灵鬼",Buff_Desc="智力+2，专注-1"},
        new BuffConfig(){Buff_ID=9004,Buff_Cost=1,Buff_Name="柔软身体",Buff_Desc="敏捷+1"},
        new BuffConfig(){Buff_ID=9005,Buff_Cost=2,Buff_Name="低代谢",Buff_Desc="饥饿度下降缓慢"},
        new BuffConfig(){Buff_ID=9006,Buff_Cost=2,Buff_Name="乐天派",Buff_Desc="心情度下降缓慢"},
        new BuffConfig(){Buff_ID=9007,Buff_Cost=2,Buff_Name="皮糙肉厚",Buff_Desc="初始护甲+1"},
        new BuffConfig(){Buff_ID=9008,Buff_Cost=3,Buff_Name="美食品鉴家",Buff_Desc="食物提供的心情变化翻倍"},
        new BuffConfig(){Buff_ID=9009,Buff_Cost=3,Buff_Name="天生战士",Buff_Desc="击杀回复心情值"},
        new BuffConfig(){Buff_ID=9010,Buff_Cost=4,Buff_Name="歌唱家",Buff_Desc="可以唱歌"},
        new BuffConfig(){Buff_ID=9011,Buff_Cost=4,Buff_Name="大众脸",Buff_Desc="恶名值积累更慢"},
        new BuffConfig(){Buff_ID=9012,Buff_Cost=3,Buff_Name="我是公民",Buff_Desc="你是一名合法公民"},
        new BuffConfig(){Buff_ID=9013,Buff_Cost=4,Buff_Name="我是治安官",Buff_Desc="你是一名现役治安官"},
        new BuffConfig(){Buff_ID=9014,Buff_Cost=6,Buff_Name="我是贵族",Buff_Desc="你是一名被承认的贵族"},
        new BuffConfig(){Buff_ID=9015,Buff_Cost=6,Buff_Name="弃徒",Buff_Desc="你曾经是某个大师的徒弟,自带一些随机技能"},
        new BuffConfig(){Buff_ID=9016,Buff_Cost=6,Buff_Name="家境殷实",Buff_Desc="你银行账户内有一些存款"},
        new BuffConfig(){Buff_ID=9017,Buff_Cost=6,Buff_Name="资深开锁大师",Buff_Desc="你开锁成功概率上升"},
        new BuffConfig(){Buff_ID=9018,Buff_Cost=2,Buff_Name="碳酸水爱好者",Buff_Desc="你食用碳酸饮料获得额外心情值和额外增益"},
        new BuffConfig(){Buff_ID=9019,Buff_Cost= 1,Buff_Name="购物狂",Buff_Desc="购买贵重商品时心情值上升"},

        new BuffConfig(){Buff_ID=8000,Buff_Cost=-1,Buff_Name="体弱无力",Buff_Desc="力量-1"},
        new BuffConfig(){Buff_ID=8001,Buff_Cost=-1,Buff_Name="小个子",Buff_Desc="力量-2，敏捷+1"},
        new BuffConfig(){Buff_ID=8002,Buff_Cost=-1,Buff_Name="发散思维",Buff_Desc="专注-2，智力+1"},
        new BuffConfig(){Buff_ID=8003,Buff_Cost=-1,Buff_Name="笨手笨脚",Buff_Desc="敏捷-2"},
        new BuffConfig(){Buff_ID=8004,Buff_Cost=-2,Buff_Name="高代谢",Buff_Desc="饥饿度下降变快"},
        new BuffConfig(){Buff_ID=8005,Buff_Cost=-2,Buff_Name="抑郁症",Buff_Desc="心情度下降变快"},
        new BuffConfig(){Buff_ID=8006,Buff_Cost=-2,Buff_Name="挑食",Buff_Desc="食物提供的心情减益翻倍"},
        new BuffConfig(){Buff_ID=8007,Buff_Cost=-2,Buff_Name="味觉失灵",Buff_Desc="食物提供的心情增益减半"},
        new BuffConfig(){Buff_ID=8008,Buff_Cost=-2,Buff_Name="悲天悯人",Buff_Desc="击杀心情值下降"},
        new BuffConfig(){Buff_ID=8009,Buff_Cost=-3,Buff_Name="喷嚏王",Buff_Desc="偶尔打喷嚏"},
        new BuffConfig(){Buff_ID=8010,Buff_Cost=-3,Buff_Name="不像好人",Buff_Desc="恶名值积累更快"},
        new BuffConfig(){Buff_ID=8011,Buff_Cost=-4,Buff_Name="通缉犯",Buff_Desc="你是一名通缉犯"},
        new BuffConfig(){Buff_ID=8012,Buff_Cost=-4,Buff_Name="小笨嘴",Buff_Desc="一半概率发送错误表情轮盘"},
        new BuffConfig(){Buff_ID=8013,Buff_Cost=-6,Buff_Name="负债累累",Buff_Desc="你银行账户内有一些欠款"},
        new BuffConfig(){Buff_ID=8014,Buff_Cost= -3,Buff_Name="铁公鸡",Buff_Desc="购买贵重商品时心情值下降"},
  };
}
public struct BuffConfig
{
    public short Buff_ID;
    public string Buff_Name;
    public string Buff_Desc;
    public int Buff_Cost;
}
