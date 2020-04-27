using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int _damage;

    public int damage
    {
        get => _damage;
        set => _damage = value;
    }

    private void Awake()
    {
        _damage = 1;
    }
}
