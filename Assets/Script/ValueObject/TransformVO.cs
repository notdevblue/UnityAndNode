using UnityEngine;
using System;

[Serializable]
public class TransformVO
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 turretRotation;
    public int socketId;
    public TankCategory tank;
    
    public TransformVO(Vector3 pos, Vector3 rot, Vector3 turretRot, int socketId, TankCategory tank)
    {
        this.position       = pos;
        this.rotation       = rot;
        this.turretRotation = turretRot;
        this.socketId       = socketId;
        this.tank           = tank;
    }

    public TransformVO()
    {

    }

}
