using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InfoUI : MonoBehaviour
{
    public Transform playerTrm;
    public float followSpeed = 8f;
    public Text txtName;
    public Transform hpBar;

    private CanvasGroup cg;
    private bool on = true;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
    }

    public void SetVisible(bool on)
    {
        this.on = on;
        cg.alpha = on ? 1.0f : 0.0f;
    }

    public void SetTarget(Transform playerTrm, string name)
    {
        this.playerTrm = playerTrm;
        txtName.text = name;
        gameObject.SetActive(true);
    }

    public void UpdateHPBar(float ratio)
    {
        Debug.Log(ratio);
        ratio = Mathf.Clamp(ratio, 0, 1); // 안전하게
        hpBar.DOScaleX(ratio, 0.3f);
    }

    private void LateUpdate()
    {
        if(!on) { return; }

        Vector3 pos = Camera.main.WorldToScreenPoint(playerTrm.position);
        Vector3 nextPos = Vector3.Lerp(transform.position, pos, Time.deltaTime * followSpeed);
        transform.position = nextPos;
    }
}
