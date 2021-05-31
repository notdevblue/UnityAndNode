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

    }



}
