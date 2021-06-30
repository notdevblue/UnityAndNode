using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SendData : MonoBehaviour
{
    public Button sendDataBtn;
    public Button loginBtn;
    public InputField inputText;

    public int score = 200;
    public string token;

    private void Start()
    {
        sendDataBtn.onClick.AddListener(() => 
        {
            StartCoroutine(Login());
        });

        loginBtn.onClick.AddListener(() =>
        {
            StartCoroutine(Send());
        });
    }

    IEnumerator Login()
    {
        WWWForm form = new WWWForm();
        // 실행한 것은 요청하는 객채를 만들어 준 것
        UnityWebRequest www = UnityWebRequest.Post("http://localhost:54000/login", form);

        // 결과 나올때까지 대기함
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning(www.error);
        }
        else
        {
            string result = www.downloadHandler.text;
            ResponseVO vo = JsonUtility.FromJson<ResponseVO>(result);
            token = vo.msg;

            Debug.Log(result);
        }
    }

    IEnumerator Send()
    {
        WWWForm form = new WWWForm();
        form.AddField("score", score);
        form.AddField("data", inputText.text);
        form.AddField("token", token);

        // 실행한 것은 요청하는 객채를 만들어 준 것
        UnityWebRequest www = UnityWebRequest.Post("http://localhost:54000/save_data", form);

        // 결과 나올때까지 대기함
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning(www.error);
        }
        else
        {
            string result = www.downloadHandler.text;

            Debug.Log(result);
        }
    }
}
