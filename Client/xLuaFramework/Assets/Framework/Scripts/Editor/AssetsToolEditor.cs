using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssetsToolEditor
{
    [MenuItem("Assets/Yz Export Assets")]
    public static void Build()
    {
        if (Selection.objects == null) return;
        List<string> paths = new List<string>();
        foreach (Object o in Selection.objects)
        {
            paths.Add(AssetDatabase.GetAssetPath(o));
        }

        AssetDatabase.ExportPackage(paths.ToArray(),
            "Assets/UnityPackage/" + Selection.objects[0].name + ".unitypackage",
            ExportPackageOptions.IncludeDependencies);
        AssetDatabase.Refresh();
        Debug.Log("导出场景【" + Selection.objects[0].name + "】完毕！");
    }
}
