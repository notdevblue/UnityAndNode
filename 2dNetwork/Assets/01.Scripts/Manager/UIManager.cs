using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject infoPrefab;
    public CanvasGroup cvsLogin;

    private void Awake()
    {
        instance = this;
        PoolManager.CreatePool<InfoUI>(infoPrefab, transform, 8); //8개의 HP바와 Text 제작
        //cvsLogin = GetComponent<CanvasGroup>();

    }

    public static void OpenLoginPannel()
    {
        instance.cvsLogin.alpha = 1;
        instance.cvsLogin.interactable = true;
        instance.cvsLogin.blocksRaycasts = true;
    }

    public static InfoUI SetInfoUI(Transform player, string name)
    {
        InfoUI ui = PoolManager.GetItem<InfoUI>();
        ui.SetTarget(player, name);
        return ui;
    }

    public static void CloseLoginPanel()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(
            DOTween.To(
                () => instance.cvsLogin.alpha, 
                value => instance.cvsLogin.alpha = value, 
                0f, 
                1f));
        seq.AppendCallback(()=>{
            instance.cvsLogin.interactable = false;
            instance.cvsLogin.blocksRaycasts = false;
        });
    }
}
