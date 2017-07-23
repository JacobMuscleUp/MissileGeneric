using System.Collections.Generic;

// a class used to contain some global variables used by other classes and global methods used to manipulate those global variables 
public static class GlobalManager
{
    public class Ref<T> where T : struct
    {
        public T Value { get; set; }
    }
    
    public static List<Cannon> cannons = new List<Cannon>();
    static LinkedList<Ref<bool>> disabledObjLevers = new LinkedList<Ref<bool>>();

    public static void DisableAllUpdates()
    {
        foreach (var elem in disabledObjLevers)
            elem.Value = true;
    }

    public static void EnableAllUpdates()
    {
        foreach (var elem in disabledObjLevers)
            elem.Value = false;
    }

    public static void AddToDisabledObjContainer(Disableable _obj)
    {
        disabledObjLevers.AddLast(_obj.disabled);
    }
}
