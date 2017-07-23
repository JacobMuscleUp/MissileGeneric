using UnityEngine;

// a class used to enable the game object to which the class is attached to always face the main camera
public class Billboard : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Camera.main.transform.localRotation;
    }
}
