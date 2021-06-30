using System;

[Serializable]
public class DeadVO
{
    public int socketId;
    public int killerId;

    public DeadVO(int id, int killerId) 
    { 
        socketId = id;
        this.killerId = killerId;
    }
}
