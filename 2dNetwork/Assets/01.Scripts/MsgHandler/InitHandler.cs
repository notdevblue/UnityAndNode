using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        //넘겨준 데이터를 처리해서 저장하도록 게임매니저의 함수를 실행한다.
        GameManager.InitGameData(payload);
    }
}
