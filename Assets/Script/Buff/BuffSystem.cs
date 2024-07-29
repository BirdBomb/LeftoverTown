using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BuffSystem 
{
}
/// <summary>
/// 身强力壮
/// </summary>
public class Buff9000:BuffBase
{
    public override void AddWhenCreatePlayer(ref PlayerData data)
    {
        data.Point_Strength += 1;
        base.AddWhenCreatePlayer(ref data);
    }
    public override void SubWhenCreatePlayer(ref PlayerData data)
    {
        data.Point_Strength -= 1;
        base.SubWhenCreatePlayer(ref data);
    }
}
/// <summary>
/// 大块头
/// </summary>
public class Buff9001 : BuffBase
{
    public override void AddWhenCreatePlayer(ref PlayerData data)
    {
        data.Point_Strength += 2;
        data.Point_Agility -= 1;
        base.AddWhenCreatePlayer(ref data);
    }
    public override void SubWhenCreatePlayer(ref PlayerData data)
    {
        data.Point_Strength -= 2;
        data.Point_Agility += 1;
        base.SubWhenCreatePlayer(ref data);
    }
}
/// <summary>
/// 全神贯注
/// </summary>
public class Buff9002 : BuffBase
{
    public override void AddWhenCreatePlayer(ref PlayerData data)
    {
        data.Point_Focus += 1;
        base.AddWhenCreatePlayer(ref data);
    }
    public override void SubWhenCreatePlayer(ref PlayerData data)
    {
        data.Point_Focus -= 1;
        base.SubWhenCreatePlayer(ref data);
    }
}
/// <summary>
/// 机灵鬼
/// </summary>
public class Buff9003 : BuffBase
{
    public override void AddWhenCreatePlayer(ref PlayerData data)
    {
        data.Point_Intelligence += 2;
        data.Point_Focus -= 1;
        base.AddWhenCreatePlayer(ref data);
    }
    public override void SubWhenCreatePlayer(ref PlayerData data)
    {
        data.Point_Intelligence -= 2;
        data.Point_Focus += 1;
        base.SubWhenCreatePlayer(ref data);
    }
}
/// <summary>
/// 柔软身体
/// </summary>
public class Buff9004 : BuffBase
{
    public override void AddWhenCreatePlayer(ref PlayerData data)
    {
        data.Point_Agility += 1;
        base.AddWhenCreatePlayer(ref data);
    }
    public override void SubWhenCreatePlayer(ref PlayerData data)
    {
        data.Point_Agility -= 1;
        base.SubWhenCreatePlayer(ref data);
    }
}
/// <summary>
/// 低代谢
/// </summary>
public class Buff9005 : BuffBase
{

}
/// <summary>
/// 乐天派
/// </summary>
public class Buff9006 : BuffBase
{

}
/// <summary>
/// 皮糙肉厚
/// </summary>
public class Buff9007 : BuffBase
{
    public override void AddWhenCreatePlayer(ref PlayerData data)
    {
        data.Armor += 1;
        base.AddWhenCreatePlayer(ref data);
    }
}
/// <summary>
/// 美食品鉴家
/// </summary>
public class Buff9008 : BuffBase
{
}
/// <summary>
/// 天生战士
/// </summary>
public class Buff9009 : BuffBase
{
}
/// <summary>
/// 歌唱家
/// </summary>
public class Buff9010 : BuffBase
{
}
/// <summary>
/// 大众脸
/// </summary>
public class Buff9011 : BuffBase
{
}
/// <summary>
/// 公民身份
/// </summary>
public class Buff9012 : BuffBase
{
}
/// <summary>
/// 治安官身份
/// </summary>
public class Buff9013 : BuffBase
{
}
/// <summary>
/// 贵族身份
/// </summary>
public class Buff9014 : BuffBase
{
}
/// <summary>
/// 弃徒
/// </summary>
public class Buff9015 : BuffBase
{
}
/// <summary>
/// 家境殷实
/// </summary>
public class Buff9016 : BuffBase
{
}
/// <summary>
/// 资深开锁大师
/// </summary>
public class Buff9017 : BuffBase
{
}
/// <summary>
/// 碳酸水爱好者
/// </summary>
public class Buff9018 : BuffBase
{
}
/// <summary>
/// 购物狂
/// </summary>
public class Buff9019 : BuffBase
{

}


/// <summary>
/// 体弱无力
/// </summary>
public class Buff8000 : BuffBase
{
    public override void AddWhenCreatePlayer(ref PlayerData data)
    {
        data.Point_Strength -= 1;
        base.AddWhenCreatePlayer(ref data);
    }
    public override void SubWhenCreatePlayer(ref PlayerData data)
    {
        data.Point_Strength += 1;
        base.SubWhenCreatePlayer(ref data);
    }
}
/// <summary>
/// 小个子
/// </summary>
public class Buff8001 : BuffBase
{
    public override void AddWhenCreatePlayer(ref PlayerData data)
    {
        data.Point_Strength -= 2;
        data.Point_Agility += 1;
        base.AddWhenCreatePlayer(ref data);
    }
    public override void SubWhenCreatePlayer(ref PlayerData data)
    {
        data.Point_Strength += 2;
        data.Point_Agility -= 1;
        base.SubWhenCreatePlayer(ref data);
    }
}
/// <summary>
/// 发散思维
/// </summary>
public class Buff8002 : BuffBase
{
    public override void AddWhenCreatePlayer(ref PlayerData data)
    {
        data.Point_Focus -= 2;
        data.Point_Intelligence += 1;
        base.AddWhenCreatePlayer(ref data);
    }
    public override void SubWhenCreatePlayer(ref PlayerData data)
    {
        data.Point_Focus += 2;
        data.Point_Intelligence -= 1;
        base.SubWhenCreatePlayer(ref data);
    }
}
/// <summary>
/// 笨手笨脚
/// </summary>
public class Buff8003 : BuffBase
{
    public override void AddWhenCreatePlayer(ref PlayerData data)
    {
        data.Point_Agility -= 2;
        base.AddWhenCreatePlayer(ref data);
    }
    public override void SubWhenCreatePlayer(ref PlayerData data)
    {
        data.Point_Agility += 2;
        base.SubWhenCreatePlayer(ref data);
    }
}
/// <summary>
/// 高代谢
/// </summary>
public class Buff8004 : BuffBase
{
}
/// <summary>
/// 抑郁症
/// </summary>
public class Buff8005 : BuffBase
{
}
/// <summary>
/// 挑食
/// </summary>
public class Buff8006 : BuffBase
{
}
/// <summary>
/// 味觉失灵
/// </summary>
public class Buff8007 : BuffBase
{
}
/// <summary>
/// 悲天悯人
/// </summary>
public class Buff8008 : BuffBase
{
}
/// <summary>
/// 喷嚏王
/// </summary>
public class Buff8009 : BuffBase
{
}
/// <summary>
/// 不像好人
/// </summary>
public class Buff8010 : BuffBase
{
}
/// <summary>
/// 通缉犯
/// </summary>
public class Buff8011 : BuffBase
{
}
/// <summary>
/// 小笨嘴
/// </summary>
public class Buff8012 : BuffBase
{
}
/// <summary>
/// 负债累累
/// </summary>
public class Buff8013 : BuffBase
{
}
/// <summary>
/// 铁公鸡
/// </summary>
public class Buff8014 : BuffBase
{
}
