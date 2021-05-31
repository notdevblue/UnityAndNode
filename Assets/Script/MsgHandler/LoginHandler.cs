using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginHandler : MonoBehaviour, IMsgHandler
{
    private TankCategory tank;
    public Image[] selectImageHolders; // 선택 이미지 저장
    public Sprite[] selectedImages; // O, X 스프라이트
    public InputField nameInput; // 이름 입력한 인풋 필드

    public Button redTankBtn;
    public Button blueTankBtn;

    public Button loginBtn;
    // TODO : 로그인 하면 데이터 전송하는거
    // 잘못된 값을 입력하면 팝업. Dotween 깔쌈하게
    // Login 성공 시 게임 스테이지
    // 게임 스테이지 타일맵으로 만들기

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
