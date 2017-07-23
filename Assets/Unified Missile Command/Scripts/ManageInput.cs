using UnityEngine;
using UnityEngine.SceneManagement;

// a class used to handle all inputs related to cannon firing
public class ManageInput : Disableable
{
    public static ManageInput Ins { get; private set; }

    [Tooltip("the useful reminder")] 
    [SerializeField] string reminder = string.Empty;

    int sceneIndex;

    void Awake()
    {
        if (!InitGlobalIns()) return;
        GlobalManager.AddToDisabledObjContainer(this);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        if (!disabled.Value) {
            CheckCannonButtons();
            AutoSelectCannonToFire();
        }
        RespondToLevelTransitionRequest();
    }

    Vector3 CalcMissileDest()
    {
        Vector3 mousePos = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(new Vector3(
            mousePos.x, mousePos.y, ManageSceneSetup.Ins.playField.transform.position.z - Camera.main.transform.position.z));
    }

    // queries the occurrence of the event of pressing a button corresponding to one of the cannons
    void CheckCannonButtons()
    {
        if (!Input.anyKeyDown) return;

        var cannons = GlobalManager.cannons;
        for (int i = 0; i < cannons.Count; ++i) {
            if (Input.GetKeyDown((i + 1).ToString())) {
                FireCannon(cannons[i], CalcMissileDest());
                break;
            }
        }
    }

    void AutoSelectCannonToFire()
    {
        if (Input.GetMouseButtonDown(0)) {
            var targetPos = CalcMissileDest();
            FireCannon(GetClosestCannon(targetPos), targetPos);
        }
    }

    void RespondToLevelTransitionRequest()
    {
        if (Input.GetKeyDown("l")) {
            Time.timeScale = 0;

            int levelSceneCount = SceneManager.sceneCountInBuildSettings - 1;
            for (int i = levelSceneCount; i >= 1; --i) {
                if (sceneIndex == i) {
                    SceneManager.LoadSceneAsync(i % levelSceneCount + 1);
                    break;
                }
            }
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        sceneIndex = _scene.buildIndex;
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

    // HELPER FUNC
    void FireCannon(Cannon _cannon, Vector3 _targetPos)
    {
        if (_targetPos.x > ManageSceneSetup.Ins.spawnHorizontalRange.y
            || _targetPos.x < ManageSceneSetup.Ins.spawnHorizontalRange.x
            || _targetPos.y > ManageSceneSetup.Ins.spawnVerticalRange.y
            || _targetPos.y < ManageSceneSetup.Ins.spawnVerticalRange.x)
            return;

        _cannon.FireNow();
        if (_cannon.IsRecharging() || _cannon.AmmoIsDepleted())
            GameEventSignals.DoCannonFailToFire();
        else
            GameEventSignals.DoCannonFire(_targetPos);
    }

    Cannon GetClosestCannon(Vector3 _src)
    {
        Cannon closestCannon = null;
        float minDist = float.MaxValue;

        foreach (var elem in GlobalManager.cannons) {
            float currentDist = Vector3.Distance(elem.transform.position, _src);
            if (minDist > currentDist) {
                minDist = currentDist;
                closestCannon = elem;
            }
        }
        return closestCannon;
    }
    //! HELPER FUNC
}
