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

    //이 함수는 나중에 Remote시에는 변경되어야 해.
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
                //이때는 쌍발식으로 나가야 해
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
        int socketId = GameManager.instance.socketId; //나의 소켓아이디를 가져와서
        FireInfoVO fireVO = new FireInfoVO(socketId, position, direction, speed, damage);

        string payload = JsonUtility.ToJson(fireVO);
        DataVO sendData = new DataVO();
        sendData.type = "FIRE";
        sendData.payload = payload;

        //소켓으로 전송
        SocketClient.SendDataToSocket(JsonUtility.ToJson(sendData));
    }
}