using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Throwable : BulletBase
{
    private Vector3 vector3_From;
    private Vector3 vector3_To;
    private float float_Elapsed = 0f;
    private float float_FlyDuraction = 1f;
    private float startHeight;   // 起点Y坐标（包含地面高度）
    private float endHeight;     // 终点Y坐标
    private Vector3 originalScale;
    public float rotateSpeed = 360f;  // 每秒旋转角度（度/秒）

    public void SetPath(Vector3 from,Vector3 to,float speedSqr)
    {
        vector3_From = from;
        vector3_To = to;
        startHeight = from.y;
        endHeight = to.y;
        float duraction = Vector3.Distance(from, to) / speedSqr + 0.5f;
        float_FlyDuraction = Mathf.Max(0.0001f, duraction);
        float_Elapsed = 0f;

        SetLifeTime(duraction + 0.1f);

        // 设置初始位置
        transform.position = vector3_From;
        originalScale = transform.localScale;
    }
    public void FixedUpdate()
    {
        if (!_hide)
        {
            Move(Time.fixedDeltaTime);
        }
    }
    private void Move(float dt)
    {
        if (float_FlyDuraction <= 0f) return;

        float_Elapsed += dt;
        float t = Mathf.Clamp01(float_Elapsed / float_FlyDuraction);

        // 1. 平面线性插值（X轴）
        float x = Mathf.Lerp(vector3_From.x, vector3_To.x, t);

        // 2. 抛物线高度（基于X轴水平距离）
        float horizontalDist = Mathf.Abs(vector3_To.x - vector3_From.x);
        float peakHeight = horizontalDist * 0.5f;  // 峰值高度

        // 抛物线弧线（起点0，中点峰值，终点0）
        float arcHeight = 4f * peakHeight * t * (1f - t);

        // 3. 高度线性插值（从起点Y到终点Y）
        float linearHeight = Mathf.Lerp(startHeight, endHeight, t);

        // 4. 最终Y坐标 = 线性高度 + 弧线偏移
        float y = linearHeight + arcHeight;

        // 5. 设置位置（2D只需要X和Y，Z为0）
        transform.position = new Vector3(x, y, 0);


        // 6. 根据抛物线进度缩放（最高点放大2倍）
        float maxScale = 1.25f;
        float scaleFactor = 1f + (arcHeight / peakHeight) * (maxScale - 1f);

        transform.localScale = originalScale * scaleFactor;

        // 旋转：持续自转
        float angle = rotateSpeed * float_Elapsed;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (float_Elapsed >= float_FlyDuraction)
        {
            transform.position = new Vector3(vector3_To.x, vector3_To.y, 0);
            OnArrived();
            HideBullet();
        }
    }
    public virtual void OnArrived()
    {

    }
    public override void HideBullet()
    {
        _hide = true;
        base.HideBullet();
    }

    public override void SetOwner(ActorManager owner)
    {
        actorManager_Owner = owner;
        base.SetOwner(owner);
    }
}
