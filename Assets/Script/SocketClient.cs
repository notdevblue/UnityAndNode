using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class SocketClient : MonoBehaviour
{
    public string url = "ws://localhost";
    public int port = 32000;
    public GameObject handlers;

    private WebSocket webSocket; //웹소켓 인스턴스

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
        if (instance != null)
        {
            Debug.LogWarning("소켓 클라이언트가 여러개 실행되고 있습니다.");
        }
        instance = this;
    }

    void Start()
    {
        //이런식으로 처리할 프로토콜을 여기에 써주면 돼
        handlerDictionary.Add("CHAT", handlers.GetComponent<ChatHandler>());
        handlerDictionary.Add("LOGIN", handlers.GetComponent<LoginHandler>());
        handlerDictionary.Add("REFRESH", handlers.GetComponent<RefreshHandler>());
        handlerDictionary.Add("DISCONNECT", handlers.GetComponent<DisconnHandler>());
        handlerDictionary.Add("INITDATA", handlers.GetComponent<InitHandler>());
        handlerDictionary.Add("FIRE", handlers.GetComponent<FireHandler>());


        webSocket = new WebSocket($"{url}:{port}");
        webSocket.Connect();

        webSocket.OnMessage += (sender, e) =>
        {
            ReceiveData((WebSocket)sender, e);
        };

        //webSocket.Send("CHAT:Hello WebServer");
    }

    private void ReceiveData(WebSocket sender, MessageEventArgs e)
    {
        DataVO vo = JsonUtility.FromJson<DataVO>(e.Data);

        IMsgHandler handler = null;
        if (handlerDictionary.TryGetValue(vo.type, out handler))
        {
            handler.HandleMsg(vo.payload);
        }
        else
        {
            Debug.LogWarning("존재하지 않는 프로토콜 요청 " + vo.type);
        }
    }

    public void SendData(string json)
    {
        webSocket.Send(json);
    }

    private void OnDestroy()
    {
        webSocket.Close();
    }
}