using UnityEngine;
using UnityEngine.SceneManagement;

// a class used to determine and thus respond to the states related to level ending
public class ManageState : MonoBehaviour
{
    public static ManageState Ins { get; private set; }

    public enum EnumLevelEndState { victory, defeat }

    void Awake()
    {
        if (!InitGlobalIns()) return;

        SceneManager.sceneLoaded += OnSceneLoaded;
        GameEventSignals.OnLastWaveEnd += OnLastWaveEnd;
        GameEventSignals.OnAllCannonsDestroyed += OnAllCannonsDestroyed;
        GameEventSignals.OnLevelEnd += OnLevelEnd;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameEventSignals.OnLastWaveEnd -= OnLastWaveEnd;
        GameEventSignals.OnAllCannonsDestroyed -= OnAllCannonsDestroyed;
        GameEventSignals.OnLevelEnd -= OnLevelEnd;
    }

    void OnLastWaveEnd()
    {
        GameEventSignals.DoLevelEnd(EnumLevelEndState.victory);
    }

    void OnAllCannonsDestroyed()
    {
        GameEventSignals.DoLevelEnd(EnumLevelEndState.defeat);
    }

    void OnLevelEnd(EnumLevelEndState _state)
    {
        Time.timeScale = 0;
    }

    void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        if (_scene.buildIndex != 0) {
            Time.timeScale = 1;
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
