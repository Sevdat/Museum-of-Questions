using UnityEngine;
using UnityEditor;

public class SaveAsPrefab : MonoBehaviour
{
    static void saveSelectedAsPrefab(GameObject selected)
    {
        string resourcesPath = "Assets/Resources/";
        string path = resourcesPath + selected.name + ".prefab";
        PrefabUtility.SaveAsPrefabAsset(selected, path);
    }
}