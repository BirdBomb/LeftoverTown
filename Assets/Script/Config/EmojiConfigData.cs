using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmojiConfigData : MonoBehaviour
{
    public static EmojiConfig GetItemConfig(int ID)
    {
        return emojiConfigs.Find((x) => { return x.Emoji_ID == ID; });
    }
    /*
     * 1000 - 1999 材料
     * 2000 - 2999 工具
     * 3000 - 3999 食材
     * 4000 - 4999 容器
     * 5000 - 5999 衣物
     * 9000 - 9999 其他
     */
    public readonly static List<EmojiConfig> emojiConfigs = new List<EmojiConfig>()
    {
        new EmojiConfig(){ Emoji_ID = 0,Emoji_Name = "震惊" },
        new EmojiConfig(){ Emoji_ID = 1,Emoji_Name = "疑惑" },
        new EmojiConfig(){ Emoji_ID = 2,Emoji_Name = "是" },
        new EmojiConfig(){ Emoji_ID = 3,Emoji_Name = "否" },
        new EmojiConfig(){ Emoji_ID = 4,Emoji_Name = "开心" },
        new EmojiConfig(){ Emoji_ID = 5,Emoji_Name = "生气" },
        new EmojiConfig(){ Emoji_ID = 6,Emoji_Name = "惊恐" },
        new EmojiConfig(){ Emoji_ID = 7,Emoji_Name = "惊讶" },
        new EmojiConfig(){ Emoji_ID = 8,Emoji_Name = "敬礼" },
        new EmojiConfig(){ Emoji_ID = 9,Emoji_Name = "攻击" },
        new EmojiConfig(){ Emoji_ID = 10,Emoji_Name = "夜晚" },
        new EmojiConfig(){ Emoji_ID = 11,Emoji_Name = "食物" },
        new EmojiConfig(){ Emoji_ID = 12,Emoji_Name = "攻击" },
        new EmojiConfig(){ Emoji_ID = 13,Emoji_Name = "金币" },
        new EmojiConfig(){ Emoji_ID = 14,Emoji_Name = "随眠" },
        new EmojiConfig(){ Emoji_ID = 15,Emoji_Name = "省略" },
        new EmojiConfig(){ Emoji_ID = 16,Emoji_Name = "大叫" },
    };

}
public struct EmojiConfig
{
    public int Emoji_ID;
    public string Emoji_Name;
}
