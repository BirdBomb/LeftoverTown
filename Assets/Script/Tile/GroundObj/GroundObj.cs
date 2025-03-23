using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundObj : MonoBehaviour
{
    [HideInInspector]
    public GroundTile groundTile;
    /// <summary>
    /// °ó¶¨
    /// </summary>
    public virtual void Bind(GroundTile tile,out GroundObj obj)
    {
        groundTile = tile;
        obj = this;
    }
    /// <summary>
    /// »æÖÆµØ¿é
    /// </summary>
    public virtual void Draw()
    {

    }
}
