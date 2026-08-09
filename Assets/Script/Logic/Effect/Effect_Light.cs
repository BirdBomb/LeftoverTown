using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Effect_Light : EffectBase
{
    public Light2D light_Spot;
    public float float_LightLife;
    private float float_LightLifeTimer;

    public override void OnEnable()
    {
        float_LightLifeTimer = float_LightLife;
        light_Spot.intensity = 1;
        base.OnEnable();
    }
    public override void OnDisable()
    {
        float_LightLifeTimer = 0;
        light_Spot.intensity = 0;
        base.OnDisable();
    }
    private void FixedUpdate()
    {
        if (float_LightLifeTimer > 0)
        {
            float_LightLifeTimer -= Time.fixedDeltaTime;
            light_Spot.intensity = Mathf.Lerp(0, 1, float_LightLifeTimer / float_LightLife);
        }
    }

}
