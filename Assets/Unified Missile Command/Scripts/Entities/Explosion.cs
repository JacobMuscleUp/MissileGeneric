using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AnimationCurve animCurve = AnimationCurve.Linear(0, 0, 1, 1);
    float lifetime;
    float lifetimeHelper;
    float lifetimeInverse;
    Transform modelTransform;
    Vector3 scale;

    void Awake()
    {
        GameEventSignals.OnMissileNotifyExplosion += OnMissileNotifyExplosion;

        modelTransform = transform.GetChild(0);
        scale = modelTransform.localScale;
        transform.SetParent(ManageSceneSetup.tempObjTransform);
    }

    void OnDestroy()
    {
        GameEventSignals.OnMissileNotifyExplosion -= OnMissileNotifyExplosion;
    }

    void OnMissileNotifyExplosion(Missile _missile, Explosion _explosion)
    {
        if (_explosion == this) {
            lifetime = _missile.explosionLifetime;
            lifetimeHelper = 0.0f;
            lifetimeInverse = 1 / lifetime;
        }
    }

    // manage the lifetime of the explosion
    void Update()
    {
        lifetimeHelper += Time.deltaTime;
        modelTransform.localScale = scale * Mathf.LerpUnclamped(0.1f, 1.0f, animCurve.Evaluate(lifetimeHelper * lifetimeInverse));
        if (lifetimeHelper >= lifetime)
            Destroy(gameObject);
    }

    // apply damage to all damageable game entities that are within the bounds of the explosion
    void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("EnemyMissile")) {
            GameEventSignals.DoMissileExplode(_other.GetComponent<EnemyMissile>(), this);
        }
        else if (_other.CompareTag("CounterMissile")) {
            GameEventSignals.DoMissileExplode(_other.GetComponent<CounterMissile>(), this);
        }
        else if (_other.CompareTag("Cannon")) {
            GameEventSignals.DoEntityDamaged(_other.GetComponent<Cannon>());
        }
    }
}

