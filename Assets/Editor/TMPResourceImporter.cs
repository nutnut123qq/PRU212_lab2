using UnityEngine;
using UnityEditor;

public class TMPResourceImporter : EditorWindow
{
    [MenuItem("Tools/Import TMP Essential Resources")]
    public static void ImportTMPEssentialResources()
    {
        Debug.Log("Đang cố gắng import TextMesh Pro Essential Resources...");

        // Thử mở menu TMP import trực tiếp
        try
        {
            EditorApplication.ExecuteMenuItem("Window/TextMeshPro/Import TMP Essential Resources");
            Debug.Log("Đã mở dialog import TMP Essential Resources. Hãy nhấn Import để hoàn thành.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Không thể mở dialog import TMP: " + e.Message);

            // Thử cách khác - tìm và import package trực tiếp
            string[] possiblePaths = {
                "Packages/com.unity.textmeshpro/Package Resources/TMP Essential Resources.unitypackage",
                "Library/PackageCache/com.unity.textmeshpro@*/Package Resources/TMP Essential Resources.unitypackage"
            };

            bool imported = false;
            foreach (string path in possiblePaths)
            {
                if (System.IO.File.Exists(path))
                {
                    AssetDatabase.ImportPackage(path, false);
                    Debug.Log("TextMesh Pro Essential Resources đã được import từ: " + path);
                    imported = true;
                    break;
                }
            }

            if (!imported)
            {
                Debug.LogWarning("Không thể tự động import TMP Essential Resources.");
                Debug.Log("Hãy thử thủ công: Window > TextMeshPro > Import TMP Essential Resources");
            }
        }
    }

    [MenuItem("Tools/Fix TMP Missing Resources")]
    public static void FixTMPMissingResources()
    {
        Debug.Log("=== BẮT ĐẦU SỬA LỖI TMP MISSING RESOURCES ===");

        // Kiểm tra xem TMP đã được cài đặt chưa
        if (!System.IO.Directory.Exists("Assets/TextMesh Pro"))
        {
            Debug.Log("Thư mục TextMesh Pro chưa tồn tại. Đang import...");
            ImportTMPEssentialResources();
        }
        else
        {
            Debug.Log("TextMesh Pro đã được import trước đó.");
        }

        // Refresh AssetDatabase
        AssetDatabase.Refresh();

        Debug.Log("=== HOÀN THÀNH ===");
        Debug.Log("Nếu vẫn có lỗi, hãy:");
        Debug.Log("1. Restart Unity Editor");
        Debug.Log("2. Thử Window > TextMeshPro > Import TMP Essential Resources");
    }
}
