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

    public TransformVO transform;

    //여기에서 사실 하나 더 들어가야 한다. 

    public FireInfoVO(int socketId, Vector3 position, Vector3 direction, float speed, int damage, TransformVO transform)
    {
        this.socketId = socketId;
        this.position = position;
        this.direction = direction;
        this.speed = speed;
        this.damage = damage;
        this.transform = transform;
    }
}
