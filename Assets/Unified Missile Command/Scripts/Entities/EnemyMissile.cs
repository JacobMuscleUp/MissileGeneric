using UnityEngine;

public class EnemyMissile : Missile
{
    [HideInInspector] public int switchDirCount;

    [HideInInspector] public static bool isFirstInstance = true;

    protected override void Awake()
    {
        if (!SpawnEnemy.Ins.verticalEnemyPath) {
            float intervalCount = switchDirCount + 1;
            for (float i = intervalCount - 1; i > 0; --i) {
                path.Enqueue(new Vector3(
                    Random.Range(ManageSceneSetup.Ins.spawnHorizontalRange.x, ManageSceneSetup.Ins.spawnHorizontalRange.y)
                    , ManageSceneSetup.Ins.spawnVerticalRange.x + i / intervalCount * ManageSceneSetup.Ins.playFieldLength
                    , ManageSceneSetup.Ins.playField.transform.position.z));
            }
        }

        if (isFirstInstance && !SpawnEnemy.Ins.verticalEnemyPath) {
            isFirstInstance = false;
            InspectorEventSignals.DoEnemySwitchDirCountUpdated(switchDirCount, gameObject);
        }

        base.Awake();
    }
}
