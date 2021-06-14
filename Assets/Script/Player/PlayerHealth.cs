using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int maxHP;
    public int currentHP;
    public bool isEnemy = false;
    public InfoUI ui;

    public void SetHealthScript(TankDataVO data, bool isEnemy, InfoUI ui)
    {
        maxHP = data.maxHP;
        currentHP = maxHP;
        this.isEnemy = isEnemy;
        this.ui = ui;
    }

    public void OnDamage(int damage, Vector2 powerDir, bool isEnemy)
    {
        // �ݵ��� ����

        currentHP -= damage;
        UpdateUI();
        if(currentHP <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// ü�� ������ ������Ʈ ���ִ� �ż���
    /// </summary>
    private void UpdateUI()
    {

    }

    public void Die()
    {
        gameObject.SetActive(false);
    }
}
