using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class PlayerNetController : NetworkBehaviour
{
    [SerializeField,Header("½ÇÉ«¿ØÖÆÆ÷")]
    private PlayerController playerController;
    public override void Spawned()
    {
        base.Spawned();
    }
    private Vector2 moveDir_temp;
    private bool left_click = false;
    private bool right_click = false;
    private bool left_press = false;
    private bool right_press = false;
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            moveDir_temp = Vector2.zero;
            if (data.ClickLeftMouse > 0)
            {
                left_click = true;
            }
            else
            {
                left_click = false;
            }
            if (data.ClickRightMouse > 0)
            {
                right_click = true;
            }
            else
            {
                right_click = false;
            }
            if (data.PressLeftMouse)
            {
                left_press = true;
            }
            else
            {
                left_press = false;
            }
            if (data.PressRightMouse)
            {
                right_press = true;
            }
            else
            {
                right_press = false;
            }

            if (data.goRight)
            {
                moveDir_temp += new Vector2(1, 0);
            }
            if (data.goLeft)
            {
                moveDir_temp += new Vector2(-1, 0);
            }
            if (data.goUp)
            {
                moveDir_temp += new Vector2(0, 1);
            }
            if (data.goDown)
            {
                moveDir_temp += new Vector2(0, -1);
            }
            playerController.InputMouse(left_click, right_click, left_press, right_press,Runner.DeltaTime);
            playerController.InputMoveDir(moveDir_temp,Runner.DeltaTime,data.goFaster);
            playerController.InputFaceDir(data.mousePostion, Runner.DeltaTime);
        }
        base.FixedUpdateNetwork();
    }
}

