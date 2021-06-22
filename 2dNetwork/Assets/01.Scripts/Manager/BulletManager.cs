using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public GameObject bulletPrefab;

    private void Awake()
    {
        PoolManager.CreatePool<BulletController>(bulletPrefab, transform, 30);
    }

    public static BulletController GetBullet()
    {
        return PoolManager.GetItem<BulletController>();
    }
}
