using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_ItemObj : EffectBase
{
    public SpriteRenderer spriteRenderer_Icon;
    private Transform transform_Target;
    private bool bool_Follow = false;
    private float float_Speed = 5f;
    public void DrawSpriter(Sprite icon)
    {
        spriteRenderer_Icon.sprite = icon;
    }
    public void BindFollow(Transform target)
    {
        bool_Follow = true;
        transform_Target = target;
    }
    private void FixedUpdate()
    {
        if (bool_Follow)
        {
            if (transform_Target != null)
            {
                transform.position += (transform_Target.position - transform.position).normalized * Time.fixedDeltaTime * float_Speed;
                if (Vector2.Distance(transform.position, transform_Target.position) < 0.1f)
                {
                    TryToHide();
                }
            }
            else
            {
                TryToHide();
            }
        }
    }
    private void TryToHide()
    {
        bool_Follow = false;
        transform_Target = null;
        Hide();
    }
}
