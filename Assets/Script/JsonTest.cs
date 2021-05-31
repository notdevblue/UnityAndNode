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


        //// 이것만 만질 것
        // FromJson
        // ToJson

        string json = " { \"type\":\"CHAT\",\"payload\":\"Hello Unity\"}";
        DataVO vo = JsonUtility.FromJson<DataVO>(json); // <= Json string 을 파싱 해서 넘김

        Debug.Log(vo.type);
        Debug.Log(vo.payload);
    }


    void Update()
    {
        
    }
}
