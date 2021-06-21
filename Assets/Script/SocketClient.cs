using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class SocketClient : MonoBehaviour
{
    public string url = "ws://localhost";
    public int port = 32000;
    public GameObject handlers;

    private WebSocket webSocket; //������ �ν��Ͻ�

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
            Debug.LogWarning("���� Ŭ���̾�Ʈ�� ������ ����ǰ� �ֽ��ϴ�.");
        }
        instance = this;
    }

    void Start()
    {
        //�̷������� ó���� ���������� ���⿡ ���ָ� ��
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
            Debug.LogWarning("�������� �ʴ� �������� ��û " + vo.type);
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