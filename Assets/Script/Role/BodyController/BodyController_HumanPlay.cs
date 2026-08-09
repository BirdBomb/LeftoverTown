using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController_HumanPlay : BodyController_Human
{
    public ParticleSystem particleSystem_Dust_Left = null;
    public ParticleSystem particleSystem_Dust_Right = null;
    public ParticleSystem particleSystem_Dust_Small = null;
    public ParticleSystem particleSystem_SpeedLine = null;
    private bool bool_StepLeft = false;
    private float float_rotateSpeed = 5;
    private const float float_SpeedLineMinSpeed = 5;
    public override void PlayStep()
    {
        if (bool_StepLeft) particleSystem_Dust_Left?.Play();
        else particleSystem_Dust_Right?.Play();
        particleSystem_Dust_Small?.Play();
        bool_StepLeft = !bool_StepLeft;
        base.PlayStep();
    }
    public override void PlaySpeedEffect(float dt)
    {
        particleSystem_SpeedLine.transform.right = Vector3.Slerp(particleSystem_SpeedLine.transform.right, vector2_Dir, float_rotateSpeed * dt);

        if (float_Speed > float_SpeedLineMinSpeed) particleSystem_SpeedLine?.Play();
        else particleSystem_SpeedLine?.Stop();
        base.PlaySpeedEffect(dt);
    }
}
