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
        new EmojiConfig(){ Emoji_ID = 0,Emoji_Name = "Õð¾ª" },
        new EmojiConfig(){ Emoji_ID = 1,Emoji_Name = "ÒÉ»ó" },
        new EmojiConfig(){ Emoji_ID = 2,Emoji_Name = "ÊÇ" },
        new EmojiConfig(){ Emoji_ID = 3,Emoji_Name = "·ñ" },
        new EmojiConfig(){ Emoji_ID = 4,Emoji_Name = "¿ªÐÄ" },
        new EmojiConfig(){ Emoji_ID = 5,Emoji_Name = "ÉúÆø" },
        new EmojiConfig(){ Emoji_ID = 6,Emoji_Name = "¾ª¿Ö" },
        new EmojiConfig(){ Emoji_ID = 7,Emoji_Name = "¾ªÑÈ" },
        new EmojiConfig(){ Emoji_ID = 8,Emoji_Name = "¾´Àñ" },
        new EmojiConfig(){ Emoji_ID = 9,Emoji_Name = "¹¥»÷" },
        new EmojiConfig(){ Emoji_ID = 10,Emoji_Name = "Ò¹Íí" },
        new EmojiConfig(){ Emoji_ID = 11,Emoji_Name = "Ê³Îï" },
        new EmojiConfig(){ Emoji_ID = 12,Emoji_Name = "¹¥»÷" },
        new EmojiConfig(){ Emoji_ID = 13,Emoji_Name = "½ð±Ò" },
        new EmojiConfig(){ Emoji_ID = 14,Emoji_Name = "ËæÃß" },
        new EmojiConfig(){ Emoji_ID = 15,Emoji_Name = "Ê¡ÂÔ" },
        new EmojiConfig(){ Emoji_ID = 16,Emoji_Name = "´ó½Ð" },
    };

}
public struct EmojiConfig
{
    public int Emoji_ID;
    public string Emoji_Name;
}
