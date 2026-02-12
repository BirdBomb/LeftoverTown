using DG.Tweening;
using System;
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
    public enum BuildState
    {
        Sleep,
        /// <summary>
        /// 强制建造建筑
        /// </summary>
        ForceBuildBuilding,
        /// <summary>
        /// 强制建造地板
        /// </summary>
        ForceBuildFloor,
        /// <summary>
        /// 试图建造建筑
        /// </summary>
        TryBuildBuilding,
        /// <summary>
        /// 试图建造地板
        /// </summary>
        TryBuildFloor,
    }
    public void Init()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_AllClient_SomeoneMove>().Subscribe(_ =>
        {
            if (_.moveActor.actorAuthority.isLocal && _.moveActor.actorAuthority.isPlayer)
            {
                Local_SetPlayerPos(_.movePos);
                if (bool_InPreview)
                {
                    bool_GetEmpty = Local_CheckPostion(vector3Int_CurPreviewPos);
                    Local_UpdatePreviewColor(bool_GetEmpty && bool_GetAllRaw);
                    Local_UpdateBuildRangePos(); 
                }
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateItemInBag>().Subscribe(_ =>
        {
            if (bool_InPreview)
            {
                bool_GetAllRaw = Local_CheckRaw();
                Local_UpdatePreviewColor(bool_GetEmpty && bool_GetAllRaw);
            }
        }).AddTo(this);
    }
    public void Update()
    {
        if (bool_InPreview)
        {
            Local_UpdatePreviewPos();
            if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Local_TryBuild();
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Local_ClosePreview();
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (buildState_Target == BuildState.ForceBuildBuilding || buildState_Target == BuildState.TryBuildBuilding)
                {
                    int_GroupIndex += 1;
                    int_GroupIndex = Mathf.Abs(int_GroupIndex);
                    Local_UpdatePreviewSprite();
                }
            }
        }

    }
    /*建筑预览*/
    #region
    [Header("建筑预览图集")]
    public SpriteAtlas spriteAtlas_BuildingPreview;
    [Header("地面预览图集")]
    public SpriteAtlas spriteAtlas_GroundPreview;
    [Header("预览图片")]
    public SpriteRenderer spriteRenderer_Preview;
    [Header("预览位置")]
    public Transform transform_Preview;
    [Header("建筑禁止建造图片")]
    public List<SpriteRenderer> spriteRenderer_BuildBlocks = new List<SpriteRenderer>();
    [Header("建筑范围图片")]
    public SpriteRenderer spriteRenderer_BuildRange;
    [Header("建筑范围位置")]
    public Transform transform_BuildRange;
    [Header("预览Tab")]
    public SpriteRenderer spriteRenderer_Tab;
    /// <summary>
    /// 上次预览坐标
    /// </summary>
    private Vector3Int vector3Int_LastPreviewPos;
    /// <summary>
    /// 当前预览坐标
    /// </summary>
    private Vector3Int vector3Int_CurPreviewPos;

    private BuildingConfig buildingConfig_Target;
    private GroundConfig groundConfig_Target;
    private BuildState buildState_Target = BuildState.Sleep;
    private bool bool_GetAllRaw = false;
    private bool bool_GetEmpty = false;
    private int int_GroupIndex = 0;
    /// <summary>
    /// 正在预览
    /// </summary>
    private bool bool_InPreview = false;

    public void Local_Init(BuildingConfig buildingConfig, BuildState buildState)
    {
        buildingConfig_Target = buildingConfig;
        int_GroupIndex = 0;
        Local_OpenPreview(buildState);
    }
    public void Local_Init(GroundConfig groundConfig, BuildState buildState)
    {
        groundConfig_Target = groundConfig;
        int_GroupIndex = 0;
        Local_OpenPreview(buildState);
    }
    private void Local_UpdateTab()
    {
        if (buildState_Target == BuildState.ForceBuildBuilding || buildState_Target == BuildState.TryBuildBuilding)
        {
            if (buildingConfig_Target.Building_Group != null && buildingConfig_Target.Building_Group.Count > 0)

            {
                spriteRenderer_Tab.gameObject.SetActive(true);
                return;
            }
        };
        spriteRenderer_Tab.gameObject.SetActive(false);
    }
    private void Local_UpdatePreviewSprite()
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
    /// <summary>
    /// 更改预览位置
    /// </summary>
    private void Local_UpdatePreviewPos()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vector3Int_CurPreviewPos = MapManager.Instance.tilemap_Building.WorldToCell(pos);
        if (vector3Int_LastPreviewPos != vector3Int_CurPreviewPos)
        {
            vector3Int_LastPreviewPos = vector3Int_CurPreviewPos;
            transform_Preview.position = vector3Int_LastPreviewPos;
            bool_GetEmpty = Local_CheckPostion(vector3Int_CurPreviewPos);
            Local_UpdatePreviewColor(bool_GetEmpty && bool_GetAllRaw);
        }
    }
    /// <summary>
    /// 更改建筑范围位置
    /// </summary>
    private void Local_UpdateBuildRangePos()
    {
        transform_BuildRange.position = vector3Int_PlayerPos;
        spriteRenderer_BuildRange.transform.DOKill();
        spriteRenderer_BuildRange.transform.localScale = Vector3.one;
        spriteRenderer_BuildRange.transform.DOPunchScale(new Vector3(0.02f, 0.02f, 1), 0.5f);
        Local_DrawBuildRange();
    }
    /// <summary>
    /// 绘制建筑范围
    /// </summary>
    private void Local_DrawBuildRange()
    {
        int index = 0;
        for(int i = -2; i <= 2; i++)
        {
            for (int j = -2; j <= 2; j++)
            {
                if (Local_CheckPostion(vector3Int_PlayerPos + new Vector3Int(j, i)))
                {
                    spriteRenderer_BuildBlocks[index].gameObject.SetActive(false);
                }
                else
                {
                    spriteRenderer_BuildBlocks[index].gameObject.SetActive(true);
                }
                index++;
            }
        }
    }
    private void Local_LoopBuildRange()
    {
        spriteRenderer_BuildRange.color = new Color(1, 1, 1, 0.5f);
        spriteRenderer_BuildRange.DOFade(0.2f, 2).SetLoops(-1, LoopType.Yoyo);
    }
    private void Local_UpdatePreviewColor(bool white)
    {
        if (white)
        {
            spriteRenderer_Preview.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            spriteRenderer_Preview.color = new Color(1, 0, 0, 0.5f);
        }
    }
    public void Local_TryBuild()
    {
        if (bool_InPreview)
        {
            spriteRenderer_Preview.color = new Color(1, 0, 0, 0.5f);
            switch (buildState_Target)
            {
                case BuildState.ForceBuildBuilding:
                    Local_PlayBuild(true);
                    BuildingConfig target0 = buildingConfig_Target;
                    if (buildingConfig_Target.Building_Group != null && buildingConfig_Target.Building_Group.Count > 0)
                    {
                        int index = int_GroupIndex % buildingConfig_Target.Building_Group.Count;
                        target0 = BuildingConfigData.GetBuildingConfig(buildingConfig_Target.Building_Group[index]);
                    }
                    MessageBroker.Default.Publish(new MapEvent.MapEvent_Local_CreateBuildingArea()
                    {
                        buildingID = target0.Building_ID,
                        buildingPos = vector3Int_CurPreviewPos,
                        areaSize = target0.Building_Size,
                    });
                    break;
                case BuildState.ForceBuildFloor:
                    Local_PlayBuild(true);
                    MessageBroker.Default.Publish(new MapEvent.MapEvent_Local_CreateGround()
                    {
                        groundID = groundConfig_Target.Ground_ID,
                        groundPos = vector3Int_CurPreviewPos,
                    });
                    break;
                case BuildState.TryBuildBuilding:
                    if (bool_GetEmpty && bool_GetAllRaw)
                    {
                        Local_PlayBuild(true);
                        Local_ExpenRaw(buildingConfig_Target.Building_Raw);
                        BuildingConfig target_1 = buildingConfig_Target;
                        if (buildingConfig_Target.Building_Group != null && buildingConfig_Target.Building_Group.Count > 0)
                        {
                            int index = int_GroupIndex % buildingConfig_Target.Building_Group.Count;
                            target_1 = BuildingConfigData.GetBuildingConfig(buildingConfig_Target.Building_Group[index]);
                        }
                        MessageBroker.Default.Publish(new MapEvent.MapEvent_Local_CreateBuildingArea()
                        {
                            buildingID = target_1.Building_ID,
                            buildingPos = vector3Int_CurPreviewPos,
                            areaSize = target_1.Building_Size,
                        });

                    }
                    else
                    {
                        Local_PlayBuild(false);
                    }
                    break;
                case BuildState.TryBuildFloor:
                    if (bool_GetEmpty && bool_GetAllRaw)
                    {
                        Local_PlayBuild(true);
                        Local_ExpenRaw(groundConfig_Target.Ground_Raw);
                        MessageBroker.Default.Publish(new MapEvent.MapEvent_Local_CreateGround()
                        {
                            groundID = groundConfig_Target.Ground_ID,
                            groundPos = vector3Int_CurPreviewPos,
                        });
                    }
                    else
                    {
                        Local_PlayBuild(false);
                    }
                    break;
            }
        }
    }
    public void Local_OpenPreview(BuildState buildState)
    {
        bool_InPreview = true;
        spriteRenderer_Preview.gameObject.SetActive(true);
        spriteRenderer_BuildRange.gameObject.SetActive(true);
        CursorManager.Instance.AddCursor(CursorManager.CursorType.Build);

        buildState_Target = buildState;

        Local_UpdateBuildRangePos();
        Local_UpdateTab();
        bool_GetAllRaw = Local_CheckRaw();
        bool_GetEmpty = Local_CheckPostion(vector3Int_CurPreviewPos);
        Local_UpdatePreviewColor(bool_GetEmpty && bool_GetAllRaw);
        Local_UpdatePreviewSprite();
    }
    public void Local_ClosePreview()
    {
        bool_InPreview = false;
        buildState_Target = BuildState.Sleep;
        spriteRenderer_Preview.gameObject.SetActive(false);
        spriteRenderer_BuildRange.gameObject.SetActive(false);
        CursorManager.Instance.SubCursor(CursorManager.CursorType.Build);
    }

    /// <summary>
    /// 检查位置
    /// </summary>
    /// <param name="pos"></param>
    /// <returns>是否可以建造</returns>
    private bool Local_CheckPostion(Vector3Int pos)
    {
        bool temp = false;
        if (bool_InPreview)
        {
            if (buildState_Target == BuildState.Sleep) return temp;
            else if (buildState_Target == BuildState.ForceBuildFloor || buildState_Target == BuildState.ForceBuildBuilding)
            {
                temp = true;
            }
            else
            {
                if (Vector3.Distance(vector3Int_PlayerPos, pos) > 2.5)
                {
                    return false;
                }
                if (buildState_Target == BuildState.TryBuildBuilding)
                {
                    if (buildingConfig_Target.Building_Group != null && buildingConfig_Target.Building_Group.Count > 0)
                    {
                        int index = int_GroupIndex % buildingConfig_Target.Building_Group.Count;
                        BuildingConfig config = BuildingConfigData.GetBuildingConfig(buildingConfig_Target.Building_Group[index]);
                        if (MapManager.Instance.CheckGround(pos, config.Building_Size) && MapManager.Instance.CheckBuildingEmpty(pos, config.Building_Size))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (MapManager.Instance.CheckGround(pos, buildingConfig_Target.Building_Size) && MapManager.Instance.CheckBuildingEmpty(pos, buildingConfig_Target.Building_Size))
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
        }
        return temp;
    }
    /// <summary>
    /// 检查材料
    /// </summary>
    /// <returns></returns>
    private bool Local_CheckRaw()
    {
        if (buildState_Target == BuildState.ForceBuildBuilding || buildState_Target == BuildState.ForceBuildFloor)
        {
            return  true;
        }
        else
        {
            if (buildState_Target == BuildState.TryBuildBuilding)
            {
                return Local_CheckRaw(buildingConfig_Target.Building_Raw);
            }
            if (buildState_Target == BuildState.TryBuildFloor)
            {
                return Local_CheckRaw(groundConfig_Target.Ground_Raw);
            }
            return false;
        }
    }
    /// <summary>
    /// 检查材料
    /// </summary>
    /// <param name="raws"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    private bool Local_CheckRaw(List<ItemRaw> raws)
    {
        bool temp = true;
        List<ItemData> data = WorldManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Local_ItemBag_Get();
        for (int i = 0; i < raws.Count; i++)
        {
            int itemCount = 0;
            for (int j = 0; j < data.Count; j++)
            {
                if (data[j].I == raws[i].ID)
                {
                    itemCount += data[j].C;
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
    private void Local_ExpenRaw(List<ItemRaw> raws)
    {
        for (int i = 0; i < raws.Count; i++)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Expend()
            {
                itemID = raws[i].ID,
                itemCount = raws[i].Count,
            });
        }

    }
    private void Local_PlayBuild(bool succes)
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
    /*地图标记*/
    #region
    [Header("标记图片")]
    public SpriteRenderer spriteRenderer_Singal;
    [Header("标记位置")]
    public Transform transform_Singal;
    public Vector3Int vector3Int_SingalOffset;
    public Vector3Int vector3Int_PlayerPos;
    /// <summary>
    /// 正在标识位置
    /// </summary>
    private bool bool_InSingal = false;

    /// <summary>
    /// 设置玩家位置
    /// </summary>
    /// <param name="pos"></param>
    private void Local_SetPlayerPos(Vector3Int pos)
    {
        vector3Int_PlayerPos = pos;
    }
    /// <summary>
    /// 更新标记位置
    /// </summary>
    /// <param name="pos"></param>
    private void Local_UpdateSingalPos()
    {
        transform_Singal.position = MapManager.Instance.tilemap_Building.CellToWorld(vector3Int_PlayerPos + vector3Int_SingalOffset) + new Vector3(0.5f, 0.5f, 0);
        transform_Singal.DOKill();
        transform_Singal.localScale = Vector3.one;
        transform_Singal.DOPunchScale(new Vector3(0.1f, 0.1f, 0), 0.2f);
        spriteRenderer_Singal.DOKill();
        spriteRenderer_Singal.color = new Color(1, 1, 1, 0.8f);
        spriteRenderer_Singal.DOFade(0f, 1);
    }
    /// <summary>
    /// 显示标记位置
    /// </summary>
    /// <param name="offset"></param>
    public void Local_ShowSingal(Vector3Int offset)
    {
        vector3Int_SingalOffset = offset;
        Local_UpdateSingalPos();
        bool_InSingal = true;
        spriteRenderer_Singal.gameObject.SetActive(true);
    }
    /// <summary>
    /// 隐藏标记位置
    /// </summary>
    public void Local_HideSingal()
    {
        bool_InSingal = false;
        if(spriteRenderer_Singal != null)
        {
            spriteRenderer_Singal.gameObject.SetActive(false);
        }
    }
    public void Local_SuccesSingal()
    {

    }
    public void Local_FailSingal(float val)
    {
        spriteRenderer_Singal.DOComplete();
        spriteRenderer_Singal.color = new Color(1, 0, 0, 0.8f);
        spriteRenderer_Singal.DOFade(0f, 1);
        spriteRenderer_Singal.transform.DOShakePosition(0.2f, new Vector3(0.05f, 0.05f, 1), 100, 200);
        AudioManager.Instance.Play2DEffect(1001);
    }
    #endregion
}
