using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;

public class MapCreate_Mines 
{
    MapCreate mapCreate_Bind;
    #region 矿区基底

    private readonly (float threshold, float scale)[] rockConfig = new[]
    {
        (0.3f, 25f), // 大矿区
        (0.2f, 10f), // 小矿区
    };

    public async Task CreateMining_Rock(MapCreate mapCreater)
    {
        System.Random random_Temp = new System.Random(mapCreater.GetAccIndex() + mapCreater.seed_Offset);
        mapCreate_Bind = mapCreater;
        mapCreate_Bind.text_Waiting.text = "正在铺设矿区(石头)";

        foreach (var config in rockConfig)
        {
            var evenlyConfig = new MapCreate.PerlinEvenlyConfig()
            {
                area_Center = Vector2.zero,
                area_OuterRadius = mapCreate_Bind.config_Map.map_Size,
                area_FillRadius = mapCreate_Bind.config_Map.map_Size - 100,
                noise_Offset = mapCreate_Bind.GetRandomOffset(),
                noise_Scale = config.scale,
            };

            await mapCreater.GenerateEvenlyArea(evenlyConfig, (index, perlinNoise, realNoise) =>
            {
                if (realNoise < config.threshold)
                {
                    mapCreate_Bind.data_mapGroundData.tileDic[index] = 1000;
                    mapCreate_Bind.data_mapBuildingData.tileDic[index] = 1110;
                }
            });
        }
    }

    #endregion    
    #region 稀有矿石
    private readonly Dictionary<int, (float threshold, short targetId)> miningConfig = new()
    {
        [1115] = (0.05f, 1115), // 金矿
        [1114] = (0.15f, 1114), // 铁矿
        [1113] = (0.15f, 1113), // 铜矿
        [1112] = (0.15f, 1112), // 硝石
        [1111] = (0.3f, 1111), // 煤矿
    };
    public async Task CreateMining_ColorRock(MapCreate mapCreater)
    {
        System.Random random_Temp = new System.Random(mapCreater.GetAccIndex() + mapCreater.seed_Offset);
        mapCreate_Bind = mapCreater;
        mapCreate_Bind.text_Waiting.text = "正在给矿区染色";

        // 并行执行所有矿石生成
        var tasks = miningConfig.Select(config => CreateMining(mapCreater, config.Value.threshold, config.Value.targetId));
        await Task.WhenAll(tasks);
    }
    private async Task CreateMining(MapCreate mapCreater, float threshold, short targetId)
    {
        var evenlyConfig = new MapCreate.PerlinEvenlyConfig()
        {
            area_Center = Vector2.zero,
            area_OuterRadius = mapCreate_Bind.config_Map.map_Size,
            area_FillRadius = mapCreate_Bind.config_Map.map_Size - 100,
            noise_Offset = mapCreate_Bind.GetRandomOffset(),
            noise_Scale = 10,
        };

        await mapCreater.GenerateEvenlyArea(evenlyConfig, (index, perlinNoise, realNoise) =>
        {
            if (realNoise < threshold)
            {
                var tileDic = mapCreate_Bind.data_mapBuildingData.tileDic;
                if (tileDic.TryGetValue(index, out var value) && value == 1110)
                {
                    tileDic[index] = targetId;
                }
            }
        });
    }
    #endregion
}
