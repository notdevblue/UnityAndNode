using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5.0f;
    public float rotateSpeed = 20.0f;
    public float turretRotateSpeed = 30.0f;

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

    private void Update()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        moveDirection = (transform.up * playerInput.frontMove).normalized;
        transform.rotation *= Quaternion.Euler(0,0, -playerInput.rightMove * rotateSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
        Vector3 target = playerInput.mousePos;
        Vector3 v = target - transform.position; // <= 방향백터

        float degree = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        float rot = Mathf.LerpAngle(turret.eulerAngles.z, degree - 90, Time.deltaTime * turretRotateSpeed);

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

