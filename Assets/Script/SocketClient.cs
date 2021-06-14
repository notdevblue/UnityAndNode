using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class SocketClient : MonoBehaviour
{
    public string url = "ws://localhost";
    public int port = 56789;
    public GameObject handlers;


    // 컴퓨터 코드 한 줄은 atomic (원자성) 이 보장되지 않는다
    // 원자성 = 1 개의 연산을 보장하지 않음
    // 지난번에 서노 수업때 한 것

    private WebSocket webSocket;

    private static SocketClient instance = null;
    public static void SendDataToSocket(string json)
    {
        instance.SendData(json);
    }

    private Dictionary<string, IMsgHandler> handlerDictionary;

    private void Awake()
    {
        handlerDictionary = new Dictionary<string, IMsgHandler>();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // 처리할 프로토콜을 여기에 써주면 됨
        handlerDictionary.Add("CHAT", handlers.GetComponent<ChatHandler>());
        handlerDictionary.Add("LOGIN", handlers.GetComponent<LoginHandler>());
        handlerDictionary.Add("REFRESH", handlers.GetComponent<RefreshHandler>());
        handlerDictionary.Add("DISCONNECT", handlers.GetComponent<DisconnHandler>());
        handlerDictionary.Add("INITDATA", handlers.GetComponent<InitHandler>());

        DontDestroyOnLoad(gameObject);
        if(instance != null)
        {
            Debug.LogWarning("소켓 클라이언트가 여러개 실행되고 있습니다.");
        }
        instance = this;

        webSocket = new WebSocket($"{url}:{port}");
        webSocket.Connect();

        webSocket.OnMessage += (sender, e) =>
        {
            RecvData((WebSocket)sender, e);
        };
        
        //webSocket.Send("CHAT:Hello WebServer!");

        
    }

    private void RecvData(WebSocket sender, MessageEventArgs e)
    {
        DataVO vo = JsonUtility.FromJson<DataVO>(e.Data);

        IMsgHandler handler = null;
        // 값 존재하면 true,
        // 없으면 false
        if(handlerDictionary.TryGetValue(vo.type, out handler))
        {
            handler.HandleMsg(vo.payload);
        }
        else
        {
            Debug.LogWarning("존재하지 않은 프로토콜 요청 " + vo.type);
        }
    }

    private void SendData(string json)
    {
        webSocket.Send(json);

    }

    private void OnDestroy()
    {
        webSocket.Close();
    }

}
