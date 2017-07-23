using UnityEngine;

public static class InspectorEventSignals
{
    #region Public members

    /// <summary>
    /// pending
    /// </summary>
    public static event SpawnEnemyInspectorUpdatedHandler OnSpawnEnemyInspectorUpdated;
    public delegate void SpawnEnemyInspectorUpdatedHandler();
    public static void DoSpawnEnemyInspectorUpdated() { if (OnSpawnEnemyInspectorUpdated != null) OnSpawnEnemyInspectorUpdated(); }

    /// <summary>
    /// pending
    /// </summary>
    public static event EnemySwitchDirCountUpdatedHandler OnEnemySwitchDirCountUpdated;
    public delegate void EnemySwitchDirCountUpdatedHandler(int _switchDirCount, GameObject _enemy);
    public static void DoEnemySwitchDirCountUpdated(int _switchDirCount, GameObject _enemy) { if (OnEnemySwitchDirCountUpdated != null) OnEnemySwitchDirCountUpdated(_switchDirCount, _enemy); }

    #endregion
}
