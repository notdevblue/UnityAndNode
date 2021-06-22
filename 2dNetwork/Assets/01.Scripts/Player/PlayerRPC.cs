using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRPC : MonoBehaviour
{
    public Sprite[] bodys;
    public Sprite[] turrets;

    public SpriteRenderer bodyRenderer;
    public SpriteRenderer turretRenderer;
    public bool isRemote = false;
    public bool isDead = false;

    private BoxCollider2D coll;
    private SpriteRenderer[] renderers;

    //플레이어 입력과 이동을 제어하기 위한 컴포넌트
    private PlayerInput input;
    private PlayerMove move;
    private PlayerFire playerFire;
    private PlayerHealth health;

    private TankCategory tankCategory;

    private WaitForSeconds ws = new WaitForSeconds(0.1f);
    // 200ms 간격으로 자신의 데이터 갱신

    //데이터 전송시 변경값들
    private Vector3 targetPosition;
    private Vector3 targetRotation;
    private Vector3 targetTurretRotation;

    public float lerpSpeed = 4f;

    private InfoUI ui = null;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        move = GetComponent<PlayerMove>();
        playerFire = GetComponent<PlayerFire>();
        health = GetComponent<PlayerHealth>();
        coll = GetComponent<BoxCollider2D>();
        renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public void InitPlayer(Vector3 pos, TankCategory tank, InfoUI ui, bool remote = false)
    {
        bodyRenderer.sprite = bodys[(int)tank];
        turretRenderer.sprite = turrets[(int)tank];
        tankCategory = tank;
        this.ui = ui;

        transform.position = pos;

        isRemote = remote;
        if (isRemote)
        {
            input.enabled = false;
            move.enabled = false;
            gameObject.layer = GameManager.EnemyLayer;
        }
        else
        {
            input.enabled = true;
            move.enabled = true;
            gameObject.layer = GameManager.PlayerLayer;
            move.SetMoveScript( GameManager.tankDataDic[tank] ); //이동속도 등을 셋팅해야해
            StartCoroutine(SendData());
        }
        health.SetHealthScript(GameManager.tankDataDic[tank], remote, ui);
        playerFire.SetFireScript(GameManager.tankDataDic[tank], remote);
        //플레이어의 체력관리 스크립트도 들어가야해
    }

    IEnumerator SendData()
    {
        //if(isDead) { yield return  }

        int socketId = GameManager.instance.socketId;

        while(true)
        {
            yield return ws; //0.2f 대기후
            TransformVO vo = new TransformVO(
                transform.position,
                transform.rotation.eulerAngles, 
                turretRenderer.transform.rotation.eulerAngles, 
                socketId, 
                tankCategory
            ); //여기 값들이 들어가야 해
            string payload = JsonUtility.ToJson(vo);

            DataVO dataVo = new DataVO();
            dataVo.type = "TRANSFORM";
            dataVo.payload = payload;
            SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVo));
        }   
    }


    public void SetTransform(Vector3 pos, Vector3 rotation, Vector3 turretRotation)
    {
        if (isRemote)
        {
            targetPosition = pos;
            targetRotation = rotation;
            targetTurretRotation = turretRotation;
        }
    }

    private void Update()
    {
        if (isDead) return;

        if(isRemote)
        {
            transform.position = Vector3.Lerp(
                transform.position, 
                targetPosition, 
                Time.deltaTime * lerpSpeed);

            float rot = Mathf.LerpAngle(
                transform.eulerAngles.z, 
                targetRotation.z, 
                Time.deltaTime * lerpSpeed);
            transform.eulerAngles = new Vector3(0,0,rot);

            float tRot = Mathf.LerpAngle(
                turretRenderer.transform.eulerAngles.z,
                targetTurretRotation.z,
                Time.deltaTime * lerpSpeed);
            turretRenderer.transform.eulerAngles = new Vector3(0, 0, tRot);
        }
    }

    public void SetDisable()
    {
        gameObject.SetActive(false);
        ui.gameObject.SetActive(false);
        //여기서 UI도 꺼줘야 해.
    }

    public void SetDead()
    {
        isDead = true;
        health.Die();

        SetScript(false);
    }

    public void SetScript(bool on)
    {
        input.enabled = on && !isRemote;
        coll.enabled = on;

        foreach(SpriteRenderer s in renderers)
        {
            s.enabled = true;
        }
        ui.SetVisible(on);
    }

    public void SetHP(int hp)
    {
        //여기서 playerHealth의 hp를 셋팅하고 UpdateUI 만들어서 UI갱신도 이뤄지게 해라
        health.currentHP = hp;
        health.UpdateUI();
    }

    public void Respawn()
    {
        SetScript(true);
        isDead = false;
        health.currentHP = health.maxHP;
        health.UpdateUI();

        if (!isRemote)
        {
            DataVO data = new DataVO();
            data.payload = GameManager.instance.socketId + "";
            data.type = "RESPAWN";
            SocketClient.SendDataToSocket(JsonUtility.ToJson(data));
        }
    }
}
