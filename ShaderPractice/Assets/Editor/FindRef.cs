using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetBeDepend
{
    // 存储所有依赖关系
    private static Dictionary<string, List<string>> referenceCacheDic = new Dictionary<string, List<string>>();

    // 在 Assets 文件件下鼠标右键弹板中添加按钮 AssetBeDepend
    [MenuItem("Assets/AssetBeDepend")]
    static void Select()
    {
        CollectDepend();

        // 获取所有选中 文件、文件夹的 GUID
        string[] guids = Selection.assetGUIDs;
        foreach (var guid in guids)
        {
            // 将 GUID 转换为 路径
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            IsBeDepend(assetPath);
        }
    }

    // 收集项目中所有依赖关系
    static void CollectDepend()
    {
        int count = 0;
        // 获取 Assets 文件夹下所有资源
        string[] guids = AssetDatabase.FindAssets("");
        foreach (string guid in guids)
        {
            // 将 GUID 转换为路径
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            // 获取文件所有直接依赖的资源
            string[] dependencies = AssetDatabase.GetDependencies(assetPath, false);

            foreach (var filePath in dependencies)
            {
                // dependency 被 assetPath 依赖了
                // 将所有依赖关系存储到字典中
                List<string> list = null;
                if (!referenceCacheDic.TryGetValue(filePath, out list))
                {
                    list = new List<string>();
                    referenceCacheDic[filePath] = list;
                }
                list.Add(assetPath);
            }

            count++;
            EditorUtility.DisplayProgressBar("Search Dependencies", "Dependencies", (float)(count * 1.0f / guids.Length));
        }

        EditorUtility.ClearProgressBar();
    }

    // 判断文件是否被依赖
    static bool IsBeDepend(string filePath)
    {
        List<string> list = null;
        if (!referenceCacheDic.TryGetValue(filePath, out list))
        {
            return false;
        }

        // 将依赖关系打印出来
        foreach (var file in list)
        {
            Debug.LogError(filePath + "   被:" + file + "    引用");
        }

        return true;
    }

}
