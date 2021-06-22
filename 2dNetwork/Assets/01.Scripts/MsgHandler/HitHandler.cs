using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        HitInfoVO hitVO = JsonUtility.FromJson<HitInfoVO>(payload);

        //// 해당하는 소켓의 rpc를 가져오고
        //PlayerRPC rpc = GameManager.instance.GetPlayerRPC(hitVO.socketId);
        GameManager.RecordHitInfo(hitVO);

    }
}
