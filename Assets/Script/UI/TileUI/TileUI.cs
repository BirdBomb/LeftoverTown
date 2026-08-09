using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileUI : MonoBehaviour
{
    public virtual void Show()
    {

    }
    public virtual void Hide()
    {
        Destroy(gameObject);
    }
    /// <summary>
    /// 是否需要展开Bag
    /// </summary>
    /// <returns></returns>
    public virtual bool NeedToOpenBagPanel()
    {
        return true;
    }
}
