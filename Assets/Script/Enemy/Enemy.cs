﻿using System.Collections;
using UnityEngine;

public class Enemy : AbstractEnemy, IEnemySkills
{
    private const int SPEED = 10;

    public bool isDead
    {
        get => _isDead;
        set => _isDead = value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TurnPoint"))
        {
            target += 1;
        }

        if (collision.CompareTag("Finish"))
        {
            if (!isDead)
            {
                Attack();
                Manager.manager.UnregistredEnemy(this);
            }
        }

        if (collision.CompareTag("Bullet"))
        {
            var newBullet = collision.gameObject.GetComponent<Bullet>();
            HitOnEnemy(newBullet);
            Destroy(collision.gameObject);
        }
    }

    /// <inheritdoc />
    public void HitOnEnemy(Bullet bullet)
    {
        if (health > bullet.damage)
        {
            health -= bullet.damage;
        }
        else
        {
            Die();
        }
    }

    /// <inheritdoc />
    public void Die()
    {
        isDead = true;
        collider2D.enabled = false;
        Manager.manager.UnregistredEnemy(this);
        Manager.manager.killedEnemyOnWave++;
        Manager.manager.killedEnemyCount++;
        Manager.manager.gold += dropGold;
    }

    /// <inheritdoc />
    public void Attack()
    {
        Manager.manager.playerHealth -= damageOnFort;
    }

    private void Start()
    {
        enemyTransform = GetComponent<Transform>();
        collider2D = GetComponent<Collider2D>();
        Manager.manager.RegistredEnemy(this);
        StartCoroutine(CheckAndMoveEnemy());
    }

    IEnumerator CheckAndMoveEnemy()
    {
        while (true)
        {
            if (turnPoints != null && !isDead)
            {
                _navigationTime += SPEED * Time.fixedDeltaTime;
                if (_navigationTime > navigation)
                {
                    if (target < turnPoints.Length)
                    {
                        enemyTransform.position = Vector2.MoveTowards(enemyTransform.position,
                            turnPoints[target].position, _navigationTime);
                    }
                    else
                    {
                        enemyTransform.position =
                            Vector2.MoveTowards(enemyTransform.position, finish.position, _navigationTime);
                    }
                    _navigationTime = 0;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
    