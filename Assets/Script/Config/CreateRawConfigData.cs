using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ºÏ³ÉÁÐ±í
/// </summary>
public class CreateRawConfigData 
{
    public static CreateRawConfig GetCreateRawConfig(int ID)
    {
        return createRawConfigs.Find((x) => { return x.Create_TargetID == ID; });
    }
    public readonly static List<CreateRawConfig> createRawConfigs = new List<CreateRawConfig>()
    {
        /*Ä¾²Ä*/new CreateRawConfig()
        {
            Create_TargetID = 1100, Create_TargetCount = 4,
            /*Ô­Ä¾1*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1000,1) }
        },
        /*»úÐµÔª¼þ*/new CreateRawConfig()
        {
            Create_TargetID = 1200, Create_TargetCount = 2,
            /*Ìú¶§1*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1114,1) }
        },
        /*»ð¾æ*/new CreateRawConfig()
        {
            Create_TargetID = 2000, Create_TargetCount = 1,
            /*Ê÷Ö¦1,²Ý2*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,1),new ItemRaw(1002,2) }
        },
        /*Ä¾¸«×Ó*/new CreateRawConfig()
        {
            Create_TargetID = 2010, Create_TargetCount = 1,
            /*Ê÷Ö¦1,Ìú¶§4*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,2),new ItemRaw(1114, 4) }
        },
        /*Ìú¸«×Ó*/new CreateRawConfig()
        {
            Create_TargetID = 2011, Create_TargetCount = 1,
            /*Ê÷Ö¦1,Ìú¶§4*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,2),new ItemRaw(1114, 4) }
        },
        /*Ä¾¸ä×Ó*/new CreateRawConfig()
        {
            Create_TargetID = 2020, Create_TargetCount = 1,
            /*Ê÷Ö¦1,Ä¾²Ä4*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,2),new ItemRaw(1100, 4) }
        },
        /*Ìú¸ä×Ó*/new CreateRawConfig()
        {
            Create_TargetID = 2021, Create_TargetCount = 1,
            /*Ê÷Ö¦1,Ìú¶§4*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,2),new ItemRaw(1114, 4) }
        },
        /*Ä¾Óã¸Í*/new CreateRawConfig()
        {
            Create_TargetID = 2030, Create_TargetCount = 1,
            /*Ê÷Ö¦4,Ä¾²Ä4*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,4), new ItemRaw(1100, 4) }
        },
        /*Ä¾³úÍ·*/new CreateRawConfig()
        {
            Create_TargetID = 2040, Create_TargetCount = 1,
            /*Ê÷Ö¦4,Ä¾²Ä4*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,4), new ItemRaw(1100, 4) }
        },
        /*Ä¾Á­µ¶*/new CreateRawConfig()
        {
            Create_TargetID = 2050, Create_TargetCount = 1,
            /*Ê÷Ö¦4,Ä¾²Ä4*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,4), new ItemRaw(1100, 4) }
        },
        /*Ä¾é¢×Ó*/new CreateRawConfig()
        {
            Create_TargetID = 2060, Create_TargetCount = 1,
            /*Ê÷Ö¦4,Ä¾²Ä4*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,4), new ItemRaw(1100, 4) }
        },
        /*Ä¾¹÷*/new CreateRawConfig()
        {
            Create_TargetID = 2100, Create_TargetCount = 1,
            /*Ê÷Ö¦4,Ä¾²Ä4*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,4), new ItemRaw(1100, 4) }
        },
        /*ÌúÃ¬*/new CreateRawConfig()
        {
            Create_TargetID = 2101, Create_TargetCount = 1,
            /*Ê÷Ö¦2,Ìú¶§4*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,2),new ItemRaw(1114,2) }
        },
        /*ÌúØ°Ê×*/new CreateRawConfig()
        {
            Create_TargetID = 2102, Create_TargetCount = 1,
            /*Ê÷Ö¦4,Ä¾²Ä4*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,2),new ItemRaw(1110,2) }
        },
        /*Ìú½£*/new CreateRawConfig()
        {
            Create_TargetID = 2103, Create_TargetCount = 1,
            /*Ä¾²Ä2,Ìú¶§4*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,2),new ItemRaw(1114,4) }
        },
        /*Ä¾¹­*/new CreateRawConfig()
        {
            Create_TargetID = 2200, Create_TargetCount = 1,
            /*Ê÷Ö¦3,Ä¾²Ä3*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,3),new ItemRaw(1100,3) }
        },
        /*¾«ÖÂÄ¾¹­*/new CreateRawConfig()
        {
            Create_TargetID = 2201, Create_TargetCount = 1,
            /*Ê÷Ö¦3,Ä¾²Ä1,»úÐµÔª¼þ1*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,3),new ItemRaw(1110,1), new ItemRaw(1200, 1) }
        },
        /*½ð¹­*/new CreateRawConfig()
        {
            Create_TargetID = 2202, Create_TargetCount = 1,
            /*Ê÷Ö¦3,Ä¾²Ä1,½ð¶§2*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,3),new ItemRaw(1110,1), new ItemRaw(1115, 2) }
        },
        /*ÍÁÖÊÊÖÇ¹*/new CreateRawConfig()
        {
            Create_TargetID = 2300, Create_TargetCount = 1,
            /*Ä¾²Ä1 Ìú¶§2 »úÐµÔª¼þ2 */
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1100,1),new ItemRaw(1114,2), new ItemRaw(1200, 2) }
        },
        /*¶ÌÐÍ³å·æÇ¹*/new CreateRawConfig()
        {
            Create_TargetID = 2301, Create_TargetCount = 1,
            /*Ä¾²Ä1 Ìú¶§2 »úÐµÔª¼þ2 Ç¹ÐµÔª¼þ1 */
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1100, 1), new ItemRaw(1114, 2), new ItemRaw(1200, 2), new ItemRaw(1202, 1) }
        },
        /*ÖÆÊ½×Ô¶¯²½Ç¹*/new CreateRawConfig()
        {
            Create_TargetID = 2302, Create_TargetCount = 1,
            /*Ä¾²Ä1 Ìú¶§2 »úÐµÔª¼þ2  Ç¹ÐµÔª¼þ2 */
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1100, 1), new ItemRaw(1114, 2), new ItemRaw(1200, 2), new ItemRaw(1202, 2) }
        },
        /*ÖÆÊ½ÊÖÇ¹*/new CreateRawConfig()
        {
            Create_TargetID = 2303, Create_TargetCount = 1,
            /*Ä¾²Ä1 Ìú¶§2 »úÐµÔª¼þ2  Ç¹ÐµÔª¼þ1 */
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1100, 1), new ItemRaw(1114, 2), new ItemRaw(1200, 2), new ItemRaw(1202, 1) }
        },
        /*±Ã¶¯ö±µ¯Ç¹*/new CreateRawConfig()
        {
            Create_TargetID = 2304, Create_TargetCount = 1,
            /*Ä¾²Ä1 Ìú¶§2 »úÐµÔª¼þ2 Ç¹ÐµÔª¼þ2 */
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1100, 1), new ItemRaw(1114, 2), new ItemRaw(1200, 2), new ItemRaw(1202, 2) }
        },
        /*ÂóµÂÉ­»úÇ¹*/new CreateRawConfig()
        {
            Create_TargetID = 2305, Create_TargetCount = 1,
            /*Ä¾²Ä1 Ìú¶§2 »úÐµÔª¼þ2  Ç¹ÐµÔª¼þ3 */
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1100, 1), new ItemRaw(1114, 2), new ItemRaw(1200, 2), new ItemRaw(1202, 3) }
        },
        /*¾«×¼×Ô¶¯²½Ç¹*/new CreateRawConfig()
        {
            Create_TargetID = 2306, Create_TargetCount = 1,
            /*Ä¾²Ä1 Ìú¶§2 »úÐµÔª¼þ2 Ç¹ÐµÔª¼þ5 */
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1100, 1), new ItemRaw(1114, 2), new ItemRaw(1200, 2), new ItemRaw(1202, 5) }
        },
        /*Ä¾¼ý*/new CreateRawConfig()
        {
            Create_TargetID = 9000, Create_TargetCount = 9,
            /*Ê÷Ö¦1Ä¾²Ä1*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001, 1),new ItemRaw(1100, 1) }
        },
        /*Ìú¼ý*/new CreateRawConfig()
        {
            Create_TargetID = 9001, Create_TargetCount = 9,
            /*Ê÷Ö¦1Ìú¶§1*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001, 1),new ItemRaw(1114, 1) }
        },
        /*ÌúÖÆµ¯Íè*/new CreateRawConfig()
        {
            Create_TargetID = 9010, Create_TargetCount = 10,
            /*Ìú¶§1ÏõÊ¯Ëé1*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1114, 1),new ItemRaw(1112, 1) }
        },
    };
}
public struct CreateRawConfig
{
    public int Create_TargetID;
    public int Create_TargetCount;
    public List<ItemRaw> Create_RawList;
}