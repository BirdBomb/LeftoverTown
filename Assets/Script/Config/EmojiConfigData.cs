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
        new EmojiConfig(){ Emoji_ID = 0,Emoji_Name = "��" ,Emoji_Distance = 5 },
        new EmojiConfig(){ Emoji_ID = 1,Emoji_Name = "�ɻ�" ,Emoji_Distance = 5 },
        new EmojiConfig(){ Emoji_ID = 2,Emoji_Name = "��" ,Emoji_Distance = 5},
        new EmojiConfig(){ Emoji_ID = 3,Emoji_Name = "��" ,Emoji_Distance = 5},
        new EmojiConfig(){ Emoji_ID = 4,Emoji_Name = "����" ,Emoji_Distance = 5 },
        new EmojiConfig(){ Emoji_ID = 5,Emoji_Name = "����" ,Emoji_Distance = 5},
        new EmojiConfig(){ Emoji_ID = 6,Emoji_Name = "����" ,Emoji_Distance = 5 },
        new EmojiConfig(){ Emoji_ID = 7,Emoji_Name = "����" ,Emoji_Distance = 5},
        new EmojiConfig(){ Emoji_ID = 8,Emoji_Name = "����" ,Emoji_Distance = 5},
        new EmojiConfig(){ Emoji_ID = 9,Emoji_Name = "����" ,Emoji_Distance = 5},
        new EmojiConfig(){ Emoji_ID = 10,Emoji_Name = "ҹ��" ,Emoji_Distance = 5},
        new EmojiConfig(){ Emoji_ID = 11,Emoji_Name = "ʳ��" ,Emoji_Distance = 5},
        new EmojiConfig(){ Emoji_ID = 12,Emoji_Name = "����" ,Emoji_Distance = 5},
        new EmojiConfig(){ Emoji_ID = 13,Emoji_Name = "���" ,Emoji_Distance = 5},
        new EmojiConfig(){ Emoji_ID = 14,Emoji_Name = "����" ,Emoji_Distance = 5},
        new EmojiConfig(){ Emoji_ID = 15,Emoji_Name = "ʡ��" ,Emoji_Distance = 5 },
        new EmojiConfig(){ Emoji_ID = 16,Emoji_Name = "���" ,Emoji_Distance = 20},
    };

}
public struct EmojiConfig
{
    /// <summary>
    /// ID
    /// </summary>
    public int Emoji_ID;
    /// <summary>
    /// ����
    /// </summary>
    public string Emoji_Name;
    /// <summary>
    /// ����
    /// </summary>
    public float Emoji_Distance;
}
