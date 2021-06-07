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

    private Dictionary<int, PlayerRPC> playerList = new Dictionary<int, PlayerRPC>();


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

    private void Update()
    {
        lock(crit)
        {
            if(!gameStart && data != null) // 로그인 되서 데이터 세팅된 의미
            {
                UIManager.CloseLoginPanel();
                
                PlayerRPC rpc = PoolManager.GetItem<PlayerRPC>();
                socketId = data.socketId;
                rpc.InitPlayer(data.position, data.tank, false); // 내가 조종할 탱크
                followCam.Follow = rpc.transform;

                gameStart = true;
            }
        }
    }
}
