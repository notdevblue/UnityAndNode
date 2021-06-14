using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// 파티클의 좌표를 새롭게 생성해주는 매서드
    /// </summary>
    /// <param name="pos">파티클 위치</param>
    public void ResetPos(Vector3 pos)
    {
        transform.position = pos;
        particle.Play();
        Invoke(nameof(SetDisable), 3.0f);
    }

    private void SetDisable()
    {
        gameObject.SetActive(false);
    }

}
