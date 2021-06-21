using System;
using UnityEngine;

[Serializable]
public class FireInfoVO
{
    public int socketId;
    public Vector3 position;
    public Vector3 direction;
    public float speed;
    public int damage;

    public FireInfoVO(int socketId, Vector3 position, Vector3 direction, float speed, int damage)
    {
        this.socketId   = socketId;
        this.position   = position;
        this.direction  = direction;
        this.speed      = speed;
        this.damage     = damage;
    }
}
