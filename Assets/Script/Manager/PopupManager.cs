using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum IconCategory
{
    ERR = 0,
    SYSTEM = 1
}

public class PopupManager : MonoBehaviour
{
    private static PopupManager instance = null;

    public Sprite[] icons;

    [Header("�˾� ���� ���� ���ӿ�����Ʈ")]
    public GameObject popupPanel;
    public Transform popupTrm; // ���� �˾� Ʈ������
    public Image popupIcon;
    public Text popupText;

    public Button btnClose;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("�ټ��� PopupManager�� ����ǰ� �ֽ��ϴ�.");
        }
        instance = this;

        popupTrm.localScale = new Vector2(0.0f, 0.0f);

        btnClose.onClick.AddListener(() =>
        {
            //instance.popupTrm.DOScale(new Vector3(0.0f, 0.0f, 0.0f), 0.8f);
            popupTrm.localScale = new Vector2(0.0f, 0.0f);
            popupPanel.SetActive(false);
        });


    }

    public static void OpenPopup(IconCategory icon, string text)
    {
        instance.popupPanel.SetActive(true);
        instance.popupIcon.sprite   = instance.icons[(int)icon];
        instance.popupText.text     = text;

        //instance.popupTrm.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 1.0f);
        instance.popupTrm.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 0.8f).SetEase(Ease.OutExpo);


    }


    void Start()
    {
        popupPanel.SetActive(false);

    }

}
