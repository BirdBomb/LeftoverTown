using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PointPanel : MonoBehaviour
{
    public List<GameObject> points = new List<GameObject>();
    public void UpdatePoint(int count)
    {
        for(int i = 0; i < points.Count; i++)
        {
            if (i < count)
            {
                points[i].SetActive(true);
            }
            else
            {
                points[i].SetActive(false);
            }
        }
    }
}
