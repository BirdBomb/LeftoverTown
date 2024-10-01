using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.U2D;
using UniRx;
using System.Text;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;
using TMPro;

public class UI_FloorBuilder : UI_Grid
{
    #region//����Ԥ��
    [SerializeField, Header("�ؿ�Ԥ��")]
    private RectTransform panel_floorPanel;
    [SerializeField, Header("�ؿ�����")]
    private Text text_floorName;
    [SerializeField, Header("�ؿ�ͼƬ")]
    private Image image_floorSprite;
    [SerializeField, Header("�ϳ���ͼƬ")]
    private List<Image> imageList_floorRawSprite = new List<Image>();
    [SerializeField, Header("�ϳ�������")]
    private List<Text> textList_floorRawText = new List<Text>();
    [SerializeField, Header("׼�����찴ť")]
    private Button btn_readyBuild;
    [SerializeField, Header("��ʼ���찴ť")]
    private Button btn_startBuild;
    #endregion
    #region//�����б�
    [SerializeField, Header("�ؿ��б�")]
    private RectTransform panel_floorList;
    [SerializeField, Header("�ؿ�ȼ�")]
    private Text text_floorLevel;
    [SerializeField, Header("�ؿ鰴ť�б�")]
    private List<Button> btns_floorNameBtn = new List<Button>();
    [SerializeField, Header("��һ�ѶȰ�ť")]
    private Button btn_LastLevel;
    [SerializeField, Header("��һ�ѶȰ�ť")]
    private Button btn_NextLevel;
    [SerializeField, Header("��һҳ��ť")]
    private Button btn_LastPage;
    [SerializeField, Header("��һҳ��ť")]
    private Button btn_NextPage;
    #endregion
    /// <summary>
    /// �ؿ��б�
    /// </summary>
    private List<FloorConfig> floorConfigs = new List<FloorConfig>();
    /// <summary>
    /// ѡ�еĵؿ�
    /// </summary>
    private FloorConfig targetFloor;
    private List<ItemData> itemDataList = new List<ItemData>();
    private int curPage;
    private int CurPage
    {
        get { return curPage; }
        set
        {
            curPage = value;
        }
    }

    private int curLevel;
    private int CurLevel
    {
        get { return curLevel; }
        set
        {
            if (curLevel != value)
            {
                floorConfigs = FloorConfigData.floorConfigs.FindAll((x) => { return x.Floor_RawLevel == value; });
                curLevel = value;
                CurPage = 0;
                UpdateBuildingListUI();
            }
        }
    }

    private TileObj bindTileObj;
    private SpriteAtlas buildingAtlas;
    private SpriteAtlas itemAtlas;
    private void Awake()
    {
        buildingAtlas = Resources.Load<SpriteAtlas>("Atlas/TileSprite");
        itemAtlas = Resources.Load<SpriteAtlas>("Atlas/ItemSprite");
    }
    private void Start()
    {
        btn_startBuild.onClick.AddListener(ClickStartBuildBtn);
        btn_LastLevel.onClick.AddListener(ClickLastLevelBtn);
        btn_NextLevel.onClick.AddListener(ClickNextLevelBtn);
        btn_LastPage.onClick.AddListener(ClickLastPageBtn);
        btn_NextPage.onClick.AddListener(ClickNextPageBtn);
        floorConfigs = FloorConfigData.floorConfigs.FindAll((x) => { return x.Floor_RawLevel == 0; });
        UpdateBuildingListUI();
    }
    #region//�򿪹ر�
    public override void Open(TileObj tileObj)
    {
        bindTileObj = tileObj;
        base.Open(tileObj);
    }
    #endregion
    #region//UI����
    public override void CellDragBegin(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {

    }
    public override void CellDragIn(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(gridCell.image_MainIcon.rectTransform, Input.mousePosition, Camera.main, out Vector3 pos);
        gridCell.image_MainIcon.transform.position = pos;
    }
    public override void CellDragEnd(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        if (raycastResults.Count > 0)
        {
            foreach (RaycastResult result in raycastResults)
            {
                if (result.gameObject.TryGetComponent(out UI_Grid grid))
                {
                    PutOut(itemData, out ItemData afterData);
                    grid.ListenDragOn(this, gridCell, afterData);
                    return;
                }
            }
        }
        else
        {
            PutOut(itemData, out ItemData afterData);
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryDropItem()
            {
                item = afterData
            });
        }

    }
    public override void ListenDragOn<T>(T grid, UI_GridCell cell, ItemData itemData)
    {
        PutIn(itemData, out ItemData back);
        if (back.Item_Count != 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                item = back,
            });
        }
        base.ListenDragOn<T>(grid, cell, itemData);
    }
    /// <summary>
    /// ��һ�ѶȰ�ť���
    /// </summary>
    private void ClickNextLevelBtn()
    {
        if (curLevel < 10)
        {
            CurLevel++;
        }
        else
        {
            CurLevel = 0;
        }
    }
    /// <summary>
    /// ��һ�ѶȰ�ť���
    /// </summary>
    private void ClickLastLevelBtn()
    {
        if (curLevel > 0)
        {
            CurLevel--;
        }
        else
        {
            CurLevel = 10;
        }
    }
    /// <summary>
    /// ��һҳ��ť���
    /// </summary>
    private void ClickNextPageBtn()
    {
        if (CurPage * 10 < floorConfigs.Count)
        {
            CurPage++;
        }
        else
        {
            CurPage = 0;
        }
        UpdateBuildingListUI();
    }
    /// <summary>
    /// ��һҳ��ť���
    /// </summary>
    private void ClickLastPageBtn()
    {
        if (CurPage > 0)
        {
            CurPage--;
            UpdateBuildingListUI();
        }
    }
    /// <summary>
    /// Ԥ��������ť���
    /// </summary>
    /// <param name="buildingConfig"></param>
    private void ClickBuildingNameBtn(FloorConfig floorConfig)
    {
        if (targetFloor.Floor_ID == 0)
        {
            DrawBuildingPanel(floorConfig);
            DrawBuildingRaw(floorConfig);
            btn_readyBuild.gameObject.SetActive(true);
            btn_readyBuild.onClick.RemoveAllListeners();
            btn_readyBuild.onClick.AddListener(() => { ClickReadyBuildBtn(floorConfig); });
        }
    }
    /// <summary>
    /// ׼�����찴ť���
    /// </summary>
    /// <param name="buildingConfig"></param>
    private void ClickReadyBuildBtn(FloorConfig floorConfig)
    {
        targetFloor = floorConfig;
        ChangeInfoToTile();
    }
    /// <summary>
    /// ��ʼ���찴ť
    /// </summary>
    private void ClickStartBuildBtn()
    {
        MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_ChangeBuilding()
        {
            buildingID = 0,
            buildingPos = bindTileObj.bindTile._posInCell
        });
        MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_ChangeFloor()
        {
            floorID = targetFloor.Floor_ID,
            floorPos = bindTileObj.bindTile._posInCell
        });
    }

    #endregion
    #region//UI����
    /// <summary>
    /// ���ý���UI
    /// </summary>
    private void ResetBuildUI()
    {
        btn_readyBuild.gameObject.SetActive(false);
        btn_startBuild.gameObject.SetActive(false);
        text_floorName.text = "";
        image_floorSprite.gameObject.SetActive(false);
        for (int i = 0; i < imageList_floorRawSprite.Count; i++)
        {
            imageList_floorRawSprite[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < textList_floorRawText.Count; i++)
        {
            textList_floorRawText[i].text = "";
        }
    }
    /// <summary>
    /// ���½����б�
    /// </summary>
    private void UpdateBuildingListUI()
    {
        for (int i = 0; i < btns_floorNameBtn.Count; i++)
        {
            btns_floorNameBtn[i].onClick.RemoveAllListeners();
            if (i + CurPage * 10 < floorConfigs.Count)
            {
                FloorConfig floorConfig = floorConfigs[i + CurPage * 10];
                btns_floorNameBtn[i].gameObject.SetActive(true);
                btns_floorNameBtn[i].onClick.AddListener(() => { ClickBuildingNameBtn(floorConfig); });
                btns_floorNameBtn[i].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = floorConfig.Floor_Name;
            }
            else
            {
                btns_floorNameBtn[i].gameObject.SetActive(false);
                btns_floorNameBtn[i].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "";
            }
        }
        text_floorLevel.text = "�Ѷȵȼ�" + CurLevel;
    }
    private void HideBuildingListUI()
    {
        panel_floorList.DOLocalMoveX(0, 0.2f);
        panel_floorPanel.DOLocalMoveX(0, 0.2f);

        btn_NextLevel.gameObject.SetActive(false);
        btn_LastLevel.gameObject.SetActive(false);
        btn_NextPage.gameObject.SetActive(false);
        btn_LastPage.gameObject.SetActive(false);
    }
    /// <summary>
    /// ���ƽ���Ԥ������
    /// </summary>
    private void DrawBuildingPanel(FloorConfig config)
    {
        if (config.Floor_ID > 0)
        {
            image_floorSprite.gameObject.SetActive(true);
            image_floorSprite.sprite = buildingAtlas.GetSprite(config.Floor_SpriteName);
            text_floorName.text = config.Floor_Name;
        }
        else
        {
            image_floorSprite.gameObject.SetActive(false);
            text_floorName.text = "";
        }
    }
    /// <summary>
    /// ���ƽ������Ͻ���
    /// </summary>
    private void DrawBuildingRaw(FloorConfig config)
    {
        for (int i = 0; i < imageList_floorRawSprite.Count; i++)
        {
            imageList_floorRawSprite[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < textList_floorRawText.Count; i++)
        {
            textList_floorRawText[i].text = "";
        }

        if (config.Floor_ID == 0)
        {
            for (int i = 0; i < config.Floor_Raw.Count; i++)
            {
                imageList_floorRawSprite[i].gameObject.SetActive(true);
                imageList_floorRawSprite[i].sprite = itemAtlas.GetSprite("Item_" + config.Floor_Raw[i].ID.ToString());
                textList_floorRawText[i].text = "0/" + config.Floor_Raw[i].Count.ToString();
            }
        }
        else
        {
            for (int i = 0; i < config.Floor_Raw.Count; i++)
            {
                imageList_floorRawSprite[i].gameObject.SetActive(true);
                imageList_floorRawSprite[i].sprite = itemAtlas.GetSprite("Item_" + config.Floor_Raw[i].ID.ToString());

                int tempIndex = itemDataList.FindIndex((x) => { return x.Item_ID == config.Floor_Raw[i].ID; });

                if (tempIndex >= 0)
                {
                    textList_floorRawText[i].text = itemDataList[tempIndex].Item_Count.ToString() + "/" + config.Floor_Raw[i].Count.ToString();
                }
                else
                {
                    textList_floorRawText[i].text = "0/" + config.Floor_Raw[i].Count.ToString();
                }
            }

        }
    }

    #endregion
    #region//�ó�����
    public override void PutOut(ItemData before, out ItemData after)
    {
        itemDataList.Remove(before);
        after = before;
        ChangeInfoToTile();
    }
    public override void PutIn(ItemData before, out ItemData after)
    {
        if (targetFloor.Floor_ID > 0)
        {
            ItemData resData = before;

            for (int i = 0; i < targetFloor.Floor_Raw.Count; i++)
            {
                if (targetFloor.Floor_Raw[i].ID == before.Item_ID)
                {
                    /*��Ҫ�����Ϊ����*/
                    int index = itemDataList.FindIndex((x) => { return x.Item_ID == before.Item_ID; });
                    if (index >= 0)
                    {
                        /*�Ѿ����������*/
                        Type type = Type.GetType("Item_" + before.Item_ID.ToString());
                        ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(itemDataList[index], resData, targetFloor.Floor_Raw[i].Count, out ItemData newData, out resData);
                        itemDataList[index] = newData;
                    }
                    else
                    {
                        /*��û���������*/
                        ItemData itemData = before;
                        itemData.Item_Count = 0;
                        Type type = Type.GetType("Item_" + before.Item_ID.ToString());
                        ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(itemData, resData, targetFloor.Floor_Raw[i].Count, out ItemData newData, out resData);
                        itemDataList.Add(newData);
                    }
                }
            }
            after = resData;
        }
        else
        {
            base.PutIn(before, out after);
        }
        ChangeInfoToTile();
    }
    #endregion
    #region//��Ϣ�ϴ������
    /// <summary>
    /// ������Ϣ
    /// </summary>
    /// <param name="info"></param>
    public void UpdateInfoFromTile(string info)
    {
        itemDataList.Clear();
        string[] strings = info.Split("/*I*/");
        for (int i = 0; i < strings.Length; i++)
        {
            if (i == 0)
            {
                /*��һλ�Ǵ�������id*/
                if (strings[i] != "")
                {
                    targetFloor = FloorConfigData.GetItemConfig(int.Parse(strings[i]));
                }
            }
            else if (strings[i] != "")
            {
                /*��λλ���Ѿ���ӵĲ���*/
                ItemData data = JsonUtility.FromJson<ItemData>(strings[i]);
                itemDataList.Add(data);
            }
        }
        ResetBuildUI();
        if (targetFloor.Floor_ID != 0)
        {
            DrawBuildingPanel(targetFloor);
            DrawBuildingRaw(targetFloor);
            HideBuildingListUI();
            CheckRawList();
        }
    }
    /// <summary>
    /// ������Ϣ
    /// </summary>
    public void ChangeInfoToTile()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(targetFloor.Floor_ID.ToString());
        for (int i = 0; i < itemDataList.Count; i++)
        {
            builder.Append("/*I*/" + JsonUtility.ToJson(itemDataList[i]));
        }
        bindTileObj.TryToChangeInfo(builder.ToString());
    }
    #endregion
    #region//����UI
    /// <summary>
    /// ��齨������
    /// </summary>
    private void CheckRawList()
    {
        if (targetFloor.Floor_ID != 0)
        {
            for (int i = 0; i < targetFloor.Floor_Raw.Count; i++)
            {
                int index = itemDataList.FindIndex((x) => { return x.Item_ID == targetFloor.Floor_Raw[i].ID; });
                if (index >= 0)
                {
                    if (itemDataList[index].Item_Count < targetFloor.Floor_Raw[i].Count)
                    {
                        /*���������������*/
                        return;
                    }
                }
                else
                {
                    /*û���������*/
                    return;
                }
            }
            btn_startBuild.gameObject.SetActive(true);
        }
    }
    #endregion
}
