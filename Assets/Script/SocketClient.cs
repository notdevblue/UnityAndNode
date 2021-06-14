using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class SocketClient : MonoBehaviour
{
    public string url = "ws://localhost";
    public int port = 56789;
    public GameObject handlers;


    // ��ǻ�� �ڵ� �� ���� atomic (���ڼ�) �� ������� �ʴ´�
    // ���ڼ� = 1 ���� ������ �������� ����
    // �������� ���� ������ �� ��

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
        // ó���� ���������� ���⿡ ���ָ� ��
        handlerDictionary.Add("CHAT", handlers.GetComponent<ChatHandler>());
        handlerDictionary.Add("LOGIN", handlers.GetComponent<LoginHandler>());
        handlerDictionary.Add("REFRESH", handlers.GetComponent<RefreshHandler>());
        handlerDictionary.Add("DISCONNECT", handlers.GetComponent<DisconnHandler>());
        handlerDictionary.Add("INITDATA", handlers.GetComponent<InitHandler>());

        DontDestroyOnLoad(gameObject);
        if(instance != null)
        {
            Debug.LogWarning("���� Ŭ���̾�Ʈ�� ������ ����ǰ� �ֽ��ϴ�.");
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
        // �� �����ϸ� true,
        // ������ false
        if(handlerDictionary.TryGetValue(vo.type, out handler))
        {
            handler.HandleMsg(vo.payload);
        }
        else
        {
            Debug.LogWarning("�������� ���� �������� ��û " + vo.type);
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
