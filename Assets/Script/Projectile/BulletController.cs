using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public int damage = 5;
    public bool isEnemy = false;

    private Rigidbody2D rigid;
    public Vector2 dir;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void ResetData(Vector3 pos, Vector3 dir, float speed, int damage, bool isEnemy)
    {
        transform.position = pos;
        this.dir = dir.normalized;
        this.moveSpeed = speed;
        this.damage = damage;
        this.isEnemy = isEnemy;

        gameObject.layer = isEnemy ? GameManager.EnemyLayer : GameManager.PlayerLayer;
    }

    private void Update()
    {
        rigid.velocity = dir * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable id = collision.GetComponent<IDamageable>();

        if(id != null)
        {
            id.OnDamage(damage, dir, isEnemy);
        }

        Explosion exp = EffectsManager.GetExplosion();
        exp.ResetPos(transform.position);

        gameObject.SetActive(false);
    }
}
