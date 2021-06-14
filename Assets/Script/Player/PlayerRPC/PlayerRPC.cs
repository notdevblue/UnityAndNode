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

    // 플레이어 입력과 이동 제어 컴포넌트
    private PlayerInput input;
    private PlayerMove move;
    private PlayerFire playerFire;

    private TankCategory tankCategory;

    private WaitForSeconds ws = new WaitForSeconds(1 / 5); // 200ms 간격으로 자신 데이터 갱신

    // 데이터 전송시 변경값들
    private Vector3 targetPosition;
    private Vector3 targetRotation;
    private Vector3 targetTurretRotation;

    public float lerpSpeed = 4.0f;

    private InfoUI ui = null;
    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        move = GetComponent<PlayerMove>();
        playerFire = GetComponent<PlayerFire>();
    }

    public void InitPlayer(Vector3 pos, TankCategory tank, InfoUI ui, bool remote = false)
    {
        bodyRenderer.sprite = bodys[(int)tank];
        turretRenderer.sprite = turrets[(int)tank];
        transform.position = pos;
        this.ui = ui;

        isRemote = remote;
        if(isRemote)
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
            move.SetMoveScript(GameManager.tankDataDic[tank]);

            StartCoroutine(SendData());
        }

        playerFire.SetFireScript(GameManager.tankDataDic[tank], remote);
    }

    IEnumerator SendData()
    {
        int socketId = GameManager.instance.socketId;

        while(true)
        {
            yield return ws;
            TransformVO vo = new TransformVO(transform.position, transform.rotation.eulerAngles, turretRenderer.transform.rotation.eulerAngles, socketId, tankCategory);
            string payload = JsonUtility.ToJson(vo);

            DataVO dataVO = new DataVO();
            dataVO.type = "TRANSFORM";
            dataVO.payload = payload;
            SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));


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
        if(isRemote)
        {
            // 
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed);
            float rot = Mathf.LerpAngle(transform.eulerAngles.z, targetRotation.z, Time.deltaTime * lerpSpeed);
            transform.eulerAngles = new Vector3(0, 0, rot);
            float tRot = Mathf.LerpAngle(turretRenderer.transform.eulerAngles.z, targetTurretRotation.z, Time.deltaTime * lerpSpeed);
            turretRenderer.transform.eulerAngles = new Vector3(0, 0, tRot);

        }
    }

    public void SetDisable()
    {
        gameObject.SetActive(false);
        ui.gameObject.SetActive(false);
    }
}
