using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
//using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;
using DG.Tweening;
using UnityEngine.Tilemaps;
/// <summary>
/// ��ɫ������
/// </summary>
public class RoleBase : MonoBehaviour
{
    private void Awake()
    {
        //roleRig.gravityScale = 0;
    }
    private void Start()
    {
        navManager = GameObject.Find("Grid").GetComponent<NavManager>();
    }
    public virtual void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    MyTile tile_To = navManager.tileList[42, 29];
        //    MyTile tile_From = GetMyTile();

        //    Debug.Log(tile_From.x+"/"+tile_From.y);
        //    FindPath(tile_To, tile_From);
        //}
    }
    /*�ƶ�ϵͳ*/
    #region
    [Header("������ڵ�")]
    public Transform Root;
    public void TurnLeft()
    {
        Root.localScale = new Vector3(-1, 1, 1);
    }
    public void TurnRight()
    {
        Root.localScale = new Vector3(1, 1, 1);
    }
    public void Move(UnityEngine.Vector2 vector2)
    {
        transform.position = 
            transform.position + new UnityEngine.Vector3(vector2.x * speed, vector2.y * speed, 0);
    }
    #endregion
    /*��������*/
    #region
    private int Hp;
    private float Speed;

    #endregion
    /*Ѱ·ϵͳ*/
    #region

    public List<MyTile> myLoad = new List<MyTile>();
    public Rigidbody2D roleRig;
    public float speed;

    private NavManager navManager;

    private MyTile curTile;
    private MyTile nextTile;

    /// <summary>
    /// ����������ص�ȷ��һ��·��
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    public virtual void FindPath(MyTile from,MyTile to)
    {
        myLoad.Clear();
        myLoad = navManager.FindPath(from,to);
        UpdatePath();
    }
    /// <summary>
    /// ���µ�ǰ���ж�·��
    /// </summary>
    public virtual void UpdatePath()
    {
        MoveToNext();
    }
    /// <summary>
    /// �ƶ�����һ������
    /// </summary>
    private void MoveToNext()
    {
        if (myLoad.Count != 0)
        {
            MyTile tile = myLoad[0];
            myLoad.Remove(tile);
            transform.DOMove(new(tile.pos.x, tile.pos.y, 0), speed).SetEase(Ease.Linear).OnComplete(() =>
            {
                MoveToNext();
            });
        }
    }
    public void ShowPath()
    {
        for(int i = 0; i < myLoad.Count; i++)
        {
            Debug.Log(myLoad[i].x + "/" + myLoad[i].y + "/" + myLoad[i].passOffset);
        }
    }
    #endregion
    /*����ϵͳ*/
    #region
    public List<int> itemList = new List<int>();
    #endregion
}
