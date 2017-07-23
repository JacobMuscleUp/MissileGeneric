using UnityEngine;

public class CounterMissile : Missile
{
    [Tooltip("if the missile is spawned with a random color")]
    [SerializeField] bool randomColorMode;

    Renderer rendererComp;

    protected override void Awake()
    {
        base.Awake();
        if (!randomColorMode) return;

        rendererComp = GetComponentInChildren<Renderer>();
        rendererComp.material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (!randomColorMode) return;

        Destroy(rendererComp.material);
    }
}
