using UnityEngine;
using UnityEngine.SceneManagement;

// a class used to initialize global variables used by other classes upon loading a scene
public class ManageSceneSetup : MonoBehaviour
{
    public static ManageSceneSetup Ins { get; private set; }
    [HideInInspector] public bool waveManagerExists;
    [HideInInspector] public bool cannonManagerExists;
    [HideInInspector] public bool enemySpawnerExists;

    [HideInInspector] public GameObject playField;

    [HideInInspector] public Vector2 spawnVerticalRange;
    [HideInInspector] public Vector2 spawnHorizontalRange;

    [HideInInspector] public float playFieldLength;
    [HideInInspector] public float playFieldWidth;
    public static Transform tempObjTransform = null;
    public static Transform cannonParentTransform = null;

    void Awake()
    {
        if (!InitGlobalIns()) return;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        if (_scene.buildIndex != 0) {
            waveManagerExists = (GameObject.FindWithTag("WaveManager") != null);
            cannonManagerExists = (GameObject.FindWithTag("CannonManager") != null);
            enemySpawnerExists = (GameObject.FindWithTag("EnemySpawner") != null);

            playField = GameObject.FindWithTag("PlayField");
            spawnVerticalRange = new Vector2(
                playField.transform.position.y - playField.GetComponent<MeshRenderer>().bounds.extents.y
                , playField.transform.position.y + playField.GetComponent<MeshRenderer>().bounds.extents.y
            );
            spawnHorizontalRange = new Vector2(
                playField.transform.position.x - playField.GetComponent<MeshRenderer>().bounds.extents.x
                , playField.transform.position.x + playField.GetComponent<MeshRenderer>().bounds.extents.x
            );

            playFieldLength = spawnVerticalRange.y - spawnVerticalRange.x;
            playFieldWidth = spawnHorizontalRange.y - spawnHorizontalRange.x;
            
            InitTransform(ref tempObjTransform, "Temporary Objects");
            InitTransform(ref cannonParentTransform, "Cannons Temp");
            
            if (cannonManagerExists) {
                for (int i = GlobalManager.cannons.Count - 1; i >= 0; --i) {
                    Destroy(GlobalManager.cannons[i].gameObject);
                    GlobalManager.cannons.RemoveAt(i);
                }
            }

            GameEventSignals.DoSceneConfigured();
        }
    }

    void InitTransform(ref Transform _transform, string _name)
    {
        GameObject go = new GameObject();
        go.name = _name;
        _transform = go.transform;
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
