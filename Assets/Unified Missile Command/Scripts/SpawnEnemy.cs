using UnityEngine;
using UnityEngine.SceneManagement;

// a class used to spawn enemies
public class SpawnEnemy : Disableable
{
    public static SpawnEnemy Ins { get; private set; }

    public GameObject prefabDefaultEnemy;

    [Tooltip("determine if the path of each enemy missile is vertical")]
    public bool verticalEnemyPath;
    [Tooltip("The time required for spawning an enemy")]
    public float spawnTime;

    [HideInInspector] public bool stopSpawning;
    [HideInInspector] public float spawnTimer;

    delegate void SpawnEnemyHandler();
    SpawnEnemyHandler spawnEnemyDel = new SpawnEnemyHandler(SpawnEnemy_0);

    void Awake()
    {
        Ins = this;
        //if (!InitGlobalIns()) return;
        GlobalManager.AddToDisabledObjContainer(this);

        SceneManager.sceneLoaded += OnSceneLoaded;
        InspectorEventSignals.OnSpawnEnemyInspectorUpdated += OnSpawnEnemyInspectorUpdated;
        GameEventSignals.OnSceneConfigured += OnSceneConfigured;
    }

    void Update()
    {
        if (!disabled.Value && !stopSpawning) {
            spawnEnemyDel();
        }
    }

    static void SpawnEnemy_0() {}

    void SpawnEnemy_1()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= ManageWave.Ins.enemySpawnTime) {
            spawnTimer = 0;
            
            var cannons = GlobalManager.cannons;
            var waveManager = ManageWave.Ins;

            Cannon randCannon = cannons[Random.Range(0, cannons.Count)];
            EnemyMissile enemy = Instantiate(
                waveManager.waveBundles[waveManager.waveIndex].enemyGroupBundles[waveManager.enemyGroupsIndex].prefab).GetComponent<EnemyMissile>();
            enemy.transform.position = new Vector3(
                randCannon.transform.position.x
                , ManageSceneSetup.Ins.spawnVerticalRange.y
                , ManageSceneSetup.Ins.playField.transform.position.z);
            enemy.transform.LookAt(randCannon.transform.position);
            enemy.SetDest(randCannon.transform.position);

            GameEventSignals.DoEnemySpawned(enemy);
        }
    }

    void SpawnEnemy_2()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= ManageWave.Ins.enemySpawnTime) {
            spawnTimer = 0;

            var cannons = GlobalManager.cannons;
            var waveManager = ManageWave.Ins;
            
            Cannon randCannon = cannons[Random.Range(0, cannons.Count)];
            EnemyMissile enemy = Instantiate(
                waveManager.waveBundles[waveManager.waveIndex].enemyGroupBundles[waveManager.enemyGroupsIndex].prefab).GetComponent<EnemyMissile>();
            enemy.transform.position = new Vector3(
				Random.Range(ManageSceneSetup.Ins.spawnHorizontalRange.x, ManageSceneSetup.Ins.spawnHorizontalRange.y)
                , ManageSceneSetup.Ins.spawnVerticalRange.y
                , ManageSceneSetup.Ins.playField.transform.position.z);
            enemy.transform.LookAt(randCannon.transform.position);
            enemy.SetDest(randCannon.transform.position);

            GameEventSignals.DoEnemySpawned(enemy);
        }
    }

    void SpawnEnemy_3()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnTime) {
            spawnTimer = 0;
            
            var cannons = GlobalManager.cannons;

            Cannon randCannon = cannons[Random.Range(0, cannons.Count)];
            EnemyMissile enemy = Instantiate(prefabDefaultEnemy).GetComponent<EnemyMissile>();
            enemy.transform.position = new Vector3(
                randCannon.transform.position.x
                , ManageSceneSetup.Ins.spawnVerticalRange.y
                , ManageSceneSetup.Ins.playField.transform.position.z);
            enemy.transform.LookAt(randCannon.transform.position);
            enemy.SetDest(randCannon.transform.position);

            GameEventSignals.DoEnemySpawned(enemy);
        }
    }

    void SpawnEnemy_4()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnTime) {
            spawnTimer = 0;

            var cannons = GlobalManager.cannons;

            Cannon randCannon = cannons[Random.Range(0, cannons.Count)];
            EnemyMissile enemy = Instantiate(prefabDefaultEnemy).GetComponent<EnemyMissile>();
            enemy.transform.position = new Vector3(
                Random.Range(ManageSceneSetup.Ins.spawnHorizontalRange.x, ManageSceneSetup.Ins.spawnHorizontalRange.y)
                , ManageSceneSetup.Ins.spawnVerticalRange.y
                , ManageSceneSetup.Ins.playField.transform.position.z);
            enemy.transform.LookAt(randCannon.transform.position);
            enemy.SetDest(randCannon.transform.position);

            GameEventSignals.DoEnemySpawned(enemy);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        InspectorEventSignals.OnSpawnEnemyInspectorUpdated -= OnSpawnEnemyInspectorUpdated;
        GameEventSignals.OnSceneConfigured -= OnSceneConfigured;
    }

    void OnSpawnEnemyInspectorUpdated()
    {
        if (verticalEnemyPath && ManageSceneSetup.Ins.waveManagerExists)
            spawnEnemyDel = SpawnEnemy_1;
        else if (verticalEnemyPath)
            spawnEnemyDel = SpawnEnemy_3;
        else if (ManageSceneSetup.Ins.waveManagerExists)
            spawnEnemyDel = SpawnEnemy_2;
        else
            spawnEnemyDel = SpawnEnemy_4;
    }

    void OnSceneConfigured()
    {
        OnSpawnEnemyInspectorUpdated();
    }

    void OnDrawGizmosSelected()
	{
		var playField = GameObject.FindWithTag("PlayField");
		var spawnVerticalLevel = playField.transform.position.y + playField.GetComponent<MeshRenderer>().bounds.extents.y;
		var spawnHorizontalRange = new Vector2(
			playField.transform.position.x - playField.GetComponent<MeshRenderer>().bounds.extents.x
			, playField.transform.position.x + playField.GetComponent<MeshRenderer>().bounds.extents.x
		);

		Gizmos.color = Color.red;
		Gizmos.DrawLine(new Vector3(spawnHorizontalRange.x, spawnVerticalLevel, 0), new Vector3(spawnHorizontalRange.y, spawnVerticalLevel, 0));
	}

    void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        if (_scene.buildIndex != 0) {
            stopSpawning = false;
            EnemyMissile.isFirstInstance = true;
        }
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
