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

    // �÷��̾� �Է°� �̵� ���� ������Ʈ
    private PlayerInput input;
    private PlayerMove move;

    private TankCategory tankCategory;

    private WaitForSeconds ws = new WaitForSeconds(1 / 5); // 200ms �������� �ڽ� ������ ����

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        move = GetComponent<PlayerMove>();
    }

    public void InitPlayer(Vector3 pos, TankCategory tank, bool remote = false)
    {
        bodyRenderer.sprite = bodys[(int)tank];
        turretRenderer.sprite = turrets[(int)tank];

        isRemote = remote;
        if(isRemote)
        {
            input.enabled = false;
            move.enabled = false;
        }
        else
        {
            input.enabled = true;
            move.enabled = true;
            //StartCoroutine(SendData());
        }


    }


    IEnumerator SendData()
    {
        int socketId = 0;

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
}
