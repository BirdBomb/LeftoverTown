using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UI_Cursor : MonoBehaviour
{
    private bool following = false;
    [SerializeField]
    private Image image_FollowCursor;
    private void Start()
    {
        MessageBroker.Default.Receive<UIEvent.UIEvent_ShowCursorImage>().Subscribe(_ =>
        {
            StartFollowing(_.sprite);
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_HideCursorImage>().Subscribe(_ =>
        {
            EndFollowing();
        }).AddTo(this);
       
    }
    private void Update()
    {
        if (following)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                FollowCursor();
            }
            else
            {
                EndFollowing();
            }
        }
    }
    private void FollowCursor()
    {
        image_FollowCursor.transform.position = Input.mousePosition;
    }
    private void StartFollowing(Sprite sprite)
    {
        following = true;
        image_FollowCursor.enabled = true;
        image_FollowCursor.sprite = sprite;
        image_FollowCursor.transform.DOKill();
        image_FollowCursor.transform.localScale = Vector3.one;
        image_FollowCursor.transform.DOShakeRotation(0.2f, new Vector3(0, 0, 10), 30, 720, false).SetEase(Ease.Linear).SetLoops(-1);
        image_FollowCursor.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
    }
    private void EndFollowing()
    {
        following = false;
        image_FollowCursor.transform.DOKill();
        image_FollowCursor.transform.rotation = Quaternion.identity;
        image_FollowCursor.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
        {
            image_FollowCursor.enabled = false;
        });
    }
}
