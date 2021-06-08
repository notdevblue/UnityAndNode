using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("탱크 프리팹")]
    public GameObject tankPrefab;
    public bool gameStart = false;
    private TransformVO data = null; // 초기 로그인 완료시 데이터
    
    public CinemachineVirtualCamera followCam;

    // 소켓 관련
    [HideInInspector] public int socketId;
    [HideInInspector] public object crit = new object();

    // 적 탱크 위치와 회전데이터 저장 리스트
    private List<TransformVO> dataList;
    private bool needRefresh = false; // 갱신필요여부

    // 접속해있는 플레이어들을 저장하는 리스트
    private Dictionary<int, PlayerRPC> playerList = new Dictionary<int, PlayerRPC>();

    // 접속해체 요청 기록하는 큐
    private Queue<int> removeSocketQueue = new Queue<int>();

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("다수의 게임메니저 실행 중");
        }
        instance = this;
        PoolManager.CratePool<PlayerRPC>(tankPrefab, transform, 8); // 8개의 탱크를 미리 만드는 풀
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
            if(!gameStart && data != null) // 로그인 되서 데이터 세팅된 의미
            {
                UIManager.CloseLoginPanel();
                
                PlayerRPC rpc = PoolManager.GetItem<PlayerRPC>();
                socketId = data.socketId;
                InfoUI ui = UIManager.SetInfoUI(rpc.transform, data.name);
                rpc.InitPlayer(data.position, data.tank, ui, false); // 내가 조종할 탱크
                followCam.Follow = rpc.transform;

                

                gameStart = true;
            }

            if(needRefresh)
            {
                // dataList 값 이용해서 현재 npc 를 갱신해줘야함
                foreach(TransformVO tv in dataList)
                {
                    if(tv.socketId != socketId) // 갱신전송된 데이터가 내 데이터가 아니라면
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

                        // 플레이어 위치와 회전 세팅
                    }
                }
                needRefresh = false;
            } // end of needRefrersh

            while(removeSocketQueue.Count > 0)
            {
                int soc = removeSocketQueue.Dequeue();
                playerList[soc].SetDisable();
                playerList.Remove(soc); // 해당 게임오브젝트를 리스트에서도 제거
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
