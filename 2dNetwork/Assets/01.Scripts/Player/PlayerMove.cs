using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;
    public float rotateSpeed = 20f; //회전속도
    public float turretRotateSpeed = 30f; //터렛 회전속도

    public Transform turret;
    
    private PlayerInput playerInput;
    private Rigidbody2D rigid;
    private Vector3 moveDirection;
    private float rotateDir;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rigid = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        moveDirection = (transform.up * playerInput.frontMove).normalized;
        transform.rotation *= 
            Quaternion.Euler(0, 0, -playerInput.rightMove * rotateSpeed * Time.deltaTime);
    }
    private void Rotate()
    {
        Vector3 target = playerInput.mousePos;
        Vector3 v = target - transform.position; //방향벡터 구하고

        float degree = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        float rot = Mathf.LerpAngle(
                    turret.eulerAngles.z,
                    degree - 90,
                    Time.deltaTime * turretRotateSpeed);
        turret.eulerAngles = new Vector3(0, 0, rot);
    }

    private void FixedUpdate()
    {
        rigid.velocity = moveDirection * speed;
    }

    public void SetMoveScript(TankDataVO data)
    {
        speed = data.movingSpeed;
        rotateSpeed = data.rotateSpeed;
        turretRotateSpeed = data.turretRotateSpeed;
        
    }
}
