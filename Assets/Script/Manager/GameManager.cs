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

    // �� ��ũ ��ġ�� ȸ�������� ���� ����Ʈ
    private List<TransformVO> dataList;
    private bool needRefresh = false; // �����ʿ俩��

    // �������ִ� �÷��̾���� �����ϴ� ����Ʈ
    private Dictionary<int, PlayerRPC> playerList = new Dictionary<int, PlayerRPC>();

    // ������ü ��û ����ϴ� ť
    private Queue<int> removeSocketQueue = new Queue<int>();

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

    public static void SetRefreshData(List<TransformVO> list)
    {
        lock(instance.crit)
        {
            instance.dataList = list;
            instance.needRefresh = true;
        }
    }

    public static void DisconnectUser(int socketId)
    {
        lock(instance.crit)
        {
            instance.removeSocketQueue.Enqueue(socketId);
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
                InfoUI ui = UIManager.SetInfoUI(rpc.transform, data.name);
                rpc.InitPlayer(data.position, data.tank, ui, false); // ���� ������ ��ũ
                followCam.Follow = rpc.transform;

                

                gameStart = true;
            }

            if(needRefresh)
            {
                // dataList �� �̿��ؼ� ���� npc �� �����������
                foreach(TransformVO tv in dataList)
                {
                    if(tv.socketId != socketId) // �������۵� �����Ͱ� �� �����Ͱ� �ƴ϶��
                    {
                        PlayerRPC p = null;
                        playerList.TryGetValue(tv.socketId, out p);
                        if(p == null)
                        {
                            MakeRemotePlayer(tv);
                        }
                        else
                        {
                            p.SetTransform(tv.position, tv.rotation, tv.turretRotation);
                        }

                        // �÷��̾� ��ġ�� ȸ�� ����
                    }
                }
                needRefresh = false;
            } // end of needRefrersh

            while(removeSocketQueue.Count > 0)
            {
                int soc = removeSocketQueue.Dequeue();
                playerList[soc].SetDisable();
                playerList.Remove(soc); // �ش� ���ӿ�����Ʈ�� ����Ʈ������ ����
            }
        }
    } // end of update

    public PlayerRPC MakeRemotePlayer(TransformVO data)
    {
        PlayerRPC rpc = PoolManager.GetItem<PlayerRPC>();
        InfoUI ui = UIManager.SetInfoUI(rpc.transform, data.name);
        rpc.InitPlayer(data.position, data.tank, ui, true);
        playerList.Add(data.socketId, rpc);

        return rpc;
    }
}
