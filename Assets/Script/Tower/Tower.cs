using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : AbstractTower, ITowerSkills
{
    /// <inheritdoc />
    public List<Enemy> GetEnemiesInRange()
    {
        List<Enemy> enemies = new List<Enemy>();

        foreach (Enemy enemy in Manager.manager.enemyList)
        {
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) <= attackRadius)
            {
                enemies.Add(enemy);
            }
        }

        return enemies;
    }

    /// <inheritdoc />
    public Enemy GetNearestEnemy()
    {
        Enemy attackedEnemy = null;

        foreach (Enemy enemy in GetEnemiesInRange())
        {
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) < attackRadius)
            {
                attackedEnemy = enemy;
            }
        }
        return attackedEnemy;
    }


    /// <inheritdoc />
    public void Attack()
    {
        isAttack = false;
        Transform newBullet = Instantiate(bullet.transform, towerObject.transform.position,Quaternion.identity );
        newBullet.transform.localPosition = transform.localPosition;

        if (target != null)
        {
            StartCoroutine(MovedBullet(newBullet, target));
        }
        else
        {
            Destroy(newBullet.gameObject);
        }
    }

    public IEnumerator MovedBullet(Transform bullet, Enemy targetEnemy)
    {
        while (GetTargetDistance(targetEnemy) < attackRadius && bullet != null && targetEnemy != null)
        {
            bullet.transform.localPosition = Vector2.MoveTowards(bullet.transform.localPosition,
                targetEnemy.transform.position, 10f * Time.fixedDeltaTime);
            yield return null;
        }

        try
        {
            if (bullet != null || targetEnemy == null)
            {
                Destroy(bullet.gameObject);
            }
        }
        catch (MissingReferenceException) {}
    }

    public float GetTargetDistance(Enemy targetEnemy)
    {
        if (targetEnemy == null)
        {
            targetEnemy = GetNearestEnemy();
            if (targetEnemy == null)
            {
                return 0f;
            }
        }

        return Mathf.Abs(Vector2.Distance(transform.localPosition, targetEnemy.transform.localPosition));
    }

    private void FixedUpdate()
    {
        attackCounter -= Time.fixedDeltaTime;
        var nearestEnemy = GetNearestEnemy();
        
        if (isAttack)
        {
            Attack();
        }
        
        if (target == null || target.isDead)
        {
            //мб создать для этого метод проверки?
            if (nearestEnemy !=null && 
                Vector2.Distance(transform.localPosition, nearestEnemy.transform.localPosition) <= attackRadius)
            {
                target = nearestEnemy;
            }
        }
        else
        {
            if (attackCounter <= 0)
            {
                isAttack = true;

                attackCounter = attackSpeedDelay;
            }
            else
            {
                isAttack = false;
            }

            if (Vector2.Distance(transform.localPosition, target.transform.localPosition) > attackRadius)
            {
                target = null;
            }
        }
    }

    private void Awake()
    {
        level = 1;
        price = 10;
        attackSpeedDelay = 0.5f;
        attackRadius = 15;
        attackCounter = 0;
    }
}
