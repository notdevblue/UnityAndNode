using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public Transform firePosition;

    private TankCategory tankCategory = TankCategory.Red;
    private float bulletSpeed = 10f;
    private int bulletDamage = 5;
    private bool isEnemy = false;

    private PlayerInput input;

    public float timeBetFire = 1f;
    public float lastFireTime = 0;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        lastFireTime = Time.time;
    }

    public void SetFireScript(TankDataVO data, bool isEnemy)
    {
        tankCategory = data.tank;
        bulletSpeed = data.bulletSpeed;
        bulletDamage = data.damage;
        this.isEnemy = isEnemy;
        timeBetFire = data.timeBetFire;
    }


    private void Update()
    {
        if (input.fire && lastFireTime + timeBetFire < Time.time)
        {
            Fire();
            lastFireTime = Time.time;
        }
    }

    //�� �Լ��� ���߿� Remote�ÿ��� ����Ǿ�� ��.
    public void Fire()
    {
        switch (tankCategory)
        {
            case TankCategory.Blue:
                BulletController bc = BulletManager.GetBullet();
                bc.ResetData(firePosition.position, firePosition.up, bulletSpeed, bulletDamage, isEnemy);
                SendFireData(firePosition.position, firePosition.up, bulletSpeed, bulletDamage);
                break;
            case TankCategory.Red:
                //�̶��� �ֹ߽����� ������ ��
                for (int i = 0; i < 2; i++)
                {
                    BulletController bc2 = BulletManager.GetBullet();
                    bc2.ResetData(firePosition.position + firePosition.right * (i * 0.3f - 0.15f),
                        firePosition.up, bulletSpeed, bulletDamage, isEnemy);
                    SendFireData(firePosition.position + firePosition.right * (i * 0.3f - 0.15f),
                        firePosition.up, bulletSpeed, bulletDamage);
                }
                break;
        }
    }


    private void SendFireData(Vector3 position, Vector3 direction, float speed, int damage)
    {
        int socketId = GameManager.instance.socketId; //���� ���Ͼ��̵� �����ͼ�
        FireInfoVO fireVO = new FireInfoVO(socketId, position, direction, speed, damage);

        string payload = JsonUtility.ToJson(fireVO);
        DataVO sendData = new DataVO();
        sendData.type = "FIRE";
        sendData.payload = payload;

        //�������� ����
        SocketClient.SendDataToSocket(JsonUtility.ToJson(sendData));
    }
}