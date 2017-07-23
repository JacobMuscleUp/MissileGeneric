using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

// a class used to draw horizontal lines that indicate the current levels at which enemy missiles switch their directions
public class DrawSwitchDirLevels : Disableable
{
    public static DrawSwitchDirLevels Ins { get; private set; }

    Dictionary<GameObject, List<GameObject>> switchDirLevelsMap = new Dictionary<GameObject, List<GameObject>>();
    Material switchDirLevelMat = null;
    Queue<EnemyMissile> enemies = new Queue<EnemyMissile>();
    bool lockOn = false;

    void Awake()
    {
        Ins = this;
        //if (!InitGlobalIns()) return;

        SceneManager.sceneLoaded += OnSceneLoaded;
        InspectorEventSignals.OnEnemySwitchDirCountUpdated += OnEnemySwitchDirCountUpdated;
        InspectorEventSignals.OnSpawnEnemyInspectorUpdated += OnSpawnEnemyInspectorUpdated;
        GameEventSignals.OnEnemySpawned += OnEnemySpawned;
        GameEventSignals.OnSceneConfigured += OnSceneConfigured;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        InspectorEventSignals.OnEnemySwitchDirCountUpdated -= OnEnemySwitchDirCountUpdated;
        InspectorEventSignals.OnSpawnEnemyInspectorUpdated -= OnSpawnEnemyInspectorUpdated;
        GameEventSignals.OnEnemySpawned -= OnEnemySpawned;
        GameEventSignals.OnSceneConfigured -= OnSceneConfigured;
    }

    void OnEnemySwitchDirCountUpdated(int _switchDirCount, GameObject _enemy)
    {
        foreach (var elem in switchDirLevelsMap) {
            if (elem.Key == _enemy) {
                ClearSwitchDirLevels(elem);
                CreateSwitchDirLevels(_switchDirCount, elem);

                lockOn = true;
                for (int i = 0; i < enemies.Count; ++i)
                    GameEventSignals.DoMissileExplode(enemies.Dequeue(), null);
                lockOn = false;
            }
        }
    }
    
    void OnSpawnEnemyInspectorUpdated()
    {
        foreach (var elem in switchDirLevelsMap) {
            if (SpawnEnemy.Ins.verticalEnemyPath)
                ClearSwitchDirLevels(elem);
            else
                OnEnemySwitchDirCountUpdated(elem.Key.GetComponent<EnemyMissile>().switchDirCount, elem.Key);
        }
    }

    void OnEnemySpawned(EnemyMissile _enemy)
    {
        if (!lockOn)
            enemies.Enqueue(_enemy);
    }

    void OnSceneConfigured()
    {
        InitSwitchDirLevels();
        foreach (var elem in switchDirLevelsMap) 
            if (!SpawnEnemy.Ins.verticalEnemyPath)
                CreateSwitchDirLevels(elem.Key.GetComponent<EnemyMissile>().switchDirCount, elem);
    }

    void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        if (_scene.buildIndex != 0) {
            
        }
    }

	void InitSwitchDirLevels()
	{
		if (ManageSceneSetup.Ins.waveManagerExists) {
			for (int i = ManageWave.Ins.waveBundles.Length - 1; i >= 0; --i) {
				var enemyGroups = ManageWave.Ins.waveBundles[i].enemyGroupBundles;
				foreach (var elem in enemyGroups)
					if (!switchDirLevelsMap.ContainsKey(elem.prefab))
						switchDirLevelsMap[elem.prefab] = new List<GameObject>();
			}
		}
		else
			switchDirLevelsMap[SpawnEnemy.Ins.prefabDefaultEnemy] = new List<GameObject>();
	}

    void ClearSwitchDirLevels(KeyValuePair<GameObject, List<GameObject>> _pair)
    {
        for (int i = _pair.Value.Count - 1; i >= 0; --i) {
            Destroy(_pair.Value[i]);
            switchDirLevelsMap[_pair.Key].RemoveAt(i);
        }
    }

    void CreateSwitchDirLevels(int _switchDirCount, KeyValuePair<GameObject, List<GameObject>> _pair)
    {
        float intervalCount = _switchDirCount + 1;
        for (float i = 1; i < intervalCount; ++i) {
            var switchDirLevel = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var playField = ManageSceneSetup.Ins.playField;
            var playFieldScale = playField.transform.localScale;
            var cannonLevel = ManageSceneSetup.Ins.spawnVerticalRange[0];

            switchDirLevel.transform.SetParent(ManageSceneSetup.tempObjTransform);
            switchDirLevel.name = "Switch Dir Level";
            switchDirLevel.transform.localScale = new Vector3(playFieldScale.x, 0.25f, playFieldScale.z);
            switchDirLevel.transform.position = new Vector3(
                playField.transform.position.x, cannonLevel + i / intervalCount * ManageSceneSetup.Ins.playFieldLength, playField.transform.position.z);
            switchDirLevel.GetComponent<MeshRenderer>().material =
                ((switchDirLevelMat != null) ? switchDirLevelMat : _pair.Key.GetComponentInChildren<MeshRenderer>().sharedMaterial);
            switchDirLevelsMap[_pair.Key].Add(switchDirLevel);
        }
    }

    bool InitGlobalIns()
    {
        if (Ins == null)
            Ins = this;
        else if (Ins != this) {
            DestroyImmediate(gameObject);
            return false;
        }
        DontDestroyOnLoad(gameObject);
        return true;
    }
}
