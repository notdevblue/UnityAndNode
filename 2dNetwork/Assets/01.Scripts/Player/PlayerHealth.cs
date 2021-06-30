using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private PlayerRPC rpc;
    public int maxHP;
    public int currentHP;

    public bool isEnemy = false;
    public InfoUI ui;

    private void Awake()
    {
        rpc = GetComponent<PlayerRPC>();
    }

    //rpc에서 이걸 돌려야 한다.
    public void SetHealthScript(TankDataVO data, bool isEnemy, InfoUI ui)
    {
        maxHP = data.maxHP;
        currentHP = maxHP;
        this.isEnemy = isEnemy;
        this.ui = ui;
    }

    public void OnDamage(int damage, Vector2 powerDir, bool isEnemy, int shooterId)
    {
        if (rpc.isRemote) return; // 원격 탱크는 HP를 건드리지 않는다

        //반동 처리는 현재는 구현하지 않는다.
        currentHP -= damage;
        UpdateUI();
        //HitInfoVO 라는 녀석을 소켓으로 쏴줘야 한다
        HitInfoVO vo = new HitInfoVO(GameManager.instance.socketId, currentHP);
        string payload = JsonUtility.ToJson(vo);
        DataVO dataVO = new DataVO();
        dataVO.type = "HIT";
        dataVO.payload = payload;

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));

        if(currentHP <= 0)
        {
            Die(shooterId);
        }
    }

    /// <summary>
    /// 체력 게이지 업데이트 해주는 매서드
    /// </summary>
    public void UpdateUI()
    {
        ui.UpdateHPBar((float)currentHP / maxHP);
    }

    public void Die(int shooterId = 0)
    {
        if (rpc.isDead) return;

        //폭발 이펙트 재생이 필요하다
        MassiveExplosion mExp = EffectManager.GetMassiveExplosion();
        mExp.ResetPosition(transform.position);

        //gameObject.SetActive(false);
        //ui.gameObject.SetActive(false);

        if(!isEnemy)
        {
            DeadVO vo = new DeadVO(GameManager.instance.socketId, shooterId);
            string payload = JsonUtility.ToJson(vo);
            DataVO dataVO = new DataVO();
            dataVO.type = "DEAD";
            dataVO.payload = payload;
            Debug.Log(JsonUtility.ToJson(dataVO));
            SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));

            GameManager.instance.SetPlayerDead();
            rpc.isDead = true;
            rpc.SetScript(false);
        }


        //isEnemy가 false였다면 게임오버 화면이 필요하다
    }
}
