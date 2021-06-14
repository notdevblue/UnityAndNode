using UnityEngine;

public interface IDamageable
{
    /// <summary>
    /// �ǰ� ó�� �ż���
    /// </summary>
    /// <param name="damage">�ǰݽ� �� ������</param>
    /// <param name="powerDir">�ǰݽ� �з����� ����</param>
    /// <param name="isEnemy">���� �� �Ѿ����� ����ϴ� ����</param>
    void OnDamage(int damage, Vector2 powerDir, bool isEnemy);
}