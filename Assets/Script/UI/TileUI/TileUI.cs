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
}
