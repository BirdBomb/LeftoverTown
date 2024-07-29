using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipManager
{
    SingleClip[] allSingleClip;
    Dictionary<string , SingleClip> singleClipDic = new Dictionary<string, SingleClip>(); 
    public SingleClip FindClipByID(string name)
    {
        if (name != "")
        {
            if (singleClipDic.ContainsKey(name))
            {
                return singleClipDic[name];
            }
            else
            {
                AudioClip clip = Resources.Load<AudioClip>("Audio/" + name);
                SingleClip single = new SingleClip(clip);
                singleClipDic[name] = single;
                return single;
            }
        }
        else
        {
            return null;
        }
    }
}
