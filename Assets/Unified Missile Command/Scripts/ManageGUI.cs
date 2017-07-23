using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// a class used to manage all GUI events occurring in the game
public class ManageGUI : MonoBehaviour
{
    public static ManageGUI Ins { get; private set; }

    public Text waveInfoText;
    public Text waveCountdownText;
	public Text resultText;

    public Button levelEndTrigger;
    public Button levelStartTrigger;

	[Tooltip("the message shown upon winning the level")]
	public string playerWinMsg = "you win";
	[Tooltip("the message shown upon losing the level")]
	public string playerLoseMsg = "you lose";
	[Tooltip("the color of the message shown upon winning the level")]
	public Color playerWinMsgColor = Color.green;
	[Tooltip("the color of the message shown upon losing the level")]
	public Color playerLoseMsgColor = Color.red;

    int sceneIndex;

    void Awake()
    {
        Ins = this;
        //if (!InitGlobalIns()) return;

        SceneManager.sceneLoaded += OnSceneLoaded;
        GameEventSignals.OnWaveUpdated += OnWaveUpdated;
        GameEventSignals.OnWaveCountdownUpdated += OnWaveCountdownUpdated;
        GameEventSignals.OnLevelEnd += OnLevelEnd;
        GameEventSignals.OnSceneConfigured += OnSceneConfigured;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameEventSignals.OnWaveUpdated -= OnWaveUpdated;
        GameEventSignals.OnWaveCountdownUpdated -= OnWaveCountdownUpdated;
        GameEventSignals.OnLevelEnd -= OnLevelEnd;
        GameEventSignals.OnSceneConfigured -= OnSceneConfigured;
    }

    void OnWaveUpdated(int _waveIndex)
    {
        if (ManageSceneSetup.Ins.waveManagerExists) {
            waveInfoText.text = string.Format("Wave {0} / {1}"
                , _waveIndex + 1, ManageWave.Ins.waveBundles.Length);
        }
        else
            waveInfoText.text = "no wave system";
    }

    void OnWaveCountdownUpdated()
    {
        if (ManageSceneSetup.Ins.waveManagerExists)
            waveCountdownText.text = string.Format("{0}", ManageWave.Ins.waveCountdown);
        else
            waveCountdownText.text = string.Empty;
    }

    void OnLevelEnd(ManageState.EnumLevelEndState _state)
    {
        levelEndTrigger.onClick.Invoke();
		if (_state == ManageState.EnumLevelEndState.victory) {
			resultText.text = playerWinMsg;
			resultText.color = playerWinMsgColor;
		} 
		else {
			resultText.text = playerLoseMsg;
			resultText.color = playerLoseMsgColor;
		}
    }

    void OnSceneConfigured()
    {
        OnWaveUpdated(0);
        OnWaveCountdownUpdated();
    }

    // BUTTONS
    public void OnClickNextLevelButton()
    {
        int levelSceneCount = SceneManager.sceneCountInBuildSettings - 1;
        for (int i = levelSceneCount; i >= 1; --i) {
            if (sceneIndex == i) {
                SceneManager.LoadSceneAsync(i % levelSceneCount + 1);
                break;
            }
        }
    }
    //! BUTTONS

    void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        sceneIndex = _scene.buildIndex;
        if (sceneIndex != 0) {
            levelStartTrigger.onClick.Invoke();
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

