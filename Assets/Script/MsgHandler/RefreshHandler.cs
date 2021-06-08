using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        // payload�� C# Ŭ������ �Ľ��ؼ� ó���ؾ���
        RefreshVO vo = JsonUtility.FromJson<RefreshVO>(payload);

        GameManager.SetRefreshData(vo.dataList); // �����͸���Ʈ ó���ϰ� �����ش�.


    }
}
