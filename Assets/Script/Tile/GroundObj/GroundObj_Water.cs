using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundObj_Water : GroundObj
{
    public Sprite[] sprite_Water;
    public Sprite[] sprite_Base;
    public SpriteRenderer spriteRenderer_Water;
    public SpriteRenderer spriteRenderer_Base;
    public override void Draw()
    {
        int i = GetInde(MapManager.Instance.CheckGround(groundTile.tileID, groundTile.tilePos));
        spriteRenderer_Water.sprite = sprite_Water[i];
        spriteRenderer_Base.sprite = sprite_Base[i];
        base.Draw();
    }
    private int GetInde(Around aroundState)
    {
        int val = -1;
        if (aroundState.U)
        {
            if (aroundState.D)
            {
                if (aroundState.L)
                {
                    if (aroundState.R)
                    {
                        if (aroundState.UL)
                        {
                            if (aroundState.UR)
                            {
                                if (aroundState.DL)
                                {
                                    if (aroundState.DR)
                                    {
                                        val = 8;
                                    }
                                    else
                                    {
                                        val = 11;
                                    }
                                }
                                else
                                {
                                    if (aroundState.DR)
                                    {
                                        val = 12;
                                    }
                                    else
                                    {
                                        val = 37;
                                    }
                                }
                            }
                            else
                            {
                                if (aroundState.DL)
                                {
                                    if (aroundState.DR)
                                    {
                                        val = 18;
                                    }
                                    else
                                    {
                                        val = 44;
                                    }
                                }
                                else
                                {
                                    if (aroundState.DR)
                                    {
                                        val = 34;
                                    }
                                    else
                                    {
                                        val = 46;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (aroundState.UR)
                            {
                                if (aroundState.DL)
                                {
                                    if (aroundState.DR)
                                    {
                                        val = 19;
                                    }
                                    else
                                    {
                                        val = 27;
                                    }
                                }
                                else
                                {
                                    if (aroundState.DR)
                                    {
                                        val = 38;
                                    }
                                    else
                                    {
                                        val = 39;
                                    }
                                }
                            }
                            else
                            {
                                if (aroundState.DL)
                                {
                                    if (aroundState.DR)
                                    {
                                        val = 45;
                                    }
                                    else
                                    {
                                        val = 47;
                                    }
                                }
                                else
                                {
                                    if (aroundState.DR)
                                    {
                                        val = 40;
                                    }
                                    else
                                    {
                                        val = 20;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (aroundState.UL)
                        {
                            if (aroundState.DL)
                            {
                                val = 9;
                            }
                            else
                            {
                                val = 24;
                            }
                        }
                        else
                        {
                            if (aroundState.DL)
                            {
                                val = 29;
                            }
                            else
                            {
                                val = 33;
                            }
                        }
                    }
                }
                else
                {
                    if (aroundState.R)
                    {
                        if (aroundState.UR)
                        {
                            if (aroundState.DR)
                            {
                                val = 7;
                            }
                            else
                            {
                                val = 21;
                            }
                        }
                        else
                        {
                            if (aroundState.DR)
                            {
                                val = 30;
                            }
                            else
                            {
                                val = 25;
                            }
                        }
                    }
                    else
                    {
                        val = 10;
                    }
                }
            }
            else
            {
                if (aroundState.L)
                {
                    if (aroundState.R)
                    {
                        if (aroundState.UL)
                        {
                            if (aroundState.UR)
                            {
                                val = 15;
                            }
                            else
                            {
                                val = 28;
                            }
                        }
                        else
                        {
                            if (aroundState.UR)
                            {
                                val = 31;
                            }
                            else
                            {
                                val = 32;
                            }
                        }

                    }
                    else
                    {
                        if (aroundState.UL)
                        {
                            val = 16;
                        }
                        else
                        {
                            val = 43;
                        }
                    }
                }
                else
                {
                    if (aroundState.R)
                    {
                        if (aroundState.UR)
                        {
                            val = 14;
                        }
                        else
                        {
                            val = 42;
                        }
                    }
                    else
                    {
                        val = 17;
                    }
                }
            }
        }
        else
        {
            if (aroundState.D)
            {
                if (aroundState.L)
                {
                    if (aroundState.R)
                    {
                        if (aroundState.DL)
                        {
                            if (aroundState.DR)
                            {
                                val = 1;
                            }
                            else
                            {
                                val = 23;
                            }
                        }
                        else
                        {
                            if (aroundState.DR)
                            {
                                val = 22;
                            }
                            else
                            {
                                val = 26;
                            }
                        }
                    }
                    else
                    {
                        if (aroundState.DL)
                        {
                            val = 2;
                        }
                        else
                        {
                            val = 36;
                        }
                    }
                }
                else
                {
                    if (aroundState.R)
                    {
                        if (aroundState.DR)
                        {
                            val = 0;
                        }
                        else
                        {
                            val = 35;
                        }
                    }
                    else
                    {
                        val = 3;
                    }
                }
            }
            else
            {
                if (aroundState.L)
                {
                    if (aroundState.R)
                    {
                        val = 5;
                    }
                    else
                    {
                        val = 6;
                    }
                }
                else
                {
                    if (aroundState.R)
                    {
                        val = 4;
                    }
                    else
                    {
                        val = 13;
                    }
                }
            }
        }
        return val;
    }
}
