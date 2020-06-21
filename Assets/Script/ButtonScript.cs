using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    [SerializeField]
    private Text _textLabel;
    
    private bool _canUpgrade;

    public Tower tower;
    public Button button;

    public Text textLabel => _textLabel;
    
    public void Upgrade()
    {
        var totalGold = Manager.manager.gold;
        var price = tower.price;
        var level = tower.level;
        var attackDelay = tower.attackSpeedDelay;
        var damage = tower.bullet.damage;

        if (totalGold > price && totalGold - price >= 0)
        {
            level += 1;
            attackDelay *= .8f;
            damage += tower.level;
            totalGold -= price;
            price *= tower.level;
        }
        else
        {
            Debug.Log("Недостаточно золота для апгрейда, необходимо ещё " + (tower.price - totalGold) + ".");
        }

        tower.price = price;
        tower.level = level;
        tower.attackSpeedDelay = attackDelay;
        tower.bullet.damage = damage;
        Manager.manager.gold = totalGold;
    }
    
    private void Update()
    {
        textLabel.text = "lvlUp " + tower.price;
    }

    private void Awake()
    {
        textLabel.text = "lvlUp " + tower.price;
    }
}