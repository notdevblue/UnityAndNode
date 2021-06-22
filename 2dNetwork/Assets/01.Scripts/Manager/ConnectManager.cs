using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectManager : MonoBehaviour
{
    public InputField txtIp;
    public InputField txtPort;

    public Button btnConnect;
    public CanvasGroup connectPanel;

    bool isConnected = false;

    private void Start()
    {
        btnConnect.onClick.AddListener(() =>
        {
            if (isConnected) return;
            if(txtPort.text == "" || txtIp.text == "")
            {
                PopUpManager.OpenPopup(IconCategory.ERR, "필수 값이 빠져 있습니다.");
                return;
            }


            SocketClient.instance.ConnectSocket(txtIp.text, txtPort.text);

            connectPanel.alpha = 0.0f;
            connectPanel.interactable = false;
            connectPanel.blocksRaycasts = false;

            UIManager.OpenLoginPannel();
        });
    }
}
