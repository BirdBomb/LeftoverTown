using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateListConfigData
{
    public static CreateListConfig GetCreateListConfig(int ID)
    {
        return createListConfigs.Find((x) => { return x.ID == ID; });
    }
    public readonly static List<CreateListConfig> createListConfigs = new List<CreateListConfig>()
    {
        new CreateListConfig()
        {
            ID = 1000,
            Name = "木制工具台",
            List = new List<int> { 1100, 2000, 2010, 2020, 2030, 2100, 2200, 5001, 5007, 5105, 9000}
        },
        new CreateListConfig()
        {
            ID = 1001,
            Name = "铁制工具台",
            List = new List<int> { 1200, 2011, 2021, 2101, 2102, 2103, 2201, 2202, 5005, 5104 }
        },
        new CreateListConfig()
        {
            ID = 1002,
            Name = "枪械工具台",
            List = new List<int> { 2300, 2301, 2302, 2303, 2304, 2305, 2306, 9010}
        },
    };
}
public struct CreateListConfig
{
    public int ID;
    public string Name;
    public List<int> List;
}