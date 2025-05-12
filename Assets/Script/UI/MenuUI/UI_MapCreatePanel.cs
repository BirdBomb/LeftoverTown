using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MapCreatePanel : MonoBehaviour
{
    public Transform transform_Panel;
    public Button btn_Return;
    public Button btn_Create;
    public Button btn_Refresh;
    public Slider slider_Waiting;
    public TextMeshProUGUI text_Waiting;

    private MapCreater mapCreater_Bind = new MapCreater();

    public TMP_InputField input_MapName;
    public TMP_InputField input_MapSeed;
    public TMP_Dropdown dropdown_MapType;
    /*自定义地图*/
    private MapInfoData mapInfoData= new MapInfoData();
    private MapTileInfoData buildingInfoData = new MapTileInfoData();
    private MapTileTypeData buildingTypeData = new MapTileTypeData();
    private MapTileTypeData floorTypeData = new MapTileTypeData();
    private string mapSeed;
    private string mapName;
    private string bind_MapInfoPath;
    private string bind_BuildingInfoPath;
    private string bind_BuildingTypePath;
    private string bind_FloorTypePath;
    private Action action_Create;
    private void Awake()
    {
        Bind();
    }
    private void Bind()
    {
        btn_Return.onClick.AddListener(Hide);
        btn_Create.onClick.AddListener(CreateMapData);
        btn_Refresh.onClick.AddListener(GetRandomSeed);
        input_MapName.onValueChanged.AddListener(ChangeName);
        input_MapSeed.onValueChanged.AddListener(ChangeSeed);
    }
    public void Init(string mapInfoPath,string buildingInfoPath, string buildingTypePath, string floorTypePath, Action create)
    {
        bind_MapInfoPath = mapInfoPath;
        bind_BuildingInfoPath = buildingInfoPath;
        bind_BuildingTypePath = buildingTypePath;
        bind_FloorTypePath = floorTypePath;
        action_Create = create;
        GetRandomSeed();
    }
    public void Show()
    {
        transform_Panel.gameObject.SetActive(true);
        btn_Create.enabled = true;
    }
    public void Hide()
    {
        transform_Panel.gameObject.SetActive(false);
        btn_Create.enabled = false;
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
            strRan.Append(chars[UnityEngine.Random.Range(0,37)]);
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
    public async void CreateMapData()
    {
        Debug.Log("开始生成");
        btn_Create.enabled = true;
        await SetMapData();
        Debug.Log("结束生成");
        FileManager.Instance.WriteFile(bind_MapInfoPath, JsonConvert.SerializeObject(mapInfoData));
        FileManager.Instance.WriteFile(bind_BuildingInfoPath, JsonConvert.SerializeObject(buildingInfoData));
        FileManager.Instance.WriteFile(bind_BuildingTypePath, JsonConvert.SerializeObject(buildingTypeData));
        FileManager.Instance.WriteFile(bind_FloorTypePath, JsonConvert.SerializeObject(floorTypeData));
        if (action_Create != null)
        {
            action_Create.Invoke();
        }
    }
    public async Task SetMapData()
    {
        MapConfig config = new MapConfig();
        config.map_Seed = ReadRandomSeed(mapSeed);
        switch (dropdown_MapType.value)
        {
            case 0:/*小地图*/
                config.map_Size = 50;
                break;
            case 1:/*中地图*/
                config.map_Size = 100;
                break;
            case 2:/*大地图*/
                config.map_Size = 200;
                break;
        }

        await mapCreater_Bind.CreateMapGroundAndBuilding(config, slider_Waiting, text_Waiting, (x,y) =>
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
}
