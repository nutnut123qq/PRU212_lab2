using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SimpleCameraFollow))]
public class SimpleCameraFollowEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SimpleCameraFollow cameraFollow = (SimpleCameraFollow)target;
        
        // Vẽ Inspector mặc định
        DrawDefaultInspector();
        
        GUILayout.Space(10);
        
        // Hiển thị trạng thái
        if (cameraFollow.player != null)
        {
            EditorGUILayout.HelpBox("✅ Player đã được gán: " + cameraFollow.player.name, MessageType.Info);
        }
        else
        {
            EditorGUILayout.HelpBox("❌ Player chưa được gán!", MessageType.Warning);
        }
        
        GUILayout.Space(5);
        
        // Button để tự động tìm player
        if (GUILayout.Button("Tự động tìm Player"))
        {
            FindAndAssignPlayer(cameraFollow);
        }
        
        // Button để test camera follow
        if (Application.isPlaying && cameraFollow.player != null)
        {
            if (GUILayout.Button("Test Camera Follow"))
            {
                // Di chuyển camera đến vị trí player + offset
                Camera cam = cameraFollow.GetComponent<Camera>();
                if (cam != null)
                {
                    cam.transform.position = cameraFollow.player.position + cameraFollow.offset;
                }
            }
        }
        
        GUILayout.Space(10);
        
        // Hiển thị hướng dẫn
        EditorGUILayout.HelpBox(
            "Hướng dẫn:\n" +
            "1. Nhấn 'Tự động tìm Player' để script tự tìm\n" +
            "2. Hoặc kéo thả Player object vào field 'Player'\n" +
            "3. Điều chỉnh Offset và Smooth Speed theo ý muốn", 
            MessageType.Info
        );
    }
    
    private void FindAndAssignPlayer(SimpleCameraFollow cameraFollow)
    {
        // Tìm theo tag "Player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        
        if (playerObj != null)
        {
            cameraFollow.player = playerObj.transform;
            EditorUtility.SetDirty(cameraFollow);
            Debug.Log("✅ Đã gán Player: " + playerObj.name);
        }
        else
        {
            // Tìm theo tên "Barry"
            playerObj = GameObject.Find("Barry");
            if (playerObj != null)
            {
                cameraFollow.player = playerObj.transform;
                EditorUtility.SetDirty(cameraFollow);
                Debug.Log("✅ Đã gán Barry: " + playerObj.name);
            }
            else
            {
                Debug.LogWarning("❌ Không tìm thấy Player! Hãy đảm bảo:");
                Debug.LogWarning("1. Player object có tag 'Player'");
                Debug.LogWarning("2. Hoặc player object có tên 'Barry'");
                Debug.LogWarning("3. Player object có trong scene hiện tại");
                
                // Hiển thị tất cả objects có thể là player
                GameObject[] allObjects = FindObjectsOfType<GameObject>();
                Debug.Log("Các objects có thể là player:");
                foreach (GameObject obj in allObjects)
                {
                    if (obj.name.ToLower().Contains("player") || 
                        obj.name.ToLower().Contains("barry") ||
                        obj.tag == "Player")
                    {
                        Debug.Log("- " + obj.name + " (Tag: " + obj.tag + ")");
                    }
                }
            }
        }
    }
}

// Custom Editor cho CameraFollow nâng cao
[CustomEditor(typeof(CameraFollow))]
public class CameraFollowEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CameraFollow cameraFollow = (CameraFollow)target;
        
        DrawDefaultInspector();
        
        GUILayout.Space(10);
        
        if (cameraFollow.target != null)
        {
            EditorGUILayout.HelpBox("✅ Target đã được gán: " + cameraFollow.target.name, MessageType.Info);
        }
        else
        {
            EditorGUILayout.HelpBox("❌ Target chưa được gán!", MessageType.Warning);
        }
        
        if (GUILayout.Button("Tự động tìm Player"))
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj == null)
            {
                playerObj = GameObject.Find("Barry");
            }
            
            if (playerObj != null)
            {
                cameraFollow.target = playerObj.transform;
                EditorUtility.SetDirty(cameraFollow);
                Debug.Log("✅ Đã gán Target: " + playerObj.name);
            }
            else
            {
                Debug.LogWarning("❌ Không tìm thấy Player!");
            }
        }
    }
}
