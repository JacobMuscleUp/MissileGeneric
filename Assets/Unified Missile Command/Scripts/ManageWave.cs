using System;
using UnityEngine;

// a class used to add the wave system
public class ManageWave : Disableable
{
    public static ManageWave Ins { get; private set; }

    [Serializable]
    public class EnemyGroupBundle
    {
        [Tooltip("the name of the enemy group")]
        public string name;
        [Tooltip("the number of members in the enemy group")]
        public int count;
        [Tooltip("the prefab that represents the enemy group")]
        public GameObject prefab;
    }

    [Serializable]
    public class WaveBundle
    {
        [Tooltip("the name of the wave")]
        public string name;
        [Tooltip("the duration in seconds of the wave")]
        public int duration;
        [Tooltip("the time in seconds required to spawn each enemy during the wave")]
        public float enemySpawnTime;
        [Tooltip("an array of variable size that stores info about all enemy groups that are present in the wave")]
        public EnemyGroupBundle[] enemyGroupBundles;
    }

    [Tooltip("an array of variable size that stores info about all waves")]
    public WaveBundle[] waveBundles;

    [HideInInspector] public int waveIndex; // the index of the current wave in waveBundles
    [HideInInspector] public int enemyGroupsIndex;
    int enemiesInGroupIndex;
    [HideInInspector] public float enemySpawnTime; // the time required to spawn each enemy
    [HideInInspector] public int waveCountdown; // 
    float waveCountdownTimer;

    void Awake()
    {
        Ins = this;
        //if (!InitGlobalIns()) return;
        GlobalManager.AddToDisabledObjContainer(this);

        GameEventSignals.OnEnemySpawned += OnEnemySpawned;
        GameEventSignals.OnSceneConfigured += OnSceneConfigured;
    }

    void Update()
    {
        if (!disabled.Value) {
            CheckWave();
        }
    }

    void CheckWave()
    {
        if (waveIndex == waveBundles.Length - 1 && waveCountdown <= 0) {
            GameEventSignals.DoLastWaveEnd();
            return;
        }

        waveCountdownTimer += Time.deltaTime;
        if (waveCountdownTimer >= 1.0f) {
            waveCountdownTimer = 0;
            --waveCountdown;
            GameEventSignals.DoWaveCountdownUpdated();
        }

        if (waveIndex < waveBundles.Length - 1 && waveCountdown == 0) {
            ++waveIndex;
            enemyGroupsIndex = enemiesInGroupIndex = 0;
            enemySpawnTime = waveBundles[waveIndex].enemySpawnTime;
            waveCountdown = waveBundles[waveIndex].duration;

            SpawnEnemy.Ins.spawnTimer = enemySpawnTime;
            SpawnEnemy.Ins.stopSpawning = false;

            GameEventSignals.DoWaveUpdated(waveIndex);
        }
    }

    void OnDestroy()
    {
        GameEventSignals.OnEnemySpawned -= OnEnemySpawned;
        GameEventSignals.OnSceneConfigured -= OnSceneConfigured;
    }

    void OnEnemySpawned(EnemyMissile _enemy)
    {
        if (!disabled.Value) {
            EnemyGroupBundle[] currentEnemyGroups = waveBundles[waveIndex].enemyGroupBundles;

            if (enemyGroupsIndex == currentEnemyGroups.Length - 1
                && enemiesInGroupIndex == waveBundles[waveIndex].enemyGroupBundles[enemyGroupsIndex].count - 1) {
                SpawnEnemy.Ins.stopSpawning = true;
                return;
            }

            if (++enemiesInGroupIndex == currentEnemyGroups[enemyGroupsIndex].count) {
                ++enemyGroupsIndex;
                enemiesInGroupIndex = 0;
            }
        }
    }

    void OnSceneConfigured()
    {
        waveIndex = enemyGroupsIndex = enemiesInGroupIndex = 0;
        enemySpawnTime = waveBundles[waveIndex].enemySpawnTime;
        waveCountdown = waveBundles[waveIndex].duration;
        waveCountdownTimer = 0;

        if (ManageSceneSetup.Ins.enemySpawnerExists)
            SpawnEnemy.Ins.spawnTimer = enemySpawnTime;
        else {
            SpawnEnemy.Ins.spawnTimer = SpawnEnemy.Ins.spawnTime;
            disabled.Value = true;
        }

        GameEventSignals.DoWaveUpdated(waveIndex);
        GameEventSignals.DoWaveCountdownUpdated();
    }

    bool InitGlobalIns() {
        if (Ins == null)
            Ins = this;
        else if (Ins != this)
        {
            DestroyImmediate(gameObject);
            return false;
        }
        DontDestroyOnLoad(gameObject);
        return true;
    }
}
