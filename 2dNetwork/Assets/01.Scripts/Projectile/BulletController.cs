using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public int damage = 5;
    public bool isEnemy = false; //피아식별

    Rigidbody2D rigid;
    public Vector2 dir;

    private int ownerId;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void ResetData(int owner, Vector3 position, Vector2 dir, float speed, int damage, bool isEnemy)
    {
        ownerId = owner;
        transform.position = position;
        this.dir = dir.normalized;
        this.moveSpeed = speed;
        this.damage = damage;
        this.isEnemy = isEnemy;

        gameObject.layer = isEnemy ? GameManager.EnemyLayer : GameManager.PlayerLayer;
    }

    void Update()
    {
        rigid.velocity = dir * moveSpeed;        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable id = collision.GetComponent<IDamageable>();
        if(id != null)
        {
            id.OnDamage(damage, dir, isEnemy, ownerId);
        }

        Explosion exp = EffectManager.GetExplosion();
        exp.ResetPosition(transform.position);

        gameObject.SetActive(false);
    }
}
