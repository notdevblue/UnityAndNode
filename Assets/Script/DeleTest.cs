using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleTest : MonoBehaviour
{

    public delegate void AddNumber(int x, int y);

    public event AddNumber addNumbers;

    void Start()
    {
        // 형태와 타입이 같아야 함
        addNumbers += MyAdd;
        addNumbers += MyAdd2;
        addNumbers += (a, b) => Debug.Log(a - b);

        // 한번에 이밴트 콜로 + 한 모든 것이 전부 실행된다
        addNumbers(3, 4);

    }

    private void MyAdd(int a, int b)
    {
        Debug.Log(a + b);
    }

    private void MyAdd2(int a, int b)
    {
        Debug.Log(a * b);
    }

    
    void Update()
    {
        
    }
}
