using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    private const float DELAY = .5f;
    
    public static Manager manager;
    
    private int _killedEnemyCount;
    private int _enemyCountOnScreen;
    private int _gold;
    private int _waveNumber;
    private int _playerHealth;
    private int _killedEnemyOnWave;
    private GameMode _mode;

    public Text currentWaveLabel; 
    public Text moneyLabel;
    public Text healthLabel;
    public Text playLabel;
    public Text totalKilledLabel;
    public Button playButton;

    public GameObject spawn;
    public Enemy enemyObject;
    public int totalEnemiesOnWave;
    public int enemiesPerSpawn;
    
    public List<Enemy> enemyList = new List<Enemy>();

    public int killedEnemyCount 
    { 
        get => _killedEnemyCount;
        set => _killedEnemyCount = value;
    }

    public int killedEnemyOnWave
    {
        get => _killedEnemyOnWave;
        set => _killedEnemyOnWave = value;
    }

    public int enemyCountOnScreen
    {
        get => _enemyCountOnScreen;
        set => _enemyCountOnScreen = value;
    }


    public int waveNumber
    {
        get => _waveNumber;
        set => _waveNumber = value;
    }

    public int playerHealth
    {
        get => _playerHealth;
        set => _playerHealth = value;
    }

    public int gold
    {
        get => _gold;
        set => _gold = value;
    }

    IEnumerator NewWave()
    {
        var spawnPosition = spawn.transform.position;
        
        if (enemiesPerSpawn > 0 && enemyCountOnScreen < totalEnemiesOnWave)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                Enemy newEnemy = Instantiate(enemyObject, spawnPosition, Quaternion.identity);
                newEnemy.transform.position = spawnPosition;
                enemyCountOnScreen++;
            }

            yield return new WaitForSeconds(DELAY);
            StartCoroutine(NewWave());
        }
    }


    //подписка на добавление врагов в список(ниже отписка)

    public void RegistredEnemy(Enemy enemy)
    {
        enemyList.Add(enemy);
    }

    public void UnregistredEnemy(Enemy enemy)
    {
        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    public IEnumerator DrawStats()
    {
        healthLabel.text = "Health: " + playerHealth;
        moneyLabel.text = "Money: " + gold;
        currentWaveLabel.text = "Wave: " + waveNumber;

        yield return null;
        StartCoroutine(DrawStats());
    }

    public void Menu()
    {
        switch (_mode)
        {
            case GameMode.gameOver:
                playLabel.text = "Play again.";
                break;
            case GameMode.next:
                playLabel.text = "Next wave";
                break;
            case GameMode.play:
                playLabel.text = "Play";
                break;
        }
        
        playButton.gameObject.SetActive(true);
    }

    public void WaveOver()
    {
        if (enemyCountOnScreen == totalEnemiesOnWave)
        {
            SetMode();
            Menu();
        }
    }

    public void PlayButtonPressed()
    {
        switch (_mode)
        {
            case GameMode.next:
                StartCoroutine(NewWave());
                waveNumber += 1;
                totalEnemiesOnWave *= waveNumber;
                enemyObject.damageOnFort += 1;
                enemyObject.health += 2;
                enemyObject.dropGold += 5;
                killedEnemyOnWave = 0;
                enemyCountOnScreen = 0;
                break;
            
            case GameMode.gameOver:
                totalKilledLabel.gameObject.SetActive(true);
                totalKilledLabel.text = "Total Killed: " + killedEnemyCount;
                break;
            
            default: 
                totalEnemiesOnWave = 10;
                waveNumber = 1;
                enemyObject.damageOnFort = 1;
                enemyObject.health = 2;
                enemyObject.dropGold = 10;
                killedEnemyCount = 0;
                killedEnemyOnWave = 0;
                enemyCountOnScreen = 0;
                break;
        }

        StartCoroutine(NewWave());
        playButton.gameObject.SetActive(false);
    }

    private void SetMode()
    {
        if (enemyCountOnScreen == totalEnemiesOnWave && playerHealth <= 0)
        {
            _mode = GameMode.gameOver;
        }
        else
        {
            if (enemyCountOnScreen == totalEnemiesOnWave && playerHealth > 0)
            {
                _mode = GameMode.next;
            }
            else if (waveNumber > 1 && playerHealth > 0)
            {
                _mode = GameMode.next;
            }
        }
    }

    private void Start()
    {
        StartCoroutine(DrawStats());
        playButton.gameObject.SetActive(false);
        Menu();
    }

    void Awake()
    {
        _mode = GameMode.play;
        waveNumber = 0;
        enemyCountOnScreen = 0;
        playerHealth = 20;
        gold = 50;
        waveNumber = 1;

        if (manager == null)
        {
            manager = this;
        }
        else
        {
            if (manager != this)
                Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
