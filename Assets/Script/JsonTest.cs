using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonTest : MonoBehaviour
{
    


    void Start()
    {
        // FromJson
        // FromJsonOverwrite
        // ToJson


        //// �̰͸� ���� ��
        // FromJson
        // ToJson

        string json = " { \"type\":\"CHAT\",\"payload\":\"Hello Unity\"}";
        DataVO vo = JsonUtility.FromJson<DataVO>(json); // <= Json string �� �Ľ� �ؼ� �ѱ�

        Debug.Log(vo.type);
        Debug.Log(vo.payload);
    }


    void Update()
    {
        
    }
}
