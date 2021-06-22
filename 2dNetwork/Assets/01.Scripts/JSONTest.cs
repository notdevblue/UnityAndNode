using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //FromJson
        string json = "{ \"type\":\"CHAT\",\"payload\":\"Hello Unity\"}";
        DataVO vo = JsonUtility.FromJson<DataVO>(json);


        Debug.Log(vo.type);
        Debug.Log(vo.payload);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
