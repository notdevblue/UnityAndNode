using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleTest : MonoBehaviour
{

    public delegate void AddNumber(int x, int y);

    public event AddNumber addNumbers;

    void Start()
    {
        // ���¿� Ÿ���� ���ƾ� ��
        addNumbers += MyAdd;
        addNumbers += MyAdd2;
        addNumbers += (a, b) => Debug.Log(a - b);

        // �ѹ��� �̹�Ʈ �ݷ� + �� ��� ���� ���� ����ȴ�
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
