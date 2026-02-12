using DG.Tweening;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CreateMap : MonoBehaviour
{
    [SerializeField]
    private Transform transform_Panel;
    [SerializeField]
    private Button btn_Create;
    private Action action_Create;
    [SerializeField]
    private Button btn_Return;
    private Action action_Return;
    [SerializeField]
    private TMP_InputField input_MapName;
    [SerializeField]
    private TMP_InputField input_MapSeed;
    [SerializeField]
    public Button btn_RefreshSeed;
    [SerializeField]
    private TMP_Dropdown dropdown_MapType;
    [SerializeField]
    private Slider slider_Waiting;
    [SerializeField]
    private TextMeshProUGUI text_Waiting;

    /*自定义地图*/
    private MapInfoData mapInfoData = new MapInfoData();
    private MapTileInfoData buildingInfoData = new MapTileInfoData();
    private MapTileTypeData buildingTypeData = new MapTileTypeData();
    private MapTileTypeData floorTypeData = new MapTileTypeData();
    private MapCreate mapCreater_Bind = new MapCreate();
    private string mapSeed = "";
    private string mapName;
    private string bind_MapInfoPath;
    private string bind_BuildingInfoPath;
    private string bind_BuildingTypePath;
    private string bind_FloorTypePath;

    private void Awake()
    {
        Bind();
    }
    public void Init(int index, Action actionCreate, Action actionReturn)
    {
        bind_MapInfoPath = "MapData/MapInfo" + index;
        bind_BuildingInfoPath = "MapData/MapBuildingInfo" + index;
        bind_BuildingTypePath = "MapData/MapBuildingType" + index;
        bind_FloorTypePath = "MapData/MapFloorType" + index;
        btn_Create.interactable = true;
        transform_Panel.gameObject.SetActive(true);
        transform_Panel.transform.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.1f);
        action_Create = actionCreate;
        action_Return = actionReturn;
    }
    private void Bind()
    {
        btn_Return.onClick.AddListener(Return);
        btn_Create.onClick.AddListener(Create);
        btn_RefreshSeed.onClick.AddListener(GetRandomSeed);
        input_MapName.onValueChanged.AddListener(ChangeName);
        input_MapSeed.onValueChanged.AddListener(ChangeSeed);
    }
    public void ChangeSeed(string seed)
    {
        mapSeed = seed.ToString();
    }
    public void ChangeName(string name)
    {
        mapName = name.ToString();
    }
    public void GetRandomSeed()
    {
        string str = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        char[] chars = str.ToCharArray();
        StringBuilder strRan = new StringBuilder();
        for (int i = 0; i < 12; i++)
        {
            UnityEngine.Random.InitState(System.DateTime.Now.Second + i);
            strRan.Append(chars[UnityEngine.Random.Range(0, 37)]);
        }
        mapSeed = strRan.ToString();
        input_MapSeed.text = mapSeed;
    }
    public int ReadRandomSeed(string seed)
    {
        string str = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        int seedInt = 0;
        for (int i = 0; i < seed.Length; i++)
        {
            char c = seed[i];
            int temp = str.IndexOf(c);
            seedInt += temp * (int)Mathf.Pow(10, i);
        }
        return seedInt;
    }

    public async void Create()
    {
        Debug.Log("开始生成");
        btn_Create.interactable = false;
        await SetMapData();
        Debug.Log("结束生成");
        FileManager.Instance.WriteFile(bind_MapInfoPath, JsonConvert.SerializeObject(mapInfoData));
        FileManager.Instance.WriteFile(bind_BuildingInfoPath, JsonConvert.SerializeObject(buildingInfoData));
        FileManager.Instance.WriteFile(bind_BuildingTypePath, JsonConvert.SerializeObject(buildingTypeData));
        FileManager.Instance.WriteFile(bind_FloorTypePath, JsonConvert.SerializeObject(floorTypeData));
        transform_Panel.gameObject.SetActive(false);
        if (action_Create != null)
        {
            action_Create.Invoke();
        }
    }
    public async Task SetMapData()
    {
        MapConfig config = new MapConfig();
        if (mapSeed.Equals("")) { GetRandomSeed(); }
        config.map_Seed = ReadRandomSeed(mapSeed);
        switch (dropdown_MapType.value)
        {
            case 0:/*小地图*/
                config.map_Size = 500;
                break;
            case 1:/*中地图*/
                config.map_Size = 750;
                break;
            case 2:/*大地图*/
                config.map_Size = 1000;
                break;
        }

        await mapCreater_Bind.CreateMapGroundAndBuilding(config, slider_Waiting, text_Waiting, (x, y) =>
        {
            floorTypeData = x;
            buildingTypeData = y;
        });

        if (buildingInfoData == null) { buildingInfoData = new MapTileInfoData(); }
        if (buildingTypeData == null) { buildingTypeData = new MapTileTypeData(); }
        if (floorTypeData == null) { floorTypeData = new MapTileTypeData(); }
        mapInfoData.name = mapName;
        mapInfoData.seed = mapSeed;
    }

    public void Return()
    {
        transform_Panel.gameObject.SetActive(false);
        if (action_Return != null)
        {
            action_Return.Invoke();
        }
    }

}
