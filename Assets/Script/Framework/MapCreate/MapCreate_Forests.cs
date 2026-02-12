using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class MapCreate_Forests 
{
    MapCreate bindMapCreater;
    /// <summary>
    /// 森林之间距离
    /// </summary>
    private int forest_Distance = 60;
    /// <summary>
    /// 森林最小面积
    /// </summary>
    private int forest_MinSize = 20;
    /// <summary>
    /// 森林最大面积
    /// </summary>
    private int forest_MaxSize = 40;
    /// <summary>
    /// 森林树木密度(平方米/个)
    /// </summary>
    private float forest_Compactness = 5f;
    /*------------草地-------------*/
    /// <summary>
    /// 草地树木种类
    /// </summary>
    private List<short> treeIDs_Ground1001 = new List<short>() { 1000, 1003, 1022 };
    /// <summary>
    /// 雪地树木种类
    /// </summary>
    private List<short> treeIDs_Ground1004 = new List<short>() { 1018 };
    /// <summary>
    /// 沙漠树木种类
    /// </summary>
    private List<short> treeIDs_Ground1005 = new List<short>() { 1019 };

    private System.Random random_Temp = new System.Random();
    /// <summary>
    /// 生成树木
    /// </summary>
    /// <returns></returns>
    public async Task CreateForest(MapCreate mapCreater)
    {
        random_Temp = new System.Random(mapCreater.GetAccIndex() + mapCreater.seed_Offset);

        bindMapCreater = mapCreater;
        List<Vector2Int> points = new List<Vector2Int>();
        bindMapCreater.text_Waiting.text = "正在铺设树林";
        MapCreate.PoissonPointsAreaConfig config = new MapCreate.PoissonPointsAreaConfig()
        {
            pointsArea_Center = Vector2Int.zero,
            pointsArea_Size = bindMapCreater.config_Map.map_Size,
            pointsArea_MinDistance = forest_Distance,
            pointsArea_MaxSamples = 20,
        };
        await bindMapCreater.GeneratePoissonPointsAsync(config, (point) =>
        {
            points.Add(point);
        });

        for (int i = 0; i < points.Count; i++)
        {
            bindMapCreater.text_Waiting.text = "正在生成树林" + i + "/" + points.Count;
            await CreateTree(points[i]);
        }
    }
    public async Task CreateTree(Vector2Int center)
    {
        int forest_Size = random_Temp.Next(forest_MinSize, forest_MaxSize);
        MapCreate.RandomPointsAreaConfig config = new MapCreate.RandomPointsAreaConfig()
        {
            pointsArea_Center = center,
            pointsArea_Count = (int)(forest_Size * forest_Size / forest_Compactness),
            pointsArea_SizeWhole = forest_Size,
            pointsArea_SizeFill = forest_Size - 10,
        };
        await bindMapCreater.GenerateRandomPointsArea_Gradual(config, (index) =>
        {
            if (bindMapCreater.data_mapGroundData.tileDic.ContainsKey(index))
            {
                if (bindMapCreater.data_mapGroundData.tileDic[index] == 1001)
                {
                    short treeID = treeIDs_Ground1001[random_Temp.Next(0, treeIDs_Ground1001.Count)];
                    if (bindMapCreater.data_mapBuildingData.tileDic.TryAdd(index, treeID))
                    {
                        bindMapCreater.data_mapBuildingData.tileDic[index] = treeID;
                    }
                }
                else if (bindMapCreater.data_mapGroundData.tileDic[index] == 1003)
                {
                    short treeID = treeIDs_Ground1004[random_Temp.Next(0, treeIDs_Ground1004.Count)];
                    if (bindMapCreater.data_mapBuildingData.tileDic.TryAdd(index, treeID))
                    {
                        bindMapCreater.data_mapBuildingData.tileDic[index] = treeID;
                    }
                }
                else if (bindMapCreater.data_mapGroundData.tileDic[index] == 1004)
                {
                    short treeID = treeIDs_Ground1004[random_Temp.Next(0, treeIDs_Ground1004.Count)];
                    if (bindMapCreater.data_mapBuildingData.tileDic.TryAdd(index, treeID))
                    {
                        bindMapCreater.data_mapBuildingData.tileDic[index] = treeID;
                    }
                }
                else if (bindMapCreater.data_mapGroundData.tileDic[index] == 1005)
                {
                    short treeID = treeIDs_Ground1005[random_Temp.Next(0, treeIDs_Ground1005.Count)];
                    if (bindMapCreater.data_mapBuildingData.tileDic.TryAdd(index, treeID))
                    {
                        bindMapCreater.data_mapBuildingData.tileDic[index] = treeID;
                    }
                }
            }
        });
    }
}
