using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class QuestSystem 
{
}
public class Quest1000 : QuestBase
{
    private const int QuestID = 1000;
    public override void Listen_PlayerAction(PlayerAction playerAction)
    {
        if (playerAction == PlayerAction.OpenBag)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_AddExp()
            {
                exp = QuestConfigData.GetQuestConfig(QuestID).QuestExp
            });
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_QuestComplete()
            {
                id = QuestID
            });
        }
        base.Listen_PlayerAction(playerAction);
    }
}
public class Quest1001 : QuestBase
{
    private const int QuestID = 1001;
    public override void Listen_PlayerAction(PlayerAction playerAction)
    {
        if (playerAction == PlayerAction.OpenBuilding)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_AddExp()
            {
                exp = QuestConfigData.GetQuestConfig(QuestID).QuestExp
            });
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_QuestComplete()
            {
                id = QuestID
            });
        }
        base.Listen_PlayerAction(playerAction);
    }

}
public class Quest1002 : QuestBase
{
    private const int QuestID = 1002;
    public override void Listen_PlayerAction(PlayerAction playerAction)
    {
        if (playerAction == PlayerAction.OpenSkill)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_AddExp()
            {
                exp = QuestConfigData.GetQuestConfig(QuestID).QuestExp
            });
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_QuestComplete()
            {
                id = QuestID
            });
        }
        base.Listen_PlayerAction(playerAction);
    }
}
public class Quest1003 : QuestBase
{
    private const int QuestID = 1003;
    public override void Listen_PlayerAction(PlayerAction playerAction)
    {
        if (playerAction == PlayerAction.OpenEmoji)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_AddExp()
            {
                exp = QuestConfigData.GetQuestConfig(QuestID).QuestExp
            });
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_QuestComplete()
            {
                id = QuestID
            });
        }
        base.Listen_PlayerAction(playerAction);
    }
}
public class Quest2000 : QuestBase
{
    private const int QuestID = 2000;
    public override void Listen_Build(int buildingId)
    {
        if (buildingId == 3201)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_AddExp()
            {
                exp = QuestConfigData.GetQuestConfig(QuestID).QuestExp
            });
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_QuestComplete()
            {
                id = QuestID
            });
        }
        base.Listen_Build(buildingId);
    }
}
public class Quest2001 : QuestBase
{
    private const int QuestID = 2001;
    public override void Listen_Earn(int Coin)
    {
        if (Coin >= 1)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_AddExp()
            {
                exp = QuestConfigData.GetQuestConfig(QuestID).QuestExp
            });
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_QuestComplete()
            {
                id = QuestID
            });
        }
        base.Listen_Earn(Coin);
    }
}
public class Quest3000 : QuestBase
{

}
public class Quest9999 : QuestBase
{
    private const int QuestID = 9999;
}