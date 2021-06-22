using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        int socketId = int.Parse(payload);
        GameManager.RecordRespawnInfo(socketId);
    }
}
