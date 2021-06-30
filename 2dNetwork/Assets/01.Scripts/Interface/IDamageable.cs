using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{
    /// <summary>
    /// 피격 처리 매서드
    /// </summary>
    /// <param name="damage">피격시 들어갈 데미지</param>
    /// <param name="powerDir">피격시 밀려나갈 방향</param>
    /// <param name="isEnemy">적이 쏜 총알인지 기억하는 변수</param>
    void OnDamage(int damage, Vector2 powerDir, bool isEnemy, int shooterId);
}
