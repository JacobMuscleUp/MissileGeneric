using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a base class used to enable the derived class to stop its per-frame update
public class Disableable : MonoBehaviour
{
    [HideInInspector] public GlobalManager.Ref<bool> disabled = new GlobalManager.Ref<bool>() { Value = false };
}
