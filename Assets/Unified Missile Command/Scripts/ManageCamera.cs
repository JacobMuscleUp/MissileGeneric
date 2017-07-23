using UnityEngine;

// a class used to enable the main camera to automatically position and orient itself in such a way that the entire play field is clearly seen
public class ManageCamera : MonoBehaviour
{
    [Tooltip("the camera is automatically positioned and oriented in such a way that it can see the entire play field if this property is ticked")]
    [SerializeField] bool convenientMode;
    [Tooltip("The width of the additional space between any of the four camera boundaries and the corresponding play field boundary")]
    [SerializeField] float additionalSpaceWidth = 2;

    void Awake()
    {
        GameEventSignals.OnSceneConfigured += OnSceneConfigured;
    }

    void OnDestroy()
    {
        GameEventSignals.OnSceneConfigured -= OnSceneConfigured;
    }

    void OnSceneConfigured()
    {
        if (!convenientMode) return;

        float radianHalfVerticalFOV = Mathf.Deg2Rad * Camera.main.fieldOfView * 0.5f;
        float distance0 = (ManageSceneSetup.Ins.playFieldLength * 0.5f + additionalSpaceWidth) / Mathf.Tan(radianHalfVerticalFOV);
        float distance1 = (ManageSceneSetup.Ins.playFieldWidth * 0.5f + additionalSpaceWidth) / (Camera.main.aspect * Mathf.Tan(radianHalfVerticalFOV));
        transform.rotation = ManageSceneSetup.Ins.playField.transform.rotation;
        transform.position = ManageSceneSetup.Ins.playField.transform.position;
        transform.position -= transform.forward * Mathf.Max(distance0, distance1);
    }
}
