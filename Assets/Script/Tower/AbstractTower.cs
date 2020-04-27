using UnityEngine;
using UnityEngine.Serialization;

public abstract class AbstractTower : MonoBehaviour
{
    protected Enemy target;
    protected bool isAttack;
    public GameObject towerObject;
    public int level;
    public int price;
    public float attackSpeedDelay;
    public float attackRadius;
    public float attackCounter;
    public Bullet bullet;
}