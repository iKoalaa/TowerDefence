public interface IEnemySkills
{
    /// <summary>
    /// Наносит урон по игроку.
    /// </summary>
    void Attack();

    /// <summary>
    /// Убивает врага. 
    /// </summary>
    void Die();

    /// <summary>
    /// Наносит урон по врагу.
    /// </summary>
    /// <param name="bullet">Снаряд.</param>
    void HitOnEnemy(Bullet bullet);
}