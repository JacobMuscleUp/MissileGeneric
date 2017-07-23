using UnityEngine;

// a class used to handle the visibility of the default cursor and the calculation of the custom cursor's world position
public class ManageCursor : MonoBehaviour
{
    void Awake()
    {
        Cursor.visible = false;
    }

    void OnDestroy()
    {
        Cursor.visible = true;
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        // ScreenToWorldPoint is used because the camera's direction vector is always perpendicular to the play field's surface
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(
            mousePos.x, mousePos.y, ManageSceneSetup.Ins.playField.transform.position.z - Camera.main.transform.position.z));

        if ((Input.GetAxis("Fire1") == 1 && Cursor.visible)
            || (Input.GetAxis("Fire2") == 1 && Cursor.visible)
            || (Input.GetAxis("Fire3") == 1 && Cursor.visible))
            Cursor.visible = false;
    }
}
