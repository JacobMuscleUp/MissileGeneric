using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyMissile))]
public class EnemyMissileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var inspectedTarget = target as EnemyMissile;
        var oldSwitchDirCount = inspectedTarget.switchDirCount;
        if (!EditorManager.verticalEnemyPath)
            inspectedTarget.switchDirCount = EditorGUILayout.IntSlider(new GUIContent(
                "Switch Dir Count", "The number of times an enemy missile switches its direction before going straight towards one of the cannons"
                ), inspectedTarget.switchDirCount, 0, 8);
        
        if (oldSwitchDirCount != inspectedTarget.switchDirCount)
            InspectorEventSignals.DoEnemySwitchDirCountUpdated(inspectedTarget.switchDirCount, inspectedTarget.gameObject);
    }
}
