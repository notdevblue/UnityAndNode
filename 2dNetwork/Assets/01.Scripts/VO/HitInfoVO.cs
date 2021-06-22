using System;
using UnityEngine;

[Serializable]
public class HitInfoVO
{
    public int socketId;
    public int hp;

    public HitInfoVO(int socketId, int hp)
    {
        this.socketId = socketId;
        this.hp = hp;
    }



}
