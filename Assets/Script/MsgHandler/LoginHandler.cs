using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginHandler : MonoBehaviour, IMsgHandler
{
    private TankCategory tank;
    public Image[] selectImageHolders; // ���� �̹��� ����
    public Sprite[] selectedImages; // O, X ��������Ʈ
    public InputField nameInput; // �̸� �Է��� ��ǲ �ʵ�

    public Button redTankBtn;
    public Button blueTankBtn;

    public Button loginBtn;
    // TODO : �α��� �ϸ� ������ �����ϴ°�
    // �߸��� ���� �Է��ϸ� �˾�. Dotween ����ϰ�
    // Login ���� �� ���� ��������
    // ���� �������� Ÿ�ϸ����� �����

    private void Awake()
    {
        tank = TankCategory.Red;
        redTankBtn.onClick.AddListener(() =>
        {
            tank = TankCategory.Red;
            foreach (Image img in selectImageHolders)
            {
                img.sprite = selectedImages[1];
            }

            selectImageHolders[(int)TankCategory.Red].sprite = selectedImages[0];
        });

        loginBtn.onClick.AddListener(() =>
        {
            string name = nameInput.text;

            if(name == "")
            {
                PopupManager.OpenPopup(IconCategory.SYSTEM, "�̸��� �ݵ�� �Է��ؾ� �մϴ�.");
                return;
            }

            LoginVO loginVO = new LoginVO(tank, name);
            string payload = JsonUtility.ToJson(loginVO);
            
            DataVO dataVO = new DataVO();
            dataVO.type = "LOGIN";
            dataVO.payload = payload;
            string json = JsonUtility.ToJson(dataVO);

            SocketClient.SendDataToSocket(json);
        });

        blueTankBtn.onClick.AddListener(() =>
        {
            tank = TankCategory.Blue;
            foreach(Image img in selectImageHolders)
            {
                img.sprite = selectedImages[1];
            }

            selectImageHolders[(int)TankCategory.Blue].sprite = selectedImages[0];
        });
    }

    public void HandleMsg(string payload)
    {
        TransformVO vo = JsonUtility.FromJson<TransformVO>(payload);

        //GameManager.StartGame();

    }



}
