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
     * 1000 - 1999 ����
     * 2000 - 2999 ����
     * 3000 - 3999 ʳ��
     * 4000 - 4999 ����
     * 5000 - 5999 ����
     * 9000 - 9999 ����
     */
    public readonly static List<EmojiConfig> emojiConfigs = new List<EmojiConfig>()
    {
        new EmojiConfig(){ Emoji_ID = 0,Emoji_Name = "��" },
        new EmojiConfig(){ Emoji_ID = 1,Emoji_Name = "�ɻ�" },
        new EmojiConfig(){ Emoji_ID = 2,Emoji_Name = "��" },
        new EmojiConfig(){ Emoji_ID = 3,Emoji_Name = "��" },
        new EmojiConfig(){ Emoji_ID = 4,Emoji_Name = "����" },
        new EmojiConfig(){ Emoji_ID = 5,Emoji_Name = "����" },
        new EmojiConfig(){ Emoji_ID = 6,Emoji_Name = "����" },
        new EmojiConfig(){ Emoji_ID = 7,Emoji_Name = "����" },
        new EmojiConfig(){ Emoji_ID = 8,Emoji_Name = "����" },
        new EmojiConfig(){ Emoji_ID = 9,Emoji_Name = "����" },
        new EmojiConfig(){ Emoji_ID = 10,Emoji_Name = "ҹ��" },
        new EmojiConfig(){ Emoji_ID = 11,Emoji_Name = "ʳ��" },
        new EmojiConfig(){ Emoji_ID = 12,Emoji_Name = "����" },
        new EmojiConfig(){ Emoji_ID = 13,Emoji_Name = "���" },
        new EmojiConfig(){ Emoji_ID = 14,Emoji_Name = "����" },
        new EmojiConfig(){ Emoji_ID = 15,Emoji_Name = "ʡ��" },
        new EmojiConfig(){ Emoji_ID = 16,Emoji_Name = "���" },
    };

}
public struct EmojiConfig
{
    public int Emoji_ID;
    public string Emoji_Name;
}
