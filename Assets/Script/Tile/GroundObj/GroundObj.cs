using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundObj : MonoBehaviour
{
    [HideInInspector]
    public GroundTile groundTile;
    /// <summary>
    /// ��
    /// </summary>
    public virtual void Bind(GroundTile tile,out GroundObj obj)
    {
        groundTile = tile;
        obj = this;
    }
    /// <summary>
    /// ���Ƶؿ�
    /// </summary>
    public virtual void Draw()
    {

    }
}
