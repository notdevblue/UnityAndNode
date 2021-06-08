using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconnHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        // socketid => int
        GameManager.DisconnectUser(int.Parse(payload));

    }


}
