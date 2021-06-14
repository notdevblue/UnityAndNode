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
    /// ��ƼŬ�� ��ǥ�� ���Ӱ� �������ִ� �ż���
    /// </summary>
    /// <param name="pos">��ƼŬ ��ġ</param>
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
