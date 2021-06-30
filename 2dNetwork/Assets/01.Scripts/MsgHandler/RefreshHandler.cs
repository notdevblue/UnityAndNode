﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        //payload를 c# 클래스로 파싱해서 처리해야해
        RefreshVO vo = JsonUtility.FromJson<RefreshVO>(payload);
        GameManager.SetRefreshData(vo.dataList); //데이터리스트를 처리하라고 보내준다.
        ScoreBoardManager.ResetBoard(vo.dataList);
    }
}
