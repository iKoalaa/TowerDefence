using System;
using System.Collections;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    [SerializeField]
    private Text _textLabel;
    
    private bool _canUpgrade;

    public Tower tower;
    public Button button;

    public Text textLabel
    {
        get => _textLabel;
        set => _textLabel = value;
    }
    
    public void Upgrade()
    {
        var totalGold = Manager.manager.gold;
        if (totalGold > tower.price && totalGold - tower.price >= 0)
        {
            tower.level += 1;
            tower.attackSpeedDelay *= .8f;
            tower.bullet.damage += tower.level;
            totalGold -= tower.price;
            tower.price *= tower.level;
        }

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