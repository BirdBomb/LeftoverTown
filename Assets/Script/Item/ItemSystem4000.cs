using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;

public class ItemSystem4000 
{

}
#region//烤制
/// <summary>
/// 烤肉块
/// </summary>
public class Item_4001 : ItemBase_Food
{
    #region//食用逻辑
    public override bool Holding_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.bodyController.SetAnimatorTrigger(BodyPart.Hand, "Eat");
                owner.bodyController.SetAnimatorTrigger(BodyPart.Head, "Eat");
                if (input) Eat();
            }
        }
        inputData.leftPressTimer = pressTimer;
        return true;
    }
    public override void Holding_ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.Holding_ReleaseLeftPress(state, input, player);
    }
    public override void Eat()
    {
        owner.bodyController.SetAnimatorAction(BodyPart.Head, (string str) =>
        {
            if (str.Equals("Eat"))
            {
                Expend(1);
                owner.hungryManager.AddFood(5);
            }
        });
    }
    #endregion
}
/// <summary>
/// 烤肉排
/// </summary>
public class Item_4002 : ItemBase_Food
{
    #region//食用逻辑
    public override bool Holding_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.bodyController.SetAnimatorTrigger(BodyPart.Hand, "Eat");
                owner.bodyController.SetAnimatorTrigger(BodyPart.Head, "Eat");
                if (input) Eat();
            }
        }
        inputData.leftPressTimer = pressTimer;
        return true;
    }
    public override void Holding_ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.Holding_ReleaseLeftPress(state, input, player);
    }
    public override void Eat()
    {
        owner.bodyController.SetAnimatorAction(BodyPart.Head, (string str) =>
        {
            if (str.Equals("Eat"))
            {
                Expend(1);
                owner.hungryManager.AddFood(5);
            }
        });
        base.Eat();
    }
    #endregion
}
/// <summary>
/// 烤禽腿
/// </summary>
public class Item_4003 : ItemBase_Food
{
    #region//食用逻辑
    public override bool Holding_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.bodyController.SetAnimatorTrigger(BodyPart.Hand, "Eat");
                owner.bodyController.SetAnimatorTrigger(BodyPart.Head, "Eat");
                if (input) Eat();
            }
        }
        inputData.leftPressTimer = pressTimer;
        return true;
    }
    public override void Holding_ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.Holding_ReleaseLeftPress(state, input, player);
    }
    public override void Eat()
    {
        owner.bodyController.SetAnimatorAction(BodyPart.Head, (string str) =>
        {
            if (str.Equals("Eat"))
            {
                Expend(1);
                owner.hungryManager.AddFood(5);
            }
        });
    }
    #endregion
}
/// <summary>
/// 烤内脏
/// </summary>
public class Item_4004 : ItemBase_Food
{
    #region//食用逻辑
    public override bool Holding_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.bodyController.SetAnimatorTrigger(BodyPart.Hand, "Eat");
                owner.bodyController.SetAnimatorTrigger(BodyPart.Head, "Eat");
                if (input) Eat();
            }
        }
        inputData.leftPressTimer = pressTimer;
        return true;
    }
    public override void Holding_ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.Holding_ReleaseLeftPress(state, input, player);
    }
    public override void Eat()
    {
        owner.bodyController.SetAnimatorAction(BodyPart.Head, (string str) =>
        {
            if (str.Equals("Eat"))
            {
                Expend(1);
                owner.hungryManager.AddFood(5);
            }
        });
        base.Eat();
    }
    #endregion
}
/// <summary>
/// 烤臭肉
/// </summary>
public class Item_4005 : ItemBase_Food
{
    #region//食用逻辑
    public override bool Holding_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.bodyController.SetAnimatorTrigger(BodyPart.Hand, "Eat");
                owner.bodyController.SetAnimatorTrigger(BodyPart.Head, "Eat");
                if (input) Eat();
            }
        }
        inputData.leftPressTimer = pressTimer;
        return true;
    }
    public override void Holding_ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.Holding_ReleaseLeftPress(state, input, player);
    }
    public override void Eat()
    {
        owner.bodyController.SetAnimatorAction(BodyPart.Head, (string str) =>
        {
            if (str.Equals("Eat"))
            {
                Expend(1);
                owner.hungryManager.AddFood(5);
            }
        });
    }
    #endregion
}
/// <summary>
/// 烤蛇果
/// </summary>
public class Item_4006 : ItemBase_Food
{
    #region//食用逻辑
    public override bool Holding_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.bodyController.SetAnimatorTrigger(BodyPart.Hand, "Eat");
                owner.bodyController.SetAnimatorTrigger(BodyPart.Head, "Eat");
                if (input) Eat();
            }
        }
        inputData.leftPressTimer = pressTimer;
        return true;
    }
    public override void Holding_ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.Holding_ReleaseLeftPress(state, input, player);
    }
    public override void Eat()
    {
        owner.bodyController.SetAnimatorAction(BodyPart.Head, (string str) =>
        {
            if (str.Equals("Eat"))
            {
                Expend(1);
                owner.hungryManager.AddFood(5);
            }
        });
    }
    #endregion
}
/// <summary>
/// 烤水桔
/// </summary>
public class Item_4007 : ItemBase_Food
{
    #region//食用逻辑
    public override bool Holding_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.bodyController.SetAnimatorTrigger(BodyPart.Hand, "Eat");
                owner.bodyController.SetAnimatorTrigger(BodyPart.Head, "Eat");
                if (input) Eat();
            }
        }
        inputData.leftPressTimer = pressTimer;
        return true;
    }
    public override void Holding_ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.Holding_ReleaseLeftPress(state, input, player);
    }
    public override void Eat()
    {
        owner.bodyController.SetAnimatorAction(BodyPart.Head, (string str) =>
        {
            if (str.Equals("Eat"))
            {
                Expend(1);
                owner.hungryManager.AddFood(5);
            }
        });
    }
    #endregion
}
/// <summary>
/// 烤薯果
/// </summary>
public class Item_4008 : ItemBase_Food
{
    #region//食用逻辑
    public override bool Holding_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.bodyController.SetAnimatorTrigger(BodyPart.Hand, "Eat");
                owner.bodyController.SetAnimatorTrigger(BodyPart.Head, "Eat");
                if (input) Eat();
            }
        }
        inputData.leftPressTimer = pressTimer;
        return true;
    }
    public override void Holding_ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.Holding_ReleaseLeftPress(state, input, player);
    }
    public override void Eat()
    {
        owner.bodyController.SetAnimatorAction(BodyPart.Head, (string str) =>
        {
            if (str.Equals("Eat"))
            {
                Expend(1);
                owner.hungryManager.AddFood(5);
            }
        });
    }
    #endregion
}

#endregion
#region//烹饪
/// <summary>
/// 失败菜肴
/// </summary>
public class Item_4100 : ItemBase_Food
{
    #region//食用逻辑
    public override bool Holding_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.bodyController.SetAnimatorTrigger(BodyPart.Hand, "Eat");
                owner.bodyController.SetAnimatorTrigger(BodyPart.Head, "Eat");
                if (input) Eat();
            }
        }
        inputData.leftPressTimer = pressTimer;
        return true;
    }
    public override void Holding_ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.Holding_ReleaseLeftPress(state, input, player);
    }
    public override void Eat()
    {
        owner.bodyController.SetAnimatorAction(BodyPart.Head, (string str) =>
        {
            if (str.Equals("Eat"))
            {
                Expend(1);
                owner.hungryManager.AddFood(5);
            }
        });
    }
    #endregion
}
/// <summary>
/// 薯果肉煲
/// </summary>
public class Item_4101 : ItemBase_Food
{
    #region//食用逻辑
    public override bool Holding_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.bodyController.SetAnimatorTrigger(BodyPart.Hand, "Eat");
                owner.bodyController.SetAnimatorTrigger(BodyPart.Head, "Eat");
                if (input) Eat();
            }
        }
        inputData.leftPressTimer = pressTimer;
        return true;
    }
    public override void Holding_ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.Holding_ReleaseLeftPress(state, input, player);
    }
    public override void Eat()
    {
        owner.bodyController.SetAnimatorAction(BodyPart.Head,(string str) =>
        {
            if (str.Equals("Eat"))
            {
                Expend(1);
                owner.hungryManager.AddFood(5);
            }
        });
    }
    #endregion
}
/// <summary>
/// 炖杂果
/// </summary>
public class Item_4102 : ItemBase_Food
{
    #region//食用逻辑
    public override bool Holding_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.bodyController.SetAnimatorTrigger(BodyPart.Hand, "Eat");
                owner.bodyController.SetAnimatorTrigger(BodyPart.Head, "Eat");
                if (input) Eat();
            }
        }
        inputData.leftPressTimer = pressTimer;
        return true;
    }
    public override void Holding_ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.Holding_ReleaseLeftPress(state, input, player);
    }
    public override void Eat()
    {
        owner.bodyController.SetAnimatorAction(BodyPart.Head, (string str) =>
        {
            if (str.Equals("Eat"))
            {
                Expend(1);
                owner.hungryManager.AddFood(5);
            }
        });
    }
    #endregion
}
/// <summary>
/// 抓肉
/// </summary>
public class Item_4103 : ItemBase_Food
{
    #region//食用逻辑
    public override bool Holding_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.bodyController.SetAnimatorTrigger(BodyPart.Hand, "Eat");
                owner.bodyController.SetAnimatorTrigger(BodyPart.Head, "Eat");
                if (input) Eat();
            }
        }
        inputData.leftPressTimer = pressTimer;
        return true;
    }
    public override void Holding_ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.Holding_ReleaseLeftPress(state, input, player);
    }
    public override void Eat()
    {
        owner.bodyController.SetAnimatorAction(BodyPart.Head, (string str) =>
        {
            if (str.Equals("Eat"))
            {
                Expend(1);
                owner.hungryManager.AddFood(5);
            }
        });
    }
    #endregion
}
/// <summary>
/// 果渍风味烧
/// </summary>
public class Item_4104 : ItemBase_Food
{
    #region//食用逻辑
    public override bool Holding_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.bodyController.SetAnimatorTrigger(BodyPart.Hand, "Eat");
                owner.bodyController.SetAnimatorTrigger(BodyPart.Head, "Eat");
                if (input) Eat();
            }
        }
        inputData.leftPressTimer = pressTimer;
        return true;
    }
    public override void Holding_ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.Holding_ReleaseLeftPress(state, input, player);
    }
    public override void Eat()
    {
        owner.bodyController.SetAnimatorAction(BodyPart.Head, (string str) =>
        {
            if (str.Equals("Eat"))
            {
                Expend(1);
                owner.hungryManager.AddFood(5);
            }
        });
    }
    #endregion
}
/// <summary>
/// 水煮鱼
/// </summary>
public class Item_4105 : ItemBase_Food
{
    #region//食用逻辑
    public override bool Holding_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.bodyController.SetAnimatorTrigger(BodyPart.Hand, "Eat");
                owner.bodyController.SetAnimatorTrigger(BodyPart.Head, "Eat");
                if (input) Eat();
            }
        }
        inputData.leftPressTimer = pressTimer;
        return true;
    }
    public override void Holding_ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.Holding_ReleaseLeftPress(state, input, player);
    }
    public override void Eat()
    {
        owner.bodyController.SetAnimatorAction(BodyPart.Head, (string str) =>
        {
            if (str.Equals("Eat"))
            {
                Expend(1);
                owner.hungryManager.AddFood(5);
            }
        });
    }
    #endregion
}

#endregion
#region//药剂
/// <summary>
/// 鸟果汁
/// </summary>
public class Item_4200 : ItemBase_Food
{
    #region//食用逻辑
    public override bool Holding_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.bodyController.SetAnimatorTrigger(BodyPart.Hand, "Eat");
                owner.bodyController.SetAnimatorTrigger(BodyPart.Head, "Eat");
                if (input) Eat();
            }
        }
        inputData.leftPressTimer = pressTimer;
        return true;
    }
    public override void Holding_ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.Holding_ReleaseLeftPress(state, input, player);
    }
    public override void Eat()
    {
        owner.bodyController.SetAnimatorAction(BodyPart.Head, (string str) =>
        {
            if (str.Equals("Eat"))
            {
                Expend(1);
                owner.hungryManager.AddFood(5);
            }
        });
    }
    #endregion
}
#endregion
