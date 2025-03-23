using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class UI_EmojiUI : MonoBehaviour
{
    private bool open = false;
    private Vector3 dir = Vector3.zero;
    private float distance = 0;
    private Direction side;
    private Direction Side
    {
        get { return side; }
        set { if (side != value) { side = value; TryToHighLight(); } }
    }

    public Transform tran_RightPart;
    public Transform tran_LeftPart;
    public Transform tran_UpPart;
    public Transform tran_DownPart;
    public Transform tran_UpRightPart;
    public Transform tran_DownRightPart;
    public Transform tran_UpLeftPart;
    public Transform tran_DownLeftPart;

    private Transform tran_HighLightPart;
    private void Update()
    {
        if (Input.GetKey(KeyCode.G))
        {
            if (!open)
            {
                Open();
            }
            if (Camera.main != null)
            {
                dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.transform.position);
                distance = dir.magnitude;
                dir = dir.normalized;
                CheckSide();
            }
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            TryToSendEmoji();
            if (open)
            {
                Close();
            }
        }
    }
    private void Open()
    {
        open = true;
        Side = Direction.Center;
        transform.DOKill();
        transform.DOScale(Vector3.one, 0.2f);
    }
    private void Close()
    {
        open = false;
        Side = Direction.Center;
        transform.DOKill();
        transform.DOScale(Vector3.zero, 0.2f);
    }
    private void CheckSide()
    {
        if (distance > 1)
        {
            if (dir.x > 0.9f)
            {
                Side = Direction.Right;
                return;
            }
            else if (dir.x < -0.9f)
            {
                Side = Direction.Left;
                return;
            }
            if (dir.y > 0.9f)
            {
                Side = Direction.Up;
                return;
            }
            else if (dir.y < -0.9f)
            {
                Side = Direction.Down;
                return;
            }
            if (dir.x > 0.38f && dir.y > 0.38f)
            {
                Side = Direction.UpRight;
                return;
            }
            if (dir.x < -0.38f && dir.y > 0.38f)
            {
                Side = Direction.UpLeft;
                return;
            }
            if (dir.x > 0.38f && dir.y < -0.38f)
            {
                Side = Direction.DownRight;
                return;
            }
            if (dir.x < -0.38f && dir.y < -0.38f)
            {
                Side = Direction.DownLeft;
                return;
            }
        }
        Side = Direction.Center;
    }
    private void TryToHighLight()
    {
        if (tran_HighLightPart != null)
        {
            tran_HighLightPart.DOScale(Vector3.one, 0.2f);
            tran_HighLightPart.GetComponent<Image>().DOFade(0.5f, 0.2f);
        }
        switch (Side)
        {
            case Direction.Center:
                tran_HighLightPart = null;
                break;
            case Direction.Right:
                tran_HighLightPart = tran_RightPart;
                break;
            case Direction.Left:
                tran_HighLightPart = tran_LeftPart;
                break;
            case Direction.Up:
                tran_HighLightPart = tran_UpPart;
                break;
            case Direction.Down:
                tran_HighLightPart = tran_DownPart;
                break;
            case Direction.UpRight:
                tran_HighLightPart = tran_UpRightPart;
                break;
            case Direction.UpLeft:
                tran_HighLightPart = tran_UpLeftPart;
                break;
            case Direction.DownRight:
                tran_HighLightPart = tran_DownRightPart;
                break;
            case Direction.DownLeft:
                tran_HighLightPart = tran_DownLeftPart;
                break;
        }
        if (tran_HighLightPart != null)
        {
            tran_HighLightPart.DOScale(Vector3.one * 1.2f, 0.2f);
            tran_HighLightPart.GetComponent<Image>().DOFade(1, 0.2f);
        }
    }
    private void TryToSendEmoji()
    {
        switch (Side)
        {
            case Direction.Center:
                break;
            case Direction.Right:
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_Emoji()
                {
                    emoji = Emoji.Yell
                });
                break;
            case Direction.Left:
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_Emoji()
                {
                    emoji = Emoji.Shock
                });
                break;
            case Direction.Up:
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_Emoji()
                {
                    emoji = Emoji.Puzzled
                });
                break;
            case Direction.Down:
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_Emoji()
                {
                    emoji = Emoji.Greeting
                });
                break;
            case Direction.UpRight:
                break;
            case Direction.UpLeft:
                break;
            case Direction.DownRight:
                break;
            case Direction.DownLeft:
                break;
        }

    }
    public enum Direction
    {
        Center, Up, Down, Right, Left, UpRight, UpLeft, DownRight, DownLeft,
    }

}
