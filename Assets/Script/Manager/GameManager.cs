using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("��ũ ������")]
    public GameObject tankPrefab;
    public bool gameStart = false;
    private TransformVO data = null; // �ʱ� �α��� �Ϸ�� ������
    
    public CinemachineVirtualCamera followCam;

    // ���� ����
    [HideInInspector] public int socketId;
    [HideInInspector] public object crit = new object();

    private Dictionary<int, PlayerRPC> playerList = new Dictionary<int, PlayerRPC>();


    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("�ټ��� ���Ӹ޴��� ���� ��");
        }
        instance = this;
        PoolManager.CratePool<PlayerRPC>(tankPrefab, transform, 8); // 8���� ��ũ�� �̸� ����� Ǯ
    }

    public static void GameStart(TransformVO data)
    {
        //Instantiate(instance.tankPrefab, data.position, Quaternion.identity);
        //instance.socketId = data.socketId;

        lock(instance.crit)
        {
            instance.data = data;
        }
    }

    private void Update()
    {
        lock(crit)
        {
            if(!gameStart && data != null) // �α��� �Ǽ� ������ ���õ� �ǹ�
            {
                UIManager.CloseLoginPanel();
                
                PlayerRPC rpc = PoolManager.GetItem<PlayerRPC>();
                socketId = data.socketId;
                rpc.InitPlayer(data.position, data.tank, false); // ���� ������ ��ũ
                followCam.Follow = rpc.transform;

                gameStart = true;
            }
        }
    }
}
