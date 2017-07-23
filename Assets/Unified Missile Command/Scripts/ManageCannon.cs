using UnityEngine;

// a class used to automate the process of creating and evenly positioning cannons across the bottom of the playfield
public class ManageCannon : MonoBehaviour
{
    [SerializeField] GameObject prefabCannon;

    [Range(1, 9)]
    [Tooltip("the number of cannons to be created")]
    [SerializeField] int cannonCount = 3;

    void Awake()
    {
        GameEventSignals.OnSceneConfigured += OnSceneConfigured;
    }

    void OnDestroy()
    {
        GameEventSignals.OnSceneConfigured -= OnSceneConfigured;
    }

    void OnSceneConfigured()
    {
        float intervalCountInverse = 1.0f / (cannonCount - 1);
        for (int i = 0; i < cannonCount; ++i) {
            Instantiate(prefabCannon, new Vector3(
                ManageSceneSetup.Ins.spawnHorizontalRange.x + ManageSceneSetup.Ins.playFieldWidth * i * intervalCountInverse
                , ManageSceneSetup.Ins.spawnVerticalRange.x
                , ManageSceneSetup.Ins.playField.transform.position.z), prefabCannon.transform.rotation);
        }
    }
}
