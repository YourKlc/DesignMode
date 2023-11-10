using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetBeDepend
{
    // �洢����������ϵ
    private static Dictionary<string, List<string>> referenceCacheDic = new Dictionary<string, List<string>>();

    // �� Assets �ļ���������Ҽ���������Ӱ�ť AssetBeDepend
    [MenuItem("Assets/AssetBeDepend")]
    static void Select()
    {
        CollectDepend();

        // ��ȡ����ѡ�� �ļ����ļ��е� GUID
        string[] guids = Selection.assetGUIDs;
        foreach (var guid in guids)
        {
            // �� GUID ת��Ϊ ·��
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            IsBeDepend(assetPath);
        }
    }

    // �ռ���Ŀ������������ϵ
    static void CollectDepend()
    {
        int count = 0;
        // ��ȡ Assets �ļ�����������Դ
        string[] guids = AssetDatabase.FindAssets("");
        foreach (string guid in guids)
        {
            // �� GUID ת��Ϊ·��
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            // ��ȡ�ļ�����ֱ����������Դ
            string[] dependencies = AssetDatabase.GetDependencies(assetPath, false);

            foreach (var filePath in dependencies)
            {
                // dependency �� assetPath ������
                // ������������ϵ�洢���ֵ���
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

    // �ж��ļ��Ƿ�����
    static bool IsBeDepend(string filePath)
    {
        List<string> list = null;
        if (!referenceCacheDic.TryGetValue(filePath, out list))
        {
            return false;
        }

        // ��������ϵ��ӡ����
        foreach (var file in list)
        {
            Debug.LogError(filePath + "   ��:" + file + "    ����");
        }

        return true;
    }

}
