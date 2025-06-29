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
    [Header("预览位置")]
    public Transform transform_Preview;
    [Header("预览图片")]
    public SpriteRenderer spriteRenderer_Preview;
    [Header("标识位置")]
    public Transform transform_Singal;
    [Header("标识图片")]
    public SpriteRenderer spriteRenderer_Singal;
    [Header("建筑预览图集")]
    public SpriteAtlas spriteAtlas_BuildingPreview;
    [Header("地面预览图集")]
    public SpriteAtlas spriteAtlas_GroundPreview;
    /// <summary>
    /// 正在预览
    /// </summary>
    private bool bool_InPreview = false;
    /// <summary>
    /// 正在标识位置
    /// </summary>
    private bool bool_InSingal = false;
    /// <summary>
    /// 当前预览位置
    /// </summary>
    private Vector3 vector3_CurPreviewPos;
    /// <summary>
    /// 上次预览坐标
    /// </summary>
    private Vector3Int vector3Int_LastPreviewPos;
    /// <summary>
    /// 当前预览坐标
    /// </summary>
    private Vector3Int vector3Int_CurPreviewPos;
    [Header("预览UI"), SerializeField]
    private Transform tran_PreviewUI;
    [Header("预览右击UI"), SerializeField]
    private Transform tran_Preview_RightClickUI;
    [Header("预览左击UI"), SerializeField]
    private Transform tran_Preview_LeftClickUI;
    [Header("预览中击UI"), SerializeField]
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
        MessageBroker.Default.Receive<GameEvent.GameEvent_AllClient_SomeoneMove>().Subscribe(_ =>
        {
            if (_.moveActor.actorAuthority.isLocal && _.moveActor.actorAuthority.isPlayer)
            {
                UpdateSingalPos(_.movePos);
            }
        }).AddTo(this);
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
    /*建筑预览*/
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
    /// 检查位置
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
    /// 检查材料
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
    /// 检查材料
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
    /// 消耗材料
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
    /*地图高亮*/
    #region
    public Vector3Int vector3Int_SingalOffset;
    public Vector3Int vector3Int_SingalPos;
    private void UpdateSingalPos(Vector3Int pos)
    {
        vector3Int_SingalPos = pos;
        transform_Singal.position = MapManager.Instance.tilemap_Building.CellToWorld(vector3Int_SingalPos + vector3Int_SingalOffset) + new Vector3(0.5f, 0.5f, 0);
        transform_Singal.DOKill();
        transform_Singal.localScale = Vector3.one;
        transform_Singal.DOPunchScale(new Vector3(0.1f, 0.1f, 0), 0.2f);
        spriteRenderer_Singal.DOKill();
        spriteRenderer_Singal.color = new Color(1, 1, 1, 0.8f);
        spriteRenderer_Singal.DOFade(0f, 1);
    }
    public void ShowSingal(Vector3Int offset)
    {
        vector3Int_SingalOffset = offset;
        UpdateSingalPos(vector3Int_SingalPos);
        bool_InSingal = true;
        spriteRenderer_Singal.gameObject.SetActive(true);
    }
    public void HideSingal()
    {
        bool_InSingal = false;
        if(spriteRenderer_Singal != null)
        {
            spriteRenderer_Singal.gameObject.SetActive(false);
        }
    }
    public void SuccesSingal()
    {

    }
    public void FailSingal(float val)
    {
        spriteRenderer_Singal.DOComplete();
        spriteRenderer_Singal.color = new Color(1, 0, 0, 0.8f);
        spriteRenderer_Singal.DOFade(0f, 1);
        spriteRenderer_Singal.transform.DOShakePosition(0.2f, new Vector3(0.05f, 0.05f, 1), 100, 200);
        AudioManager.Instance.Play2DEffect(1001);
    }
    #endregion
}
