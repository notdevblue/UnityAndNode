using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        //넘겨준 소켓 id를 int형으로 변경해서 게임매니저에게 처리하라고 넘긴다.
        GameManager.DisconnectUser(int.Parse(payload)); 
    }
}
