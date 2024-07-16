using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipManager
{
    SingleClip[] allSingleClip;
    Dictionary<int , SingleClip> singleClipDic = new Dictionary<int, SingleClip>(); 
    public ClipManager()
    {
    }
    public SingleClip FindClipByID(int id)
    {
        AudioConfig audioConfig = AudioConfigData.audioConfigs.Find((x) => { return x.Audio_ID == id; });
        if (audioConfig.Audio_Name != "")
        {
            if (singleClipDic.ContainsKey(id))
            {
                return singleClipDic[id];
            }
            else
            {
                AudioClip clip = Resources.Load<AudioClip>("Audio/" + audioConfig.Audio_Name);
                SingleClip single = new SingleClip(clip);
                singleClipDic[id] = single;
                return single;
            }
        }
        else
        {
            return null;
        }
    }
}
