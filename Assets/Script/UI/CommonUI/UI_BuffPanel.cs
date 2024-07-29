using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_BuffPanel : MonoBehaviour
{
    public GameObject buffCell;
    public Transform pool;
    private List<UI_BuffCell> buffCells = new List<UI_BuffCell>();
    public void DrawBuffCell(BuffConfig buff, System.Action<BuffConfig> action = null)
    {
        GameObject obj = Instantiate(buffCell, pool);
        obj.GetComponent<UI_BuffCell>().InitBuff(buff,action);
        buffCells.Add(obj.GetComponent<UI_BuffCell>());
    }
    public void FindBuffCell(BuffConfig buff,out UI_BuffCell buffCell)
    {
        for (int i = 0; i < buffCells.Count; i++)
        {
            int index = i;
            if (buffCells[index].buffData.Buff_ID == buff.Buff_ID)
            {
                buffCell = buffCells[index];
            }
        }
        buffCell = null;
    }
    public void DestroyBuffCell(BuffConfig buff)
    {
        for(int i = 0; i < buffCells.Count; i++)
        {
            if (buffCells[i].buffData.Buff_ID == buff.Buff_ID)
            {
                Destroy(buffCells[i].gameObject);
                buffCells.RemoveAt(i);
                break;
            }
        }
    }
    public void ClearBuffCell()
    {
        for (int i = 0; i < buffCells.Count; i++)
        {
            Destroy(buffCells[i].gameObject);
        }
        buffCells.Clear();
    }
}
