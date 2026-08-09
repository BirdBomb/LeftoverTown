using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBase 
{
    public virtual void Listen_Input(KeyCode keyCode)
    {

    }
    public virtual void Listen_PlayerAction(PlayerAction playerAction) 
    {

    }
    public virtual void Listen_Build(int buildingId)
    {

    }
    public virtual void Listen_Create(int buildingId)
    {

    }
    public virtual void Listen_Earn(int Coin)
    {

    }
}
