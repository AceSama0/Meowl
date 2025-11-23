using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData 
{
    public Vector3 playerTransform;
    public string mapBoundary;
    public List<InventorySaveData> InventorySaveData;
}

