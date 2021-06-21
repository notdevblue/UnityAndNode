using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHandler : MonoBehaviour, IMsgHandler
{
    private Queue<FireInfoVO> dataQueue = new Queue<FireInfoVO>();
    public object lockObj = new object();

    public void HandleMsg(string payload)
    {
        FireInfoVO vo = JsonUtility.FromJson<FireInfoVO>(payload);
        lock (lockObj)
        {
            dataQueue.Enqueue(vo);
        }
    }

    void Update()
    {
        lock (lockObj)
        {
            if (dataQueue.Count > 0)
            {
                FireInfoVO vo = dataQueue.Dequeue();
                BulletController bc = BulletManager.GetBullet();
                bc.ResetData(vo.position, vo.direction, vo.speed, vo.damage, true);

                //여기서도 뭔가를 해줘야 해.
            }
        }
    }
}