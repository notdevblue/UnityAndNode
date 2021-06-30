using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using DG.Tweening;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("탱크 프리팹")]
    public GameObject tankPrefab;
    public bool gameStart = false; //이 변수의 사용 용도는 조만간 알게 된다.
    private TransformVO data = null; //초기 로그인 완료시의 데이터

    [Header("시네머신 카메라")]
    public CinemachineVirtualCamera followCam;

    //소켓 관련
    [HideInInspector]
    public int socketId;
    private PlayerRPC rpc;

    [Header("게임오버관련 UI")]
    public CanvasGroup overPannel;
    public Text overText;

    public object lockObj = new object(); // 데이터 락킹을 위한 오브젝트

    //접속해있는 플레이어들을 저장하는 리스트
    private Dictionary<int, PlayerRPC> playerList = new Dictionary<int, PlayerRPC>();

    //접속해제 요청을 기록하는 큐
    private Queue<int> removeSocketQueue = new Queue<int>();

    //피격처리 요청을 기록하는 큐
    public Queue<HitInfoVO> hitQueue = new Queue<HitInfoVO>();

    //피격처리 요청을 기록하는 큐
    public Queue<DeadVO> deadQueue = new Queue<DeadVO>();

    //리스폰 처리 큐
    public Queue<int> respawnQueue = new Queue<int>();

    //적 탱크 위치와 회전데이터 저장 리스트
    private List<TransformVO> dataList;
    private bool needRefresh = false; //갱신필요여부

    //탱크들의 데이터 

    public static int PlayerLayer;
    public static int EnemyLayer;
    //     
    public static Dictionary<TankCategory, TankDataVO> tankDataDic
                                = new Dictionary<TankCategory, TankDataVO>();

    public static void InitGameData(string payload)
    {
        //payload를 읽어서 dictionary에 넣는것
        List<TankDataVO> list = JsonUtility.FromJson<TankDataListVO>(payload).tanks;
        foreach (TankDataVO t in list)
        {
            tankDataDic.Add(t.tank, t);
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("다수의 게임매니저가 실행되고 있습니다. 확인하세요");
        }
        instance = this;
        PoolManager.CreatePool<PlayerRPC>(tankPrefab, transform, 8); //8개의 탱크를 미리 만드는 풀

        PlayerLayer = LayerMask.NameToLayer("PLAYER");
        EnemyLayer = LayerMask.NameToLayer("ENEMY");
    }

    //스태틱 함수 리스트
    public static void GameStart(TransformVO data)
    {
        //Instantiate(instance.tankPrefab, data.position, Quaternion.identity);
        //instance.socketId = data.socketId;
        lock (instance.lockObj)
        {
            instance.data = data;
        }
    }
    public static void RecordDeadInfo(DeadVO vo)
    {
        lock (instance.lockObj)
        {
            instance.deadQueue.Enqueue(vo);
        }
    }

    public static void SetRefreshData(List<TransformVO> list)
    {
        lock (instance.lockObj)
        {
            instance.dataList = list;
            instance.needRefresh = true;
        }
    }


    public static void DisconnectUser(int socketId)
    {
        lock (instance.lockObj)
        {
            instance.removeSocketQueue.Enqueue(socketId);
        }
    }

    private void Update()
    {
        lock (lockObj)
        {
            if (!gameStart && data != null)  //로그인이 되서 데이터가 셋팅된거야
            {
                UIManager.CloseLoginPanel();

                rpc = PoolManager.GetItem<PlayerRPC>();
                socketId = data.socketId;

                InfoUI ui = UIManager.SetInfoUI(rpc.transform, data.name);

                rpc.InitPlayer(data.position, data.tank, ui, false); //내가 조종할 탱크로 설정
                followCam.Follow = rpc.transform;



                //플레이어 리스트 추가 -- 다음시간에 여기서부터 갑니다

                gameStart = true;
            }

            if (needRefresh)
            {
                //dataList의 값을 이용해서 현재 npc 를 갱신해줘야해 
                foreach (TransformVO tv in dataList)
                {
                    if (tv.socketId != socketId)  //갱신전송된 데이터가 내 데이터가 아니라면
                    {
                        PlayerRPC p = null;
                        playerList.TryGetValue(tv.socketId, out p);
                        if (p == null)
                        {
                            MakeRemotePlayer(tv);
                        }
                        else if (!p.isDead)
                        {
                            p.SetTransform(tv.position, tv.rotation, tv.turretRotation);
                        }

                        Debug.Log(tv.socketId + " , " + tv.kill + " , " + tv.death);
                    }
                }
                needRefresh = false;
            } //end of need refresh

            while (removeSocketQueue.Count > 0)
            {
                int soc = removeSocketQueue.Dequeue(); //하나 뽑아와서
                playerList[soc].SetDisable();
                playerList.Remove(soc); //해당 게임오브젝트를 리스트에서도 제거

            }

            while (hitQueue.Count > 0)
            {
                HitInfoVO hit = hitQueue.Dequeue();
                PlayerRPC rpc = playerList[hit.socketId]; //rpc에 hp를 셋팅하는 함수를 만들어야 해
                rpc.SetHP(hit.hp);
            }

            while (deadQueue.Count > 0)
            {
                DeadVO dead = deadQueue.Dequeue();
                PlayerRPC rpc = playerList[dead.socketId];
                rpc.SetDead();
            }

            while(respawnQueue.Count > 0)
            {
                int socID = respawnQueue.Dequeue();
                PlayerRPC rpc = playerList[socID];
                rpc.Respawn();
            }
        }
    } // end of update

    public PlayerRPC MakeRemotePlayer(TransformVO data)
    {
        PlayerRPC rpc = PoolManager.GetItem<PlayerRPC>();
        InfoUI ui = UIManager.SetInfoUI(rpc.transform, data.name);
        rpc.InitPlayer(data.position, data.tank, ui, true); //리모트 탱크 생성

        playerList.Add(data.socketId, rpc);
        return rpc;
    }

    public PlayerRPC GetPlayerRPC(int socketId)
    {
        return playerList[socketId];
    }

    public static void RecordHitInfo(HitInfoVO vo)
    {
        lock (instance.lockObj)
        {
            instance.hitQueue.Enqueue(vo);
        }
    }

    public static void RecordRespawnInfo(int socID)
    {
        lock(instance.lockObj)
        {
            instance.respawnQueue.Enqueue(socID);
        }
    }

    public void SetPlayerDead()
    {
        DOTween.To(() => overPannel.alpha, value => overPannel.alpha = value, 1.0f, 1.0f);

        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        for (int i = 10; i >= 0; --i)
        {
            overText.text = $"You died... \nWait For <color=ff0000>{i}</color> sec to respawn";
            yield return new WaitForSeconds(1.0f);
        }

        rpc.Respawn();
        DOTween.To(() => overPannel.alpha, value => overPannel.alpha = value, 0.0f, 1.0f);

    }
}
