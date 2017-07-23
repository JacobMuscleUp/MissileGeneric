using UnityEngine;
using System.Collections.Generic;

public class Cannon : MonoBehaviour
{
    public GameObject prefabCounterMissile;
    public FillBar healthBar;
    public FillBar rechargeBar;

    [Tooltip("the number of times the cannon has to be damaged before it is destroyed")]
    [SerializeField] int initialHealth;
    [Tooltip("the time required by the cannon to recharge")]
    [SerializeField] float rechargeTime;
    [Tooltip("the amount of ammunition available, set to a negative value to enable unlimited ammo")]
    [SerializeField] int ammoCount;

    float health;
    float initialHealthInverse;
    float rechargeTimer;
    float rechargeTimeInverse;

    bool fireNow = false; // a boolean field that indicates if the cannon should fire now

    void Awake()
    {
        transform.SetParent(ManageSceneSetup.cannonParentTransform);

        GlobalManager.cannons.Add(this);
        GlobalManager.cannons.Sort(SortByHorizontalPosition);
        
        GameEventSignals.OnCannonFire += OnCannonFire;
        GameEventSignals.OnCannonFailToFire += OnCannonFailToFire;
        GameEventSignals.OnCannonRecharge += OnCannonRecharge;
        GameEventSignals.OnEntityDamaged += OnEntityDamaged;
        GameEventSignals.OnSceneConfigured += OnSceneConfigured;
        
        health = initialHealth;
        initialHealthInverse = 1 / health;
        rechargeTimer = rechargeTime;
        rechargeTimeInverse = 1 / rechargeTime;
    }

    void Update()
    {
        Recharge();
        if (health <= 0.0f)
            Destroy(gameObject);
    }

    void Recharge()
    {
        if (rechargeTimer < rechargeTime) {
            rechargeTimer += Time.deltaTime;
            GameEventSignals.DoCannonRecharge(this);
        }
    }

    public bool IsRecharging()
    {
        return rechargeTimer < rechargeTime;
    }

    public bool AmmoIsDepleted()
    {
        if (ammoCount < 0) return false;
        return ammoCount == 0;
    }

    // notify the cannon to fire immediately
    public void FireNow()
    {
        fireNow = true;
    }

    void OnDestroy()
    {
        GlobalManager.cannons.Remove(this);
        GlobalManager.cannons.Sort(SortByHorizontalPosition);
        if (GlobalManager.cannons.Count == 0)
            GameEventSignals.DoAllCannonsDestroyed();
        
        GameEventSignals.OnCannonFire -= OnCannonFire;
        GameEventSignals.OnCannonFailToFire -= OnCannonFailToFire;
        GameEventSignals.OnCannonRecharge -= OnCannonRecharge;
        GameEventSignals.OnEntityDamaged -= OnEntityDamaged;
        GameEventSignals.OnSceneConfigured -= OnSceneConfigured;
    }

    void OnCannonFire(Vector3 _target)
    {
        if (fireNow) {
            fireNow = false;

            --ammoCount;
            rechargeTimer = 0.0f;
            var counterMissile = Instantiate(prefabCounterMissile).GetComponent<CounterMissile>();
            counterMissile.transform.position = transform.position;
            counterMissile.transform.LookAt(_target);
            counterMissile.SetDest(_target);
        }
    }

    void OnCannonFailToFire()
    {
        if (fireNow) {
            fireNow = false;
        }
    }

    void OnCannonRecharge(Cannon _cannon)
    {
        if (this == _cannon)
            rechargeBar.UpdateScale(rechargeTimer * rechargeTimeInverse);
    }

    void OnEntityDamaged(MonoBehaviour _target)
    {
        if (this == _target) {
            --health;
            healthBar.UpdateScale(health * initialHealthInverse);
        }
    }

    void OnSceneConfigured()
    {
        transform.SetParent(ManageSceneSetup.cannonParentTransform);
        transform.position = new Vector3(transform.position.x, ManageSceneSetup.Ins.spawnVerticalRange.x, ManageSceneSetup.Ins.playField.transform.position.z);
    }

    // PROPERTY
    public static IComparer<Cannon> SortByHorizontalPosition
    { get { return new HorizontalPositionComparer(); } }
    //! PROPERTY

    // COMPARER
    public class HorizontalPositionComparer : IComparer<Cannon>
    {
        int IComparer<Cannon>.Compare(Cannon _arg0, Cannon _arg1)
        {
            return _arg0.transform.position.x.CompareTo(_arg1.transform.position.x);
        }
    }
    //! COMPARER
}
