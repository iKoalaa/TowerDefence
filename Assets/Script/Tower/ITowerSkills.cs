using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITowerSkills
{
    /// <summary>
    /// Ищет ближайшего врага для атаки.
    /// </summary>
    /// <returns>Врагов.</returns>
    List<Enemy> GetEnemiesInRange();

    /// <summary>
    /// Выбирает ближайшего врага для атакию
    /// </summary>
    /// <returns>Врага.</returns>
    Enemy GetNearestEnemy();

    /// <summary>
    /// Атакует цель.
    /// </summary>
    void Attack();

    /// <summary>
    /// Двигает снаряд к цели.
    /// </summary>
    /// <param name="bullet">Снаряд.</param>
    /// <param name="targetEnemy">Цель.</param>
    /// <returns></returns>
    IEnumerator MovedBullet(Transform bullet, Enemy targetEnemy);

    /// <summary>
    /// Атакует врагов, находящихся в радиусе.
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackEnemyInRadius();

    /// <summary>
    /// Вычисляет дистанцию до цели.
    /// </summary>
    /// <param name="targetEnemy">Цель.</param>
    /// <returns>Дистанцию.</returns>
    float GetTargetDistance(Enemy targetEnemy);

    
}
