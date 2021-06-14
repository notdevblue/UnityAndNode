using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public GameObject explosionPrefab;

    private void Awake()
    {
        PoolManager.CratePool<Explosion>(explosionPrefab, transform, 10);
    }

    public static Explosion GetExplosion()
    {
        return PoolManager.GetItem<Explosion>();
    }
}
