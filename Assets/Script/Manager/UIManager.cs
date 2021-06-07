using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject infoPrefab;
    private CanvasGroup cvsLogin;

    private void Awake()
    {
        instance = this;
        PoolManager.CratePool<InfoUI>(infoPrefab, transform, 8); // 8개의 HP바와 Text 제작
        cvsLogin = GetComponent<CanvasGroup>();
        cvsLogin.alpha = 1;
        cvsLogin.interactable = true;
        cvsLogin.blocksRaycasts = true;

        
    }

    public static void SetInfoUI(Transform player, string name)
    {
        InfoUI ui = PoolManager.GetItem<InfoUI>();
        ui.SetTarget(player, name);
    }

    public static void CloseLoginPanel()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(DOTween.To(
            () => instance.cvsLogin.alpha,
            value => instance.cvsLogin.alpha = value, 
            0.0f, 1.0f));

        seq.AppendCallback(() =>
        {
            instance.cvsLogin.interactable   = false;
            instance.cvsLogin.blocksRaycasts = false;
        });
    }
}
