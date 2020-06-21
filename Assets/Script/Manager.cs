using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    private const float DELAY = .5f;
    
    public static Manager manager;

    #region Private

    private int _killedEnemyCount;
    private int _spawnedSpawnedEnemyCount;
    private int _gold;
    private int _waveNumber;
    private int _playerHealth;
    private int _killedEnemyOnWave;
    private GameMode _mode;
    
    #endregion

    #region Public
    
    [NonSerialized]
    public List<Enemy> enemyList = new List<Enemy>();

    public Text currentWaveLabel; 
    public Text moneyLabel;
    public Text healthLabel;
    public Text playLabel;
    public Text totalKilledLabel;
    public Button playButton;
    public Tower tower;

    public GameObject spawn;
    public Enemy enemyObject;
    public int totalEnemiesOnWave;
    public int enemiesPerSpawn;

    #endregion
    
    #region Property

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

    public int spawnedEnemyCount
    {
        get => _spawnedSpawnedEnemyCount;
        set => _spawnedSpawnedEnemyCount = value;
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

    #endregion
    
    IEnumerator NewWave()
    {
        var spawnPosition = spawn.transform.position;
        var enemy = enemyObject;
        
        while (spawnedEnemyCount < totalEnemiesOnWave)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                spawnedEnemyCount++;
                Enemy newEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity);
                newEnemy.transform.position = spawnPosition;
            }

            yield return new WaitForSeconds(DELAY);
        }
    }
    
    IEnumerator DrawStats()
    {
        while (true)
        {
            if (playerHealth <= 0)
            {
                playerHealth = 0;
            }
            healthLabel.text = "Health: " + playerHealth;
            moneyLabel.text = "Money: " + gold;
            currentWaveLabel.text = "Wave: " + waveNumber;
            yield return null;
        }
    }

    //подписка и отписка на добавление врагов в список.

    public void RegistredEnemy(Enemy enemy)
    {
        enemyList.Add(enemy);
    }

    public void UnregistredEnemy(Enemy enemy)
    {
        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    public void PlayButtonPressed()
    {
        var enemy = enemyObject;
        //TODO: Не запускает следующую волну, разберись сегодня!
        switch (_mode)
        {
            case GameMode.next:
                waveNumber += 1;
                totalEnemiesOnWave *= 2;
                enemy.damageOnFort += 1;
                enemy.health += waveNumber;
                enemy.dropGold += 2;
                killedEnemyOnWave = 0;
                spawnedEnemyCount = 0;
                StartCoroutine(NewWave());
                break;
            
            case GameMode.gameOver:
                totalKilledLabel.gameObject.SetActive(true);
                totalKilledLabel.text = "Total Killed: " + killedEnemyCount;
                _mode = GameMode.newGame;
                PlayButtonPressed();
                break;
            
            case GameMode.play:
                StartCoroutine(NewWave());
                break;
            
            case GameMode.newGame:
                InitializeStats();
                PlayButtonPressed();
                break;
        }
        playButton.gameObject.SetActive(false);
    }

    private void Menu()
    {
        switch (_mode)
        {
            case GameMode.gameOver:
                playLabel.text = "Play again.";
                totalKilledLabel.text = "Total Killed:" + killedEnemyCount;
                totalKilledLabel.gameObject.SetActive(true);
                break;
            
            case GameMode.next:
                playLabel.text = "Next wave";
                break;
            
            case GameMode.play:
                playLabel.text = "Play";
                break;
            
            case GameMode.newGame:
                playLabel.text = "New Game";
                break;
        }
        
        playButton.gameObject.SetActive(true);
    }

    IEnumerator WaveOver()
    {
        while (true)
        {
            if (spawnedEnemyCount == totalEnemiesOnWave && enemyList.Count == 0)
            {
                SetMode();
                Menu();
            }

            yield return null;
        }
    }

    private void SetMode()
    {
        if (playerHealth <= 0)
        {
            StopCoroutine(NewWave());
            _mode = GameMode.gameOver;
            return;
        }

        if (spawnedEnemyCount == totalEnemiesOnWave && killedEnemyOnWave == totalEnemiesOnWave)
        {
            _mode = GameMode.next;
        }
    }

    private void Start()
    {
        StartCoroutine(DrawStats());
        StartCoroutine(WaveOver());
        playButton.gameObject.SetActive(false);
        Menu();
    }

    private void InitializeStats()
    {
        totalKilledLabel.gameObject.SetActive(false);
        _mode = GameMode.play;
        spawnedEnemyCount = 0;
        playerHealth = 10;
        gold = 50;
        killedEnemyCount = 0;
        totalEnemiesOnWave = 10;
        waveNumber = 1;
        enemyObject.damageOnFort = 1;
        enemyObject.health = 2;
        enemyObject.dropGold = 4;
        killedEnemyOnWave = 0;
        tower.InitializeTowerStats();
    }

    void Awake()
    {
        InitializeStats();

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
