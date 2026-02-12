using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Impact : EffectBase
{
    public ParticleSystem particleSystem_LongPixel;
    public ParticleSystem particleSystem_ArcPixel;
    public ParticleSystem particleSystem_BombPixel;
    public ParticleSystem particleSystem_CirclePixel;
    public ParticleSystem particleSystem_SlashPixel;
    public ParticleSystem particleSystem_StabPixel;
    /// <summary>
    /// Åü¿³
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="pro"></param>
    public void PlaySlash(Vector2 dir, bool pro = false)
    {
        particleSystem_SlashPixel.transform.right = dir;
        particleSystem_SlashPixel.Play();
        particleSystem_LongPixel.transform.right = dir;
        particleSystem_LongPixel.Play();
        particleSystem_BombPixel.Play();
        particleSystem_BombPixel.Play();
    }
    /// <summary>
    /// ¶Û»÷
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="pro"></param>
    public void PlayBludgeoning(Vector2 dir, bool pro = false)
    {
        particleSystem_BombPixel.Play();
        particleSystem_ArcPixel.transform.right = dir;
        particleSystem_ArcPixel.Play();
        if (pro)
        {
            particleSystem_CirclePixel.Play();
        }
    }
    /// <summary>
    /// ´Á´Ì
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="pro"></param>
    public void PlayPiercing(Vector2 dir,bool pro = false)
    {
        particleSystem_LongPixel.transform.right = dir;
        particleSystem_LongPixel.Play();
        particleSystem_BombPixel.Play();
        if (pro)
        {
            particleSystem_StabPixel.transform.right = dir;
            particleSystem_StabPixel.Play();
        }
    }
}
