using UnityEngine;

public static class GameEventSignals
{
    #region Public members

    /// <summary>
    /// pending
    /// </summary>
    public static event MissileExplodeHandler OnMissileExplode;
    public delegate void MissileExplodeHandler(Missile _missile, Explosion _explosion);
    public static void DoMissileExplode(Missile _missile, Explosion _explosion) { if (OnMissileExplode != null) OnMissileExplode(_missile, _explosion); }

    /// <summary>
    /// pending
    /// </summary>
    public static event MissileNotifyExplosionHandler OnMissileNotifyExplosion;
    public delegate void MissileNotifyExplosionHandler(Missile _missile, Explosion _explosion);
    public static void DoMissileNotifyExplosion(Missile _missile, Explosion _explosion) { if (OnMissileNotifyExplosion != null) OnMissileNotifyExplosion(_missile, _explosion); }

    /// <summary>
    /// pending
    /// </summary>
    public static event CannonFireHandler OnCannonFire;
    public delegate void CannonFireHandler(Vector3 _target);
    public static void DoCannonFire(Vector3 _target) { if (OnCannonFire != null) OnCannonFire(_target); }

    /// <summary>
    /// pending
    /// </summary>
    public static event CannonFailToFireHandler OnCannonFailToFire;
    public delegate void CannonFailToFireHandler();
    public static void DoCannonFailToFire() { if (OnCannonFailToFire != null) OnCannonFailToFire(); }

    /// <summary>
    /// pending
    /// </summary>
    public static event CannonRechargeHandler OnCannonRecharge;
    public delegate void CannonRechargeHandler(Cannon _cannon);
    public static void DoCannonRecharge(Cannon _cannon) { if (OnCannonRecharge != null) OnCannonRecharge(_cannon); }

    /// <summary>
    /// pending
    /// </summary>
    public static event AllCannonsDestroyedHandler OnAllCannonsDestroyed;
    public delegate void AllCannonsDestroyedHandler();
    public static void DoAllCannonsDestroyed() { if (OnAllCannonsDestroyed != null) OnAllCannonsDestroyed(); }

    /// <summary>
    /// pending
    /// </summary>
    public static event MissilePathDeterminedHandler OnMissilePathDetermined;
    public delegate void MissilePathDeterminedHandler();
    public static void DoMissilePathDetermined() { if (OnMissilePathDetermined != null) OnMissilePathDetermined(); }

    /// <summary>
    /// pending
    /// </summary>
    public static event EntityDamagedHandler OnEntityDamaged;
    public delegate void EntityDamagedHandler(MonoBehaviour _target);
    public static void DoEntityDamaged(MonoBehaviour _target) { if (OnEntityDamaged != null) OnEntityDamaged(_target); }

    /// <summary>
    /// pending
    /// </summary>
    public static event EnemySpawnedHandler OnEnemySpawned;
    public delegate void EnemySpawnedHandler(EnemyMissile _enemy);
    public static void DoEnemySpawned(EnemyMissile _enemy) { if (OnEnemySpawned != null) OnEnemySpawned(_enemy); }

    /// <summary>
    /// pending
    /// </summary>
    public static event WaveUpdatedHandler OnWaveUpdated;
    public delegate void WaveUpdatedHandler(int _waveIndex);
    public static void DoWaveUpdated(int _waveIndex) { if (OnWaveUpdated != null) OnWaveUpdated(_waveIndex); }

    /// <summary>
    /// pending
    /// </summary>
    public static event LastWaveEndHandler OnLastWaveEnd;
    public delegate void LastWaveEndHandler();
    public static void DoLastWaveEnd() { if (OnLastWaveEnd != null) OnLastWaveEnd(); }

    /// <summary>
    /// pending
    /// </summary>
    public static event WaveCountdownUpdatedHandler OnWaveCountdownUpdated;
    public delegate void WaveCountdownUpdatedHandler();
    public static void DoWaveCountdownUpdated() { if (OnWaveCountdownUpdated != null) OnWaveCountdownUpdated(); }

    /// <summary>
    /// pending
    /// </summary>
    public static event LevelEndHandler OnLevelEnd;
    public delegate void LevelEndHandler(ManageState.EnumLevelEndState _state);
    public static void DoLevelEnd(ManageState.EnumLevelEndState _state) { if (OnLevelEnd != null) OnLevelEnd(_state); }

    /// <summary>
    /// pending
    /// </summary>
    public static event SceneConfiguredHandler OnSceneConfigured;
    public delegate void SceneConfiguredHandler();
    public static void DoSceneConfigured() { if (OnSceneConfigured != null) OnSceneConfigured(); }

    #endregion
}
