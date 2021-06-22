using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public GameObject explosionPrefab;
    public GameObject massivePrefab;

    private void Awake()
    {
        PoolManager.CreatePool<Explosion>(explosionPrefab, transform, 10);
        PoolManager.CreatePool<MassiveExplosion>(massivePrefab, transform, 5);
    }

    public static Explosion GetExplosion()
    {
        return PoolManager.GetItem<Explosion>();
    }

    public static MassiveExplosion GetMassiveExplosion()
    {
        return PoolManager.GetItem<MassiveExplosion>();
    }
}
