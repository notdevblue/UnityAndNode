using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginHandler : MonoBehaviour, IMsgHandler
{
    private TankCategory tank;
    public Image[] selectImageHolders; //선택한 이미지들을 저장하는 공간
    public Sprite[] selectImages; // O, X 스프라이트
    public InputField nameInput; //이름 입력한 인풋 필드

    public Button redTankBtn;
    public Button blueTankBtn;

    public Button loginBtn;
    //로그인 하면 데이터 전송하는거
    //잘못된 값을 입력하면 팝업 띄워주기 Dotween 깔쌈하게 맹글어주기
    //로그인 성공시 게임 스테이지 들어가기
    //게임 스테이지 타일맵으로 만들기

    private void Awake()
    {
        tank = TankCategory.Red; //처음은 Red탱크로 설정
        redTankBtn.onClick.AddListener(() =>
        {
            tank = TankCategory.Red;
            foreach(Image img in selectImageHolders)
            {
                img.sprite = selectImages[1]; //전부 x로 변경
            }
            selectImageHolders[(int)TankCategory.Red].sprite = selectImages[0];
        });

        blueTankBtn.onClick.AddListener(() =>
        {
            tank = TankCategory.Blue;
            foreach (Image img in selectImageHolders)
            {
                img.sprite = selectImages[1]; //전부 x로 변경
            }
            selectImageHolders[(int)TankCategory.Blue].sprite = selectImages[0];
        });

        loginBtn.onClick.AddListener(() =>
        {
            string name = nameInput.text;
            
            if(name == "") 
            {
                PopUpManager.OpenPopup(IconCategory.SYSTEM, "이름은 반드시 입력하셔야 합니다.");
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
    }

    public void HandleMsg(string payload)
    {
        Debug.Log(payload);
        TransformVO vo = JsonUtility.FromJson<TransformVO>(payload);

        GameManager.GameStart(vo);
        //GameManager의 StartGame을 호출하자.
    }


}
