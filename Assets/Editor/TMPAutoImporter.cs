using UnityEngine;
using UnityEditor;
using System.IO;

[InitializeOnLoad]
public class TMPAutoImporter
{
    private static bool hasShownWarning = false;

    static TMPAutoImporter()
    {
        EditorApplication.delayCall += CheckAndShowTMPWarning;
    }

    private static void CheckAndShowTMPWarning()
    {
        // Kiểm tra xem TMP Essential Resources đã được import chưa
        string tmpResourcesPath = "Assets/TextMesh Pro/Resources";

        if (!Directory.Exists(tmpResourcesPath) && !hasShownWarning)
        {
            hasShownWarning = true;

            Debug.LogWarning("=== CẢNH BÁO: TEXTMESH PRO RESOURCES THIẾU ===");
            Debug.LogWarning("TextMesh Pro Essential Resources chưa được import!");
            Debug.LogWarning("Để sửa lỗi này:");
            Debug.LogWarning("1. Vào menu: Window > TextMeshPro > Import TMP Essential Resources");
            Debug.LogWarning("2. Hoặc vào menu: Tools > Fix TMP Missing Resources");
            Debug.LogWarning("3. Nhấn Import trong dialog xuất hiện");
            Debug.LogWarning("===============================================");

            // Hiển thị dialog cảnh báo
            bool shouldImport = EditorUtility.DisplayDialog(
                "TextMesh Pro Resources Thiếu",
                "TextMesh Pro Essential Resources chưa được import.\n\n" +
                "Điều này có thể gây ra lỗi trong dự án của bạn.\n\n" +
                "Bạn có muốn mở dialog import ngay bây giờ không?",
                "Mở Dialog Import",
                "Để sau"
            );

            if (shouldImport)
            {
                TMPResourceImporter.ImportTMPEssentialResources();
            }
        }
    }
}
