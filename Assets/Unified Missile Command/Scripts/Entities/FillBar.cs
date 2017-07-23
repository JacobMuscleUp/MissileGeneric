using UnityEngine;

// a class used to enable the Sprite Renderer game object to which the class is attached to change its scale dynamically
public class FillBar : MonoBehaviour
{
    Vector3 scale;

    void Awake()
    {
        scale = transform.localScale;
    }

    public void UpdateScale(float _fillPercent)
    {
        transform.localScale = new Vector3(_fillPercent * scale.x, scale.y, scale.z);
    }
}
