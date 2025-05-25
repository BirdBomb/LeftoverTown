using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using static Fusion.Allocator;

public class MapPreviewManager : SingleTon<MapPreviewManager>, ISingleTon
{
    [Header("‘§¿¿Œª÷√")]
    public Transform transform_Preview;
    [Header("‘§¿¿Õº∆¨")]
    public SpriteRenderer spriteRenderer_Preview;
    [Header("Ω®÷˛‘§¿¿ÕººØ")]
    public SpriteAtlas spriteAtlas_BuildingPreview;
    [Header("µÿ√Ê‘§¿¿ÕººØ")]
    public SpriteAtlas spriteAtlas_GroundPreview;
    /// <summary>
    /// ’˝‘⁄‘§¿¿
    /// </summary>
    private bool bool_InPreview = false;
    /// <summary>
    /// µ±«∞‘§¿¿Œª÷√
    /// </summary>
    private Vector3 vector3_CurPreviewPos;
    /// <summary>
    /// …œ¥Œ‘§¿¿◊¯±Í
    /// </summary>
    private Vector3Int vector3Int_LastPreviewPos;
    /// <summary>
    /// µ±«∞‘§¿¿◊¯±Í
    /// </summary>
    private Vector3Int vector3Int_CurPreviewPos;
    [Header("‘§¿¿UI"), SerializeField]
    private Transform tran_PreviewUI;
    [Header("‘§¿¿”“ª˜UI"), SerializeField]
    private Transform tran_Preview_RightClickUI;
    [Header("‘§¿¿◊Ûª˜UI"), SerializeField]
    private Transform tran_Preview_LeftClickUI;
    [Header("‘§¿¿÷–ª˜UI"), SerializeField]
    private Transform tran_Preview_MiddleClickUI;
    public enum BuildState
    {
        Sleep,
        ForceBuildBuilding,
        ForceBuildFloor,
        TryBuildBuilding,
        TryBuildFloor,
    }

    public void Init()
    {
        
    }
    public void Update()
    {
        if (bool_InPreview)
        {
            UpdatePos();
            if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Build();
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Close();
            }
            if (buildState_Target == BuildState.ForceBuildBuilding || buildState_Target == BuildState.TryBuildBuilding)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    int_GroupIndex += 1;
                    int_GroupIndex = Mathf.Abs(int_GroupIndex);
                    UpdateSprite();
                }
                if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    int_GroupIndex -= 1;
                    int_GroupIndex = Mathf.Abs(int_GroupIndex);
                    UpdateSprite();
                }
            }
        }

    }
    /*Ω®÷˛‘§¿¿*/
    #region
    private BuildingConfig buildingConfig_Target;
    private GroundConfig groundConfig_Target;
    private BuildState buildState_Target = BuildState.Sleep;
    private bool bool_GetAllRaw = false;
    private int int_GroupIndex = 0;
    public void Init(BuildingConfig buildingConfig, BuildState buildState)
    {
        buildingConfig_Target = buildingConfig;
        buildState_Target = buildState;
        int_GroupIndex = 0;
        Open();
        CheckRow();
        UpdateSprite();
    }
    public void Init(GroundConfig groundConfig, BuildState buildState)
    {
        groundConfig_Target = groundConfig;
        buildState_Target = buildState;
        int_GroupIndex = 0;
        Open();
        CheckRow();
        UpdateSprite();
    }
    private void UpdateSprite()
    {
        if (buildState_Target == BuildState.ForceBuildBuilding || buildState_Target == BuildState.TryBuildBuilding)
        {
            if (buildingConfig_Target.Building_Group != null && buildingConfig_Target.Building_Group.Count > 0)
            {
                int index = int_GroupIndex % buildingConfig_Target.Building_Group.Count;
                spriteRenderer_Preview.sprite = spriteAtlas_BuildingPreview.GetSprite(buildingConfig_Target.Building_Group[index].ToString());
            }
            else
            {
                spriteRenderer_Preview.sprite = spriteAtlas_BuildingPreview.GetSprite(buildingConfig_Target.Building_ID.ToString());
            }
        }
        if (buildState_Target == BuildState.ForceBuildFloor || buildState_Target == BuildState.TryBuildFloor)
        {
            spriteRenderer_Preview.sprite = spriteAtlas_GroundPreview.GetSprite(groundConfig_Target.Ground_ID.ToString());
        }
    }
    private void UpdatePos()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vector3_CurPreviewPos = pos;
        vector3Int_CurPreviewPos = MapManager.Instance.tilemap_Building.WorldToCell(pos);
        if (vector3Int_LastPreviewPos != vector3Int_CurPreviewPos)
        {
            vector3Int_LastPreviewPos = vector3Int_CurPreviewPos;
            transform_Preview.position = vector3Int_LastPreviewPos;
            if (bool_GetAllRaw)
            {
                UpdateColor(!CheckPostion(pos));
            }
            else { UpdateColor(true); }
        }
    }
    private void UpdateColor(bool red)
    {
        if (red)
        {
            spriteRenderer_Preview.color = new Color(1, 0, 0, 0.5f);
        }
        else
        {
            spriteRenderer_Preview.color = new Color(1, 1, 1, 0.5f);
        }
    }
    public void Build()
    {
        if (bool_InPreview)
        {
            spriteRenderer_Preview.color = new Color(1, 0, 0, 0.5f);
            switch (buildState_Target)
            {
                case BuildState.ForceBuildBuilding:
                    PlayBuild(true);
                    if (buildingConfig_Target.Building_Group != null && buildingConfig_Target.Building_Group.Count > 0)
                    {
                        int index = int_GroupIndex % buildingConfig_Target.Building_Group.Count;
                        BuildingConfig config = BuildingConfigData.GetBuildingConfig(buildingConfig_Target.Building_Group[index]);
                        MessageBroker.Default.Publish(new MapEvent.MapEvent_Local_ChangeBuildingArea()
                        {
                            buildingID = config.Building_ID,
                            buildingPos = vector3Int_CurPreviewPos,
                            areaSize = config.Building_Size,
                        });
                    }
                    else
                    {
                        MessageBroker.Default.Publish(new MapEvent.MapEvent_Local_ChangeBuildingArea()
                        {
                            buildingID = buildingConfig_Target.Building_ID,
                            buildingPos = vector3Int_CurPreviewPos,
                            areaSize = buildingConfig_Target.Building_Size,
                        });
                    }
                    break;
                case BuildState.ForceBuildFloor:
                    PlayBuild(true);
                    MessageBroker.Default.Publish(new MapEvent.MapEvent_Local_ChangeGround()
                    {
                        groundID = groundConfig_Target.Ground_ID,
                        groundPos = vector3Int_CurPreviewPos,
                    });
                    break;
                case BuildState.TryBuildBuilding:

                    if (CheckPostion(vector3_CurPreviewPos) && CheckRaw(buildingConfig_Target.Building_Raw))
                    {
                        PlayBuild(true);
                        ExpenRaw(buildingConfig_Target.Building_Raw);
                        if (buildingConfig_Target.Building_Group != null && buildingConfig_Target.Building_Group.Count > 0)
                        {
                            int index = int_GroupIndex % buildingConfig_Target.Building_Group.Count;
                            BuildingConfig config = BuildingConfigData.GetBuildingConfig(buildingConfig_Target.Building_Group[index]);
                            MessageBroker.Default.Publish(new MapEvent.MapEvent_Local_ChangeBuildingArea()
                            {
                                buildingID = config.Building_ID,
                                buildingPos = vector3Int_CurPreviewPos,
                                areaSize = config.Building_Size,
                            });
                        }
                        else
                        {
                            MessageBroker.Default.Publish(new MapEvent.MapEvent_Local_ChangeBuildingArea()
                            {
                                buildingID = buildingConfig_Target.Building_ID,
                                buildingPos = vector3Int_CurPreviewPos,
                                areaSize = buildingConfig_Target.Building_Size,
                            });
                        }
                    }
                    else
                    {
                        PlayBuild(false);
                    }
                    break;
                case BuildState.TryBuildFloor:
                    if (CheckPostion(vector3_CurPreviewPos) && CheckRaw(groundConfig_Target.Ground_Raw))
                    {
                        PlayBuild(true);
                        ExpenRaw(groundConfig_Target.Ground_Raw);
                        MessageBroker.Default.Publish(new MapEvent.MapEvent_Local_ChangeGround()
                        {
                            groundID = groundConfig_Target.Ground_ID,
                            groundPos = vector3Int_CurPreviewPos,
                        });
                    }
                    else
                    {
                        PlayBuild(false);
                    }

                    break;
            }
        }
    }
    public void Open()
    {
        bool_InPreview = true;
        spriteRenderer_Preview.gameObject.SetActive(true);
        CursorManager.Instance.AddCursor(CursorManager.CursorType.Build);
    }
    public void Close()
    {
        bool_InPreview = false;
        buildState_Target = BuildState.Sleep;
        spriteRenderer_Preview.gameObject.SetActive(false);
        CursorManager.Instance.SubCursor(CursorManager.CursorType.Build);
    }

    /// <summary>
    /// ºÏ≤ÈŒª÷√
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private bool CheckPostion(Vector3 pos)
    {
        bool temp = false;
        if (buildState_Target == BuildState.ForceBuildFloor || buildState_Target == BuildState.ForceBuildBuilding)
        {
            temp = true;
        }
        else
        {
            if (buildState_Target == BuildState.TryBuildBuilding)
            {
                if (buildingConfig_Target.Building_Group != null && buildingConfig_Target.Building_Group.Count > 0)
                {
                    int index = int_GroupIndex % buildingConfig_Target.Building_Group.Count;
                    BuildingConfig config = BuildingConfigData.GetBuildingConfig(buildingConfig_Target.Building_Group[index]);
                    if (MapManager.Instance.CheckBuildingEmpty(pos, config.Building_Size))
                    {
                        return true;
                    }
                }
                else
                {
                    if (MapManager.Instance.CheckBuildingEmpty(pos, buildingConfig_Target.Building_Size))
                    {
                        return true;
                    }
                }
            }
            if (buildState_Target == BuildState.TryBuildFloor)
            {
                if (MapManager.Instance.CheckBuildingEmpty(pos, AreaSize._1X1))
                {
                    return true;
                }
            }
        }
        return temp;
    }
    /// <summary>
    /// ºÏ≤È≤ƒ¡œ
    /// </summary>
    /// <returns></returns>
    private void CheckRow()
    {
        if (buildState_Target == BuildState.ForceBuildBuilding || buildState_Target == BuildState.ForceBuildFloor)
        {
            bool_GetAllRaw = true;
        }
        else
        {
            if (buildState_Target == BuildState.TryBuildBuilding)
            {
                bool_GetAllRaw = CheckRaw(buildingConfig_Target.Building_Raw);
            }
            if (buildState_Target == BuildState.TryBuildFloor)
            {
                bool_GetAllRaw = CheckRaw(groundConfig_Target.Ground_Raw);
            }
        }
    }
    /// <summary>
    /// ºÏ≤È≤ƒ¡œ
    /// </summary>
    /// <param name="raws"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    private bool CheckRaw(List<ItemRaw> raws)
    {
        bool temp = true;
        List<ItemData> data = GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Local_GetBagItem();
        for (int i = 0; i < raws.Count; i++)
        {
            int itemCount = 0;
            for (int j = 0; j < data.Count; j++)
            {
                if (data[j].Item_ID == raws[i].ID)
                {
                    itemCount += data[j].Item_Count;
                }
            }
            if (itemCount < raws[i].Count)
            {
                temp = false;
            }
        }
        return temp;
    }
    /// <summary>
    /// œ˚∫ƒ≤ƒ¡œ
    /// </summary>
    /// <param name="raws"></param>
    private void ExpenRaw(List<ItemRaw> raws)
    {
        for (int i = 0; i < raws.Count; i++)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryExpendItemInBag()
            {
                itemID = raws[i].ID,
                itemCount = raws[i].Count,
            });
        }

    }
    private void PlayBuild(bool succes)
    {
        if (succes)
        {
            spriteRenderer_Preview.transform.DOKill();
            spriteRenderer_Preview.transform.localScale = Vector3.one;
            spriteRenderer_Preview.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 1), 0.5f);
            AudioManager.Instance.Play2DEffect(1004);
        }
        else
        {
            spriteRenderer_Preview.transform.DOKill();
            spriteRenderer_Preview.transform.localScale = Vector3.one;
            spriteRenderer_Preview.transform.DOShakePosition(0.2f, new Vector3(0.1f, 0.1f, 1), 100, 500);
            AudioManager.Instance.Play2DEffect(1001);
        }
    }
    #endregion
}
