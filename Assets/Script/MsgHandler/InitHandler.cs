using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        GameManager.InitGameData(payload);
    }

}
