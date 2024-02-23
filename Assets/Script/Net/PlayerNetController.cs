using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class PlayerNetController : NetworkBehaviour
{
    [SerializeField, Header("角色控制器")]
    private PlayerController playerController;
    [SerializeField, Header("角色摄像机")]
    private Camera playerCamera;
    public override void Spawned()
    {
        if (!Object.HasInputAuthority)
        {
            playerCamera.enabled = false;
        }
        base.Spawned();
    }
    private Vector2 moveDir_temp;
    private bool left_click = false;
    private bool right_click = false;
    private bool left_press = false;
    private bool right_press = false;
    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority && GetInput(out NetworkInputData data))
        {
            moveDir_temp = Vector2.zero;
            if (data.ClickLeftMouse > 0)
            {
                LeftClickTime += data.ClickLeftMouse;
            }
            if (data.ClickRightMouse > 0)
            {
                RightClickTime += data.ClickRightMouse;
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
            LeftPress = left_press;
            RightPress = right_press;
            MoveDir = moveDir_temp;
            MoveSpeedUp = data.goFaster;
            MousePostion = data.mousePostion;

        }
        if (Object.HasInputAuthority)
        {
            playerController.InputMouse(LeftClickTime, RightClickTime, LeftPress, RightPress, Runner.DeltaTime);
            playerController.InputMoveDir(MoveDir, Runner.DeltaTime, MoveSpeedUp);
            playerController.InputFaceDir(MousePostion, Runner.DeltaTime);
        }
        base.FixedUpdateNetwork();
    }
    [Networked]
    public int LeftClickTime { get; set; } = 0;
    [Networked]
    public int RightClickTime { get; set; } = 0;
    [Networked]
    public bool LeftPress { get; set; } = false;
    [Networked]
    public bool RightPress { get; set; } = false;
    [Networked]
    public Vector2 MoveDir { get; set; } = Vector2.zero;
    [Networked]
    public bool MoveSpeedUp { get; set; } = false;
    [Networked]
    public Vector3 MousePostion { get; set; } = Vector3.zero;
    //public void FixedUpdate()
    //{
    //    playerController.InputMouse(LeftClickTime, RightClickTime, LeftPress, RightPress, Time.fixedDeltaTime);
    //    playerController.InputMoveDir(MoveDir, Time.fixedDeltaTime, MoveSpeedUp);
    //    playerController.InputFaceDir(MousePostion, Time.fixedDeltaTime);
    //}
}

