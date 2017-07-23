using System.Collections.Generic;
using UnityEngine;

// a base class used to handle the movement and explosion of a missile
public class Missile : MonoBehaviour
{
    public GameObject prefabExplosion;
    public GameObject prefabDestMarker;

    [Tooltip("the speed of the missile")]
    [SerializeField] protected float speed;
    [Tooltip("the lifetime of the explosion produced upon the missile's destruction")]
    public float explosionLifetime; 

    protected Queue<Vector3> path = new Queue<Vector3>(); // the points arranged in order that lie on the path taken by the missile
    Vector3 current = Vector3.zero;
    Vector3 next = Vector3.zero;
    bool hasSetDest = false;
    Explosion explosion;
    Billboard destMarker;

    protected virtual void Awake()
    {
        transform.SetParent(ManageSceneSetup.tempObjTransform);

        GameEventSignals.OnMissileExplode += OnMissileExplode;
        GameEventSignals.OnMissilePathDetermined += OnMissilePathDetermined;

        GameEventSignals.DoMissilePathDetermined();
    }

    // manage the missile's movement and destruction
    protected virtual void Update()
    {
        if (!Move())
            GameEventSignals.DoMissileExplode(this, prefabExplosion.GetComponent<Explosion>());
    }

    // manage the missile's movement and return a boolean value that indicates if the missile is moving
    bool Move()
    {
        if (hasSetDest) {
            if (current == next) {
                current = transform.position;
                if (path.Count == 0) return false;
                next = path.Dequeue();
                transform.LookAt(next);
            }
            else {
                transform.position += transform.forward * (speed * Time.deltaTime);
                if (Vector3.Distance(current, next) < Vector3.Distance(current, transform.position))
                    transform.position = current = next;
            }
        }
        return true;
    }

    public void SetDest(Vector3 _dest)
    {
        path.Enqueue(_dest);
        if (prefabDestMarker != null) {
            destMarker = Instantiate(prefabDestMarker, _dest, Quaternion.identity).GetComponent<Billboard>();
            destMarker.transform.SetParent(ManageSceneSetup.tempObjTransform);
        }
    }

    void OnMissileExplode(Missile _missile, Explosion _explosion)
    {
        if (this == _missile) {
            explosion = Instantiate(prefabExplosion, transform.position, Quaternion.LookRotation(Vector3.forward)).GetComponent<Explosion>();
            if (destMarker != null)
                Destroy(destMarker.gameObject);
            Destroy(gameObject);
        }
    }

    void OnMissilePathDetermined()
    {
        hasSetDest = true;
    }

    protected virtual void OnDestroy()
    {
        GameEventSignals.OnMissileExplode -= OnMissileExplode;
        GameEventSignals.OnMissilePathDetermined -= OnMissilePathDetermined;
        
        GameEventSignals.DoMissileNotifyExplosion(this, explosion);
    }
}
