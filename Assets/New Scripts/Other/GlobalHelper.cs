using UnityEngine;
using UnityEngine.UIElements;

public static class GlobalHelper 
{
    public static string GenereteUniqueID(GameObject obj)
    {
        return $"{obj.scene.name}_{obj.transform.position.x}_{obj.transform.position.y}";
    }
}
