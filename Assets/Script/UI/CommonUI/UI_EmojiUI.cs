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

    public Transform RightEmoji;
    public Transform LeftEmoji;
    public Transform UpEmoji;
    public Transform DownEmoji;
    public Transform UpRightEmoji;
    public Transform DownRightEmoji;
    public Transform UpLeftEmoji;
    public Transform DownLeftEmoji;

    private Transform highLightEmoji;
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
                dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.transform.position).normalized;
                CheckHighLight();
            }
        }
        else
        {
            if (open)
            {
                Close();
            }
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            CheckEmoji();
        }
    }
    private void Open()
    {
        open = true;
        transform.DOKill();
        transform.DOScale(Vector3.one, 0.2f);
    }
    private void Close()
    {
        open = false;
        transform.DOKill();
        transform.DOScale(Vector3.zero, 0.2f);
    }
    private void CheckHighLight()
    {
        if (dir.x > 0.9f)
        {
            HighLight(RightEmoji);
            return;
        }
        else if (dir.x < -0.9f)
        {
            HighLight(LeftEmoji);
            return;
        }
        if (dir.y > 0.9f)
        {
            HighLight(UpEmoji);
            return;
        }
        else if (dir.y < -0.9f)
        {
            HighLight(DownEmoji);
            return;
        }

        if (dir.x > 0.38f && dir.y > 0.38f)
        {
            HighLight(UpRightEmoji);
            return;
        }
        if (dir.x < -0.38f && dir.y > 0.38f)
        {
            HighLight(UpLeftEmoji);
            return;
        }
        if (dir.x > 0.38f && dir.y < -0.38f)
        {
            HighLight(DownRightEmoji);
            return;
        }
        if (dir.x < -0.38f && dir.y < -0.38f)
        {
            HighLight(DownLeftEmoji);
            return;
        }
        HighLight(null);

    }
    private void HighLight(Transform highLight)
    {
        if (highLightEmoji != highLight)
        {
            if(highLightEmoji != null)
            {
                highLightEmoji.DOScale(Vector3.one, 0.2f);
                highLightEmoji.GetComponent<Image>().DOFade(0.5f, 0.2f);
            }
            highLightEmoji = highLight;
            if (highLightEmoji != null)
            {
                highLightEmoji.DOScale(Vector3.one * 1.2f, 0.2f);
                highLightEmoji.GetComponent<Image>().DOFade(1, 0.2f);
            }
        }
    }
    private void CheckEmoji()
    {
        if (dir.x > 0.9f)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_Emoji()
            {
                id = 4
            }); 
            return;
        }
        else if (dir.x < -0.9f)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_Emoji()
            {
                id = 5
            });
            return;
        }
        if (dir.y > 0.9f)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_Emoji()
            {
                id = 2
            });
            return;
        }
        else if (dir.y < -0.9f)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_Emoji()
            {
                id = 3
            });
            return;
        }

        if (dir.x > 0.38f && dir.y > 0.38f)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_Emoji()
            {
                id = 12
            });
            return;
        }
        if (dir.x < -0.38f && dir.y > 0.38f)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_Emoji()
            {
                id = 16
            });
            return;
        }
        if (dir.x > 0.38f && dir.y < -0.38f)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_Emoji()
            {
                id = 11
            });
            return;
        }
        if (dir.x < -0.38f && dir.y < -0.38f)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_Emoji()
            {
                id = 9
            });
            return;
        }
    }
}
