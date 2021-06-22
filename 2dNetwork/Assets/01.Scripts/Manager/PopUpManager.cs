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

public class PopUpManager : MonoBehaviour
{
    private static PopUpManager instance = null;

    public Sprite[] icons;

    [Header("팝업셋팅관련 게임오브젝트")]
    public GameObject popupPanel;
    public Transform popupTrm; //내부 팝업의 트랜스폼
    public Image popupIcon;
    public Text popuptext;

    public Button btnClose;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("다수의 PopupManager가 실행되고 있습니다.");
        }
        instance = this;
    }

    public static void OpenPopup(IconCategory icon, string text)
    {
        instance.popupPanel.SetActive(true);
        instance.popupIcon.sprite = instance.icons[(int)icon];
        instance.popuptext.text = text;

        instance.popupTrm.localScale = Vector3.zero;

        Sequence seq = DOTween.Sequence();

        
        seq.Append(instance.popupTrm.DOScale(
            new Vector3(1.2f, 1.2f, 1.2f), 0.4f)
            .SetEase(Ease.InOutExpo));
        seq.Append(instance.popupTrm.DOScale(new Vector3(1f, 1f, 1f), 0.2f));

    }

    void Start()
    {
        popupPanel.SetActive(false);
        btnClose.onClick.AddListener(() =>
        {
            popupPanel.SetActive(false);
        });
    }    
}
