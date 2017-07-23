using UnityEngine;
using UnityEngine.SceneManagement;

// a class used to enable and disable all classes derived from Disableable
public class ManageUpdates : MonoBehaviour
{
    public static ManageUpdates Ins { get; private set; }

    void Awake()
    {
        if (!InitGlobalIns()) return;

        SceneManager.sceneLoaded += OnSceneLoaded;
        GameEventSignals.OnAllCannonsDestroyed += OnAllCannonsDestroyed;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameEventSignals.OnAllCannonsDestroyed -= OnAllCannonsDestroyed;
    }

    void OnAllCannonsDestroyed()
    {
        GlobalManager.DisableAllUpdates();
    }

    void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        if (_scene.buildIndex != 0) {
            GlobalManager.EnableAllUpdates();
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
