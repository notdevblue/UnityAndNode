using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    
    protected ParticleSystem particle;

    protected void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }
    
    /// <summary>
    /// 파티클의 좌표를 새롭게 생성해주는 매서드
    /// </summary>
    /// <param name="position">파티클의 위치 변수 Vector3 타입</param>
    public void ResetPosition(Vector3 position)
    {
        transform.position = position;
        particle.Play();
        Invoke("SetDisable", 3f);
    }

    private void SetDisable()
    {
        gameObject.SetActive(false);
    }
}
