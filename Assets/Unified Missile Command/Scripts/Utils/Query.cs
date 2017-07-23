using System.Collections.Generic;
using UnityEngine;

public static class Query
{
    /// <summary>
    /// apply the breadth-first search to _target in an attempt to find a game object with name _name. 
    /// If a game object with name _name exists, then the game object is returned. Otherwise, null is return.
    /// </summary>
    public static GameObject FindInGameObject(GameObject _target, string _name)
    {
        if (_target != null) {
            if (_target.name == _name)
                return _target;

            Queue<GameObject> queue = new Queue<GameObject>();
            queue.Enqueue(_target);
            while (queue.Count != 0) {
                var targetTransform = queue.Dequeue().transform;
                for (int i = 0; i < targetTransform.childCount; ++i) {
                    var child = targetTransform.GetChild(i);
                    queue.Enqueue(child.gameObject);
                    if (child.name == _name)
                        return child.gameObject;
                }
            }
        }
        return null;
    }
}
