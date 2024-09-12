using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmojiConfigData : MonoBehaviour
{
    public static EmojiConfig GetEmojiConfig(int ID)
    {
        return emojiConfigs.Find((x) => { return x.Emoji_ID == ID; });
    }
    public readonly static List<EmojiConfig> emojiConfigs = new List<EmojiConfig>()
    {
        new EmojiConfig(){ Emoji_ID = 0,Emoji_Name = "Õð¾ª" ,Emoji_Distance = 5 },
        new EmojiConfig(){ Emoji_ID = 1,Emoji_Name = "ÒÉ»ó" ,Emoji_Distance = 5 },
        new EmojiConfig(){ Emoji_ID = 2,Emoji_Name = "ÊÇ" ,Emoji_Distance = 5},
        new EmojiConfig(){ Emoji_ID = 3,Emoji_Name = "·ñ" ,Emoji_Distance = 5},
        new EmojiConfig(){ Emoji_ID = 4,Emoji_Name = "¿ªÐÄ" ,Emoji_Distance = 5 },
        new EmojiConfig(){ Emoji_ID = 5,Emoji_Name = "ÉúÆø" ,Emoji_Distance = 5},
        new EmojiConfig(){ Emoji_ID = 6,Emoji_Name = "¾ª¿Ö" ,Emoji_Distance = 5 },
        new EmojiConfig(){ Emoji_ID = 7,Emoji_Name = "¾ªÑÈ" ,Emoji_Distance = 5},
        new EmojiConfig(){ Emoji_ID = 8,Emoji_Name = "¾´Àñ" ,Emoji_Distance = 5},
        new EmojiConfig(){ Emoji_ID = 9,Emoji_Name = "¹¥»÷" ,Emoji_Distance = 5},
        new EmojiConfig(){ Emoji_ID = 10,Emoji_Name = "Ò¹Íí" ,Emoji_Distance = 5},
        new EmojiConfig(){ Emoji_ID = 11,Emoji_Name = "Ê³Îï" ,Emoji_Distance = 5},
        new EmojiConfig(){ Emoji_ID = 12,Emoji_Name = "¹¥»÷" ,Emoji_Distance = 5},
        new EmojiConfig(){ Emoji_ID = 13,Emoji_Name = "½ð±Ò" ,Emoji_Distance = 5},
        new EmojiConfig(){ Emoji_ID = 14,Emoji_Name = "ËæÃß" ,Emoji_Distance = 5},
        new EmojiConfig(){ Emoji_ID = 15,Emoji_Name = "Ê¡ÂÔ" ,Emoji_Distance = 5 },
        new EmojiConfig(){ Emoji_ID = 16,Emoji_Name = "´ó½Ð" ,Emoji_Distance = 20},
    };

}
public struct EmojiConfig
{
    /// <summary>
    /// ID
    /// </summary>
    public int Emoji_ID;
    /// <summary>
    /// Ãû×Ö
    /// </summary>
    public string Emoji_Name;
    /// <summary>
    /// ¾àÀë
    /// </summary>
    public float Emoji_Distance;
}
