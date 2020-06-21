using UnityEngine;

public abstract class AbstractEnemy : MonoBehaviour
{
    protected Transform enemyTransform;
    protected Collider2D collider2D;
    
    protected float _navigationTime;
    protected bool _isDead;
    
    public int target;
    public Transform finish;
    public Transform[] turnPoints;
    public float navigation;
    
    public int health;
    public int damageOnFort;
    public int dropGold;
    
}