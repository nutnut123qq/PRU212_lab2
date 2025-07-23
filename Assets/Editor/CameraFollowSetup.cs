using UnityEngine;
using UnityEditor;

public class CameraFollowSetup : EditorWindow
{
    [MenuItem("Tools/Setup Camera Follow")]
    public static void ShowWindow()
    {
        GetWindow<CameraFollowSetup>("Camera Follow Setup");
    }
    
    private void OnGUI()
    {
        GUILayout.Label("Camera Follow Setup", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        GUILayout.Label("Chọn loại camera follow bạn muốn:");
        GUILayout.Space(5);
        
        if (GUILayout.Button("Simple Camera Follow (Đơn giản)", GUILayout.Height(30)))
        {
            SetupSimpleCameraFollow();
        }
        
        GUILayout.Space(5);
        
        if (GUILayout.Button("Advanced Camera Follow (Nâng cao)", GUILayout.Height(30)))
        {
            SetupAdvancedCameraFollow();
        }
        
        GUILayout.Space(20);
        
        GUILayout.Label("Hướng dẫn:", EditorStyles.boldLabel);
        GUILayout.Label("1. Simple: Camera theo player đơn giản");
        GUILayout.Label("2. Advanced: Có look-ahead, boundaries, smooth");
        GUILayout.Label("3. Script sẽ tự động tìm player trong scene");
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("Remove Camera Follow"))
        {
            RemoveCameraFollow();
        }
    }
    
    private void SetupSimpleCameraFollow()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Không tìm thấy Main Camera!");
            return;
        }
        
        // Remove existing camera follow scripts
        RemoveCameraFollow();
        
        // Add SimpleCameraFollow component
        SimpleCameraFollow cameraFollow = mainCamera.gameObject.AddComponent<SimpleCameraFollow>();
        
        // Try to find player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            player = GameObject.Find("Barry");
        }
        
        if (player != null)
        {
            cameraFollow.player = player.transform;
            Debug.Log("✅ Đã setup Simple Camera Follow cho: " + player.name);
        }
        else
        {
            Debug.LogWarning("⚠️ Không tìm thấy player. Hãy gán thủ công trong Inspector.");
        }
        
        // Set default values for 2D game
        cameraFollow.offset = new Vector3(3, 1, -10); // Offset phù hợp cho game 2D
        cameraFollow.smoothSpeed = 0.125f;
        
        EditorUtility.SetDirty(mainCamera.gameObject);
    }
    
    private void SetupAdvancedCameraFollow()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Không tìm thấy Main Camera!");
            return;
        }
        
        // Remove existing camera follow scripts
        RemoveCameraFollow();
        
        // Add CameraFollow component
        CameraFollow cameraFollow = mainCamera.gameObject.AddComponent<CameraFollow>();
        
        // Try to find player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            player = GameObject.Find("Barry");
        }
        
        if (player != null)
        {
            cameraFollow.target = player.transform;
            Debug.Log("✅ Đã setup Advanced Camera Follow cho: " + player.name);
        }
        else
        {
            Debug.LogWarning("⚠️ Không tìm thấy player. Hãy gán thủ công trong Inspector.");
        }
        
        // Set default values for 2D skiing game
        cameraFollow.offset = new Vector3(3, 1, -10);
        cameraFollow.smoothSpeed = 0.125f;
        cameraFollow.useLookAhead = true;
        cameraFollow.lookAheadDistance = 2f;
        cameraFollow.lookAheadSpeed = 2f;
        
        EditorUtility.SetDirty(mainCamera.gameObject);
    }
    
    private void RemoveCameraFollow()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;
        
        // Remove existing camera follow components
        SimpleCameraFollow simpleCameraFollow = mainCamera.GetComponent<SimpleCameraFollow>();
        if (simpleCameraFollow != null)
        {
            DestroyImmediate(simpleCameraFollow);
            Debug.Log("Đã xóa SimpleCameraFollow");
        }
        
        CameraFollow cameraFollow = mainCamera.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            DestroyImmediate(cameraFollow);
            Debug.Log("Đã xóa CameraFollow");
        }
    }
}
