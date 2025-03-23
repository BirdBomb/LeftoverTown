using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;

public class UI_BuffGroup : MonoBehaviour
{
    [SerializeField]
    private GameObject buffIcon;
    [SerializeField]
    private SpriteAtlas spriteAtlas;
    [SerializeField]
    private Transform tran_Pool;
    private Dictionary<int, GameObject> dic_Pool = new Dictionary<int, GameObject>(); 
    public void Start()
    {
        MessageBroker.Default.Receive<UIEvent.UIEvent_AddBuff>().Subscribe(_ =>
        {
            CreateBuffIcon(_.buffConfig);
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_SubBuff>().Subscribe(_ =>
        {
            DestroyBuffIcon(_.buffConfig);
        }).AddTo(this);
    }
    private void UpdateBuffIcon(List<BuffConfig> buffConfigs)
    {
       
    }
    private void CreateBuffIcon(BuffConfig buffConfig)
    {
        if (buffConfig.Buff_Icon)
        {
            GameObject gameObject = Instantiate(buffIcon);
            gameObject.GetComponent<UI_BuffIcon>().Draw(spriteAtlas.GetSprite("Buff" + buffConfig.Buff_ID), buffConfig.Buff_Name, buffConfig.Buff_Desc);
            dic_Pool.Add(buffConfig.Buff_ID, gameObject);
            gameObject.transform.SetParent(tran_Pool);
            gameObject.transform.localScale = Vector3.one;
        }
    }
    private void DestroyBuffIcon(BuffConfig buffConfig)
    {
        if (dic_Pool.ContainsKey(buffConfig.Buff_ID))
        {
            Destroy(dic_Pool[buffConfig.Buff_ID]);
            dic_Pool.Remove(buffConfig.Buff_ID);
        }
    }
}
