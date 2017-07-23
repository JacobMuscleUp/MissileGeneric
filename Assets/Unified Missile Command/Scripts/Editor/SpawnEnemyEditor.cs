using UnityEditor;

[CustomEditor(typeof(SpawnEnemy))]
public class SpawnEnemyEditor : Editor
{
    void Awake()
    {
        EditorManager.verticalEnemyPath = CurrentVerticalEnemyPath();
    }
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        var oldVerticalEnemyPath = EditorManager.verticalEnemyPath;
        EditorManager.verticalEnemyPath = CurrentVerticalEnemyPath();
        if (oldVerticalEnemyPath != EditorManager.verticalEnemyPath)
            InspectorEventSignals.DoSpawnEnemyInspectorUpdated();
    }

    bool CurrentVerticalEnemyPath()
    {
        var inspectedTarget = target as SpawnEnemy;
        return inspectedTarget.verticalEnemyPath;
    }
}
