using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.U2D;
using static Fusion.Allocator;
using UniRx;
using System.Text;
using UnityEngine.EventSystems;
using System;
using TMPro;
using DG.Tweening;

public class UI_BuildingBuilder : UI_Grid
{
    #region//����Ԥ��
    [SerializeField, Header("����Ԥ��")]
    private RectTransform panel_buildPanel;
    [SerializeField, Header("��������")]
    private Text text_buildName;
    [SerializeField, Header("����ͼƬ")]
    private Image image_buildSprite;
    [SerializeField, Header("�ϳ���ͼƬ")]
    private List<Image> imageList_buildRawSprite = new List<Image>();
    [SerializeField, Header("�ϳ�������")]
    private List<Text> textList_buildRawText = new List<Text>();
    [SerializeField, Header("׼�����찴ť")]
    private Button btn_readyBuild;
    [SerializeField, Header("��ʼ���찴ť")]
    private Button btn_startBuild;
    #endregion
    #region//�����б�
    [SerializeField, Header("�����б�")]
    private RectTransform panel_buildList;
    [SerializeField, Header("�����ȼ�")]
    private Text text_buildingLevel;
    [SerializeField, Header("������ť�б�")]
    private List<Button> btns_buildingNameBtn = new List<Button>();
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
    /// �����б�
    /// </summary>
    private List<BuildingConfig> buildingConfigs = new List<BuildingConfig>();
    /// <summary>
    /// ѡ�еĽ���
    /// </summary>
    private BuildingConfig targetBuilding;
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
                buildingConfigs = BuildingConfigData.buildConfigs.FindAll((x) => { return x.Building_RawLevel == value; });
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
        buildingConfigs = BuildingConfigData.buildConfigs.FindAll((x) => { return x.Building_RawLevel == 0; });
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
        if (CurPage * 10 < buildingConfigs.Count)
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
    private void ClickBuildingNameBtn(BuildingConfig buildingConfig)
    {
        if (targetBuilding.Building_ID == 0)
        {
            DrawBuildingPanel(buildingConfig);
            DrawBuildingRaw(buildingConfig);
            btn_readyBuild.gameObject.SetActive(true);
            btn_readyBuild.onClick.RemoveAllListeners();
            btn_readyBuild.onClick.AddListener(() => { ClickReadyBuildBtn(buildingConfig); });
        }
    }
    /// <summary>
    /// ׼�����찴ť���
    /// </summary>
    /// <param name="buildingConfig"></param>
    private void ClickReadyBuildBtn(BuildingConfig buildingConfig)
    {
        targetBuilding = buildingConfig;
        ChangeInfoToTile();
    }
    /// <summary>
    /// ��ʼ���찴ť
    /// </summary>
    private void ClickStartBuildBtn()
    {
        MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_ChangeBuilding()
        {
            buildingID = targetBuilding.Building_ID,
            buildingPos = bindTileObj.bindTile._posInCell
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
        text_buildName.text = "";
        image_buildSprite.gameObject.SetActive(false);
        for (int i = 0; i < imageList_buildRawSprite.Count; i++)
        {
            imageList_buildRawSprite[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < textList_buildRawText.Count; i++)
        {
            textList_buildRawText[i].text = "";
        }
    }
    /// <summary>
    /// ���½����б�
    /// </summary>
    private void UpdateBuildingListUI()
    {
        for (int i = 0; i < btns_buildingNameBtn.Count; i++)
        {
            btns_buildingNameBtn[i].onClick.RemoveAllListeners();
            if (i + CurPage * 10 < buildingConfigs.Count)
            {
                BuildingConfig buildingConfig = buildingConfigs[i + CurPage * 10];
                btns_buildingNameBtn[i].gameObject.SetActive(true);
                btns_buildingNameBtn[i].onClick.AddListener(() => { ClickBuildingNameBtn(buildingConfig); });
                btns_buildingNameBtn[i].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = buildingConfig.Building_Name;
            }
            else
            {
                btns_buildingNameBtn[i].gameObject.SetActive(false);
                btns_buildingNameBtn[i].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "";
            }
        }
        text_buildingLevel.text = "�Ѷȵȼ�" + CurLevel;
    }
    private void HideBuildingListUI()
    {
        panel_buildList.DOLocalMoveX(0, 0.2f);
        panel_buildPanel.DOLocalMoveX(0, 0.2f);

        btn_NextLevel.gameObject.SetActive(false);
        btn_LastLevel.gameObject.SetActive(false);
        btn_NextPage.gameObject.SetActive(false);
        btn_LastPage.gameObject.SetActive(false);
    }
    /// <summary>
    /// ���ƽ���Ԥ������
    /// </summary>
    private void DrawBuildingPanel(BuildingConfig config)
    {
        if (config.Building_ID > 0)
        {
            image_buildSprite.gameObject.SetActive(true);
            image_buildSprite.sprite = buildingAtlas.GetSprite(config.Building_SpriteName);
            text_buildName.text = config.Building_Name;
        }
        else
        {
            image_buildSprite.gameObject.SetActive(false);
            text_buildName.text = "";
        }
    }
    /// <summary>
    /// ���ƽ������Ͻ���
    /// </summary>
    private void DrawBuildingRaw(BuildingConfig config)
    {
        for (int i = 0; i < imageList_buildRawSprite.Count; i++)
        {
            imageList_buildRawSprite[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < textList_buildRawText.Count; i++)
        {
            textList_buildRawText[i].text = "";
        }

        if (config.Building_ID == 0)
        {
            for (int i = 0; i < config.Building_Raw.Count; i++)
            {
                imageList_buildRawSprite[i].gameObject.SetActive(true);
                imageList_buildRawSprite[i].sprite = itemAtlas.GetSprite("Item_" + config.Building_Raw[i].ID.ToString());
                textList_buildRawText[i].text = "0/" + config.Building_Raw[i].Count.ToString();
            }
        }
        else
        {
            for (int i = 0; i < config.Building_Raw.Count; i++)
            {
                imageList_buildRawSprite[i].gameObject.SetActive(true);
                imageList_buildRawSprite[i].sprite = itemAtlas.GetSprite("Item_" + config.Building_Raw[i].ID.ToString());

                int tempIndex = itemDataList.FindIndex((x) => { return x.Item_ID == config.Building_Raw[i].ID; });

                if (tempIndex >= 0)
                {
                    textList_buildRawText[i].text = itemDataList[tempIndex].Item_Count.ToString() + "/" + config.Building_Raw[i].Count.ToString();
                }
                else
                {
                    textList_buildRawText[i].text = "0/" + config.Building_Raw[i].Count.ToString();
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
        if (targetBuilding.Building_ID > 0)
        {
            ItemData resData = before;

            for (int i = 0; i < targetBuilding.Building_Raw.Count; i++)
            {
                if (targetBuilding.Building_Raw[i].ID == before.Item_ID)
                {
                    /*��Ҫ�����Ϊ����*/
                    int index = itemDataList.FindIndex((x) => { return x.Item_ID == before.Item_ID; });
                    if (index >= 0)
                    {
                        /*�Ѿ����������*/
                        Type type = Type.GetType("Item_" + before.Item_ID.ToString());
                        ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(itemDataList[index], resData, targetBuilding.Building_Raw[i].Count, out ItemData newData, out resData);
                        itemDataList[index] = newData;
                    }
                    else
                    {
                        /*��û���������*/
                        ItemData itemData = before;
                        itemData.Item_Count = 0;
                        Type type = Type.GetType("Item_" + before.Item_ID.ToString());
                        ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(itemData, resData, targetBuilding.Building_Raw[i].Count, out ItemData newData, out resData);
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
                    targetBuilding = BuildingConfigData.GetItemConfig(int.Parse(strings[i]));
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
        if (targetBuilding.Building_ID != 0)
        {
            DrawBuildingPanel(targetBuilding);
            DrawBuildingRaw(targetBuilding);
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
        builder.Append(targetBuilding.Building_ID.ToString());
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
        if (targetBuilding.Building_ID != 0)
        {
            for (int i = 0; i < targetBuilding.Building_Raw.Count; i++)
            {
                int index = itemDataList.FindIndex((x) => { return x.Item_ID == targetBuilding.Building_Raw[i].ID; });
                if (index >= 0)
                {
                    if (itemDataList[index].Item_Count < targetBuilding.Building_Raw[i].Count)
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
