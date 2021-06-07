using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : IPool where T : MonoBehaviour
{
    private Queue<T> m_queue;
    private GameObject prefab;
    private Transform parent;

    public ObjectPool(GameObject prefab, Transform parent, int count = 5)
    {
        this.prefab = prefab;
        this.parent = parent;
        m_queue = new Queue<T>();

        for(int i = 0; i < count; ++i)
        {
            GameObject obj = GameObject.Instantiate(prefab, parent);
            
            T t = obj.GetComponent<T>();
            obj.SetActive(false);
            m_queue.Enqueue(t);
        }
    }


    public T GetOrCreate()
    {
        T t = m_queue.Peek(); // Queue.Peek() => 맨 앞에 있는 원소를 봄
        if (t.gameObject.activeSelf)
        {
            GameObject temp = GameObject.Instantiate(prefab, parent);
            t = temp.GetComponent<T>();
        }
        else
        {
            t = m_queue.Dequeue(); // 사용 가능하니 그냥 뽑음
            t.gameObject.SetActive(true);
        }

        m_queue.Enqueue(t);
        return t;
    }
    
}
