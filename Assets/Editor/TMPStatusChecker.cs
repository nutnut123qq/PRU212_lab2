using UnityEngine;
using UnityEditor;
using System.IO;

public class TMPStatusChecker : EditorWindow
{
    [MenuItem("Tools/Check TMP Status")]
    public static void ShowWindow()
    {
        GetWindow<TMPStatusChecker>("TMP Status Checker");
    }
    
    private void OnGUI()
    {
        GUILayout.Label("TextMesh Pro Status Checker", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        // Kiểm tra package
        bool packageInstalled = CheckTMPPackage();
        DrawStatusLine("TextMesh Pro Package", packageInstalled);
        
        // Kiểm tra Essential Resources
        bool resourcesImported = CheckTMPResources();
        DrawStatusLine("Essential Resources", resourcesImported);
        
        // Kiểm tra TMP objects trong scene
        int tmpObjectCount = CountTMPObjectsInScene();
        GUILayout.Label($"Text (TMP) objects trong scene hiện tại: {tmpObjectCount}");
        
        GUILayout.Space(20);
        
        if (!packageInstalled || !resourcesImported)
        {
            GUILayout.Label("⚠️ Cần sửa lỗi:", EditorStyles.boldLabel);
            
            if (!packageInstalled)
            {
                GUILayout.Label("- TextMesh Pro package chưa được cài đặt");
            }
            
            if (!resourcesImported)
            {
                GUILayout.Label("- Essential Resources chưa được import");
                
                if (GUILayout.Button("Import TMP Essential Resources"))
                {
                    TMPResourceImporter.ImportTMPEssentialResources();
                }
            }
        }
        else
        {
            GUILayout.Label("✅ TextMesh Pro đã được cài đặt đầy đủ!", EditorStyles.boldLabel);
        }
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("Refresh Status"))
        {
            Repaint();
        }
        
        if (GUILayout.Button("Open README"))
        {
            string readmePath = Path.Combine(Application.dataPath, "../README_TMP_FIX.md");
            if (File.Exists(readmePath))
            {
                Application.OpenURL("file://" + readmePath);
            }
            else
            {
                Debug.LogWarning("Không tìm thấy file README_TMP_FIX.md");
            }
        }
    }
    
    private void DrawStatusLine(string label, bool status)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label + ":");
        
        GUI.color = status ? Color.green : Color.red;
        GUILayout.Label(status ? "✅ OK" : "❌ Missing");
        GUI.color = Color.white;
        
        GUILayout.EndHorizontal();
    }
    
    private bool CheckTMPPackage()
    {
        // Kiểm tra xem có assembly Unity.TextMeshPro không
        var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            if (assembly.GetName().Name == "Unity.TextMeshPro")
            {
                return true;
            }
        }
        return false;
    }
    
    private bool CheckTMPResources()
    {
        return Directory.Exists("Assets/TextMesh Pro/Resources");
    }
    
    private int CountTMPObjectsInScene()
    {
        var tmpComponents = FindObjectsOfType<TMPro.TextMeshProUGUI>();
        return tmpComponents.Length;
    }
}
