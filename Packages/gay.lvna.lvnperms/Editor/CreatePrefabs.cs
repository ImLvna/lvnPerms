using UnityEditor;
using UnityEngine;

namespace gay.lvna.DisBridge
{
  public class CreatePrefabs
  {

    private const string filePath = "Packages/gay.lvna.lvnperms/Runtime/Prefabs/";


    [MenuItem("GameObject/Luna/lvnPerms/lvnPerms")]
    static void CreateManager(MenuCommand cmd)
    {
      CreatePrefab("lvnPerms.prefab", cmd, true);
    }

    private static void CreatePrefab(string name, MenuCommand cmd, bool unpack = false)
    {
      var prefab = AssetDatabase.LoadAssetAtPath(filePath + name, typeof(UnityEngine.Object));
      if (prefab == null)
      {
        Debug.LogError("Prefab not found at " + filePath + name);
        return;
      }

      GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
      Undo.RegisterCreatedObjectUndo((Object)go, "Create " + go.name);

      if (cmd.context is GameObject)
      {
        GameObject parent = (GameObject)cmd.context;
        go.transform.SetParent(parent.transform, false);
      }

      Selection.activeObject = go;
      if (unpack)
      {
        PrefabUtility.UnpackPrefabInstance(go, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
      }
    }
  }
}