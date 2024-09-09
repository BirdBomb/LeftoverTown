using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MapCreatePanel : MonoBehaviour
{
    public Transform panel_Main;
    public Button btn_Return;
    public Button btn_Create;
    public Button btn_Refresh;

    private MapTileTypeData mapBuildTypeData = new MapTileTypeData();
    private MapTileTypeData mapFloorTypeData = new MapTileTypeData();
    private MapTileTypeData mapBuildInfoData = new MapTileTypeData();

    public TMP_InputField input_MapName;
    public TMP_InputField input_MapSeed;
    public TMP_Dropdown dropdown_MapType;
    /*自定义地图*/
    private MapTileInfoData buildInfoData = new MapTileInfoData();
    private MapTileTypeData buildTypeData = new MapTileTypeData();
    private MapTileTypeData floorTypeData = new MapTileTypeData();
    private string _mapSeed;
    private string _mapName;
    private string _buildInfoPath;
    private string _buildTypePath;
    private string _FloorTypePath;
    /*地图模板0*/
    private MapTileInfoData buildInfoData_Map0 = new MapTileInfoData();
    private MapTileTypeData buildTypeData_Map0 = new MapTileTypeData();
    private MapTileTypeData floorTypeData_Map0 = new MapTileTypeData();
    /*地图模板1*/
    private MapTileInfoData buildInfoData_Map1 = new MapTileInfoData();
    private MapTileTypeData buildTypeData_Map1 = new MapTileTypeData();
    private MapTileTypeData floorTypeData_Map1 = new MapTileTypeData();
    /*地图模板2*/
    private MapTileInfoData buildInfoData_Map2 = new MapTileInfoData();
    private MapTileTypeData buildTypeData_Map2 = new MapTileTypeData();
    private MapTileTypeData floorTypeData_Map2 = new MapTileTypeData();

    private Action createAction;
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

        buildInfoData_Map0 = JsonConvert.DeserializeObject<MapTileInfoData>(FileManager.Instance.ReadFile("MapModle/MapBuildInfoData_0"));
        buildTypeData_Map0 = JsonConvert.DeserializeObject<MapTileTypeData>(FileManager.Instance.ReadFile("MapModle/MapBuildTypeData_0"));
        floorTypeData_Map0 = JsonConvert.DeserializeObject<MapTileTypeData>(FileManager.Instance.ReadFile("MapModle/MapFloorTypeData_0"));

        buildInfoData_Map1 = JsonConvert.DeserializeObject<MapTileInfoData>(FileManager.Instance.ReadFile("MapModle/MapBuildInfoData_1"));
        buildTypeData_Map1 = JsonConvert.DeserializeObject<MapTileTypeData>(FileManager.Instance.ReadFile("MapModle/MapBuildTypeData_1"));
        floorTypeData_Map1 = JsonConvert.DeserializeObject<MapTileTypeData>(FileManager.Instance.ReadFile("MapModle/MapFloorTypeData_1"));

        buildInfoData_Map2 = JsonConvert.DeserializeObject<MapTileInfoData>(FileManager.Instance.ReadFile("MapModle/MapBuildInfoData_2"));
        buildTypeData_Map2 = JsonConvert.DeserializeObject<MapTileTypeData>(FileManager.Instance.ReadFile("MapModle/MapBuildTypeData_2"));
        floorTypeData_Map2 = JsonConvert.DeserializeObject<MapTileTypeData>(FileManager.Instance.ReadFile("MapModle/MapFloorTypeData_2"));
    
    }
    public void Init(string buildInfoPath, string buildTypePath, string floorTypePath, Action create)
    {
        _buildInfoPath = buildInfoPath;
        _buildTypePath = buildTypePath;
        _FloorTypePath = floorTypePath;
        createAction = create;
        GetRandomSeed();
    }
    public void Show()
    {
        panel_Main.gameObject.SetActive(true);
    }
    public void Hide()
    {
        panel_Main.gameObject.SetActive(false);
    }
    public void ChangeSeed(string seed)
    {
        _mapSeed = seed.ToString();
    }
    public void ChangeName(string name)
    {
        _mapName = name.ToString();
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
        _mapSeed = strRan.ToString();
        input_MapSeed.text = _mapSeed;
    }

    public void CreateMapData()
    {
        SetMapData();
        FileManager.Instance.WriteFile(_buildInfoPath, JsonConvert.SerializeObject(buildInfoData));
        FileManager.Instance.WriteFile(_buildTypePath, JsonConvert.SerializeObject(buildTypeData));
        FileManager.Instance.WriteFile(_FloorTypePath, JsonConvert.SerializeObject(floorTypeData));
        if (createAction != null)
        {
            createAction.Invoke();
        }
    }
    public void SetMapData()
    {
        switch (dropdown_MapType.value)
        {
            case 0:/*荒地*/
                buildInfoData = buildInfoData_Map0;
                buildTypeData = buildTypeData_Map0;
                floorTypeData = floorTypeData_Map0;
                break;
            case 1:/*无人小镇*/
                buildInfoData = buildInfoData_Map1;
                buildTypeData = buildTypeData_Map1;
                floorTypeData = floorTypeData_Map1;
                break;
            case 2:/*荒岛*/
                buildInfoData = buildInfoData_Map2;
                buildTypeData = buildTypeData_Map2;
                floorTypeData = floorTypeData_Map2;
                break;
        }
        if (buildInfoData == null) { buildInfoData = new MapTileInfoData(); }
        if (buildTypeData == null) { buildTypeData = new MapTileTypeData(); }
        if (floorTypeData == null) { floorTypeData = new MapTileTypeData(); }
        buildInfoData.name = _mapName;
        buildInfoData.seed = _mapSeed;
    }
}
