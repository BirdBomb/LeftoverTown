using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class BuildingObj_Bookshelf : BuildingObj_Box
{
    public List<Sprite> list_Sprites = new List<Sprite>();
    public SpriteRenderer spriteRenderer_Main;
    public override void All_OnRawDataUpdate()
    {
        base.All_OnRawDataUpdate();
        int bookCount = 0;
        buildingData_Box.ReadItemDataList(out List<ItemData> itemDatas);
        foreach (var itemData in itemDatas)
        {
            if (itemData.I > 0) { bookCount++; }
        }
        switch (bookCount)
        {
            case 0: spriteRenderer_Main.sprite = list_Sprites[0]; break;
            case 1: spriteRenderer_Main.sprite = list_Sprites[1]; break;
            case 2: spriteRenderer_Main.sprite = list_Sprites[2]; break;
            default: spriteRenderer_Main.sprite = list_Sprites[3]; break;
        }

    }
}
