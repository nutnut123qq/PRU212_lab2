using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform player; // Kéo thả player vào đây
    
    [Header("Settings")]
    public Vector3 offset = new Vector3(0, 2, -10); // Khoảng cách từ player
    public float smoothSpeed = 0.125f; // Tốc độ smooth (0 = không smooth, 1 = rất smooth)
    
    [Header("Auto Find Player")]
    public bool autoFindPlayer = true; // Tự động tìm player
    
    void Start()
    {
        Debug.Log("SimpleCameraFollow: Bắt đầu tìm player...");

        // Tự động tìm player nếu chưa được gán
        if (autoFindPlayer && player == null)
        {
            Debug.Log("SimpleCameraFollow: Đang tìm player với tag 'Player'...");

            // Tìm theo tag "Player"
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                Debug.Log("✅ SimpleCameraFollow: Đã tìm thấy Player với tag: " + playerObj.name);
            }
            else
            {
                Debug.Log("SimpleCameraFollow: Không tìm thấy object với tag 'Player', thử tìm theo tên 'Barry'...");

                // Tìm theo tên "Barry" (dựa vào prefab trong dự án)
                playerObj = GameObject.Find("Barry");
                if (playerObj != null)
                {
                    player = playerObj.transform;
                    Debug.Log("✅ SimpleCameraFollow: Đã tìm thấy Barry theo tên: " + playerObj.name);
                }
                else
                {
                    Debug.LogError("❌ SimpleCameraFollow: Không tìm thấy Player! Hãy:");
                    Debug.LogError("1. Gán player vào script CameraFollow trong Inspector");
                    Debug.LogError("2. Hoặc đặt tag 'Player' cho player object");
                    Debug.LogError("3. Hoặc đặt tên player là 'Barry'");

                    // List tất cả objects trong scene để debug
                    GameObject[] allObjects = FindObjectsOfType<GameObject>();
                    Debug.Log("Tất cả objects trong scene:");
                    foreach (GameObject obj in allObjects)
                    {
                        Debug.Log("- " + obj.name + " (Tag: " + obj.tag + ")");
                    }
                }
            }
        }
        else if (player != null)
        {
            Debug.Log("✅ SimpleCameraFollow: Player đã được gán sẵn: " + player.name);
        }
    }
    
    void LateUpdate()
    {
        // Kiểm tra xem có player không
        if (player == null)
        {
            // Thử tìm lại player nếu chưa có
            if (autoFindPlayer)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj == null)
                {
                    playerObj = GameObject.Find("Barry");
                }

                if (playerObj != null)
                {
                    player = playerObj.transform;
                    Debug.Log("✅ SimpleCameraFollow: Đã tìm thấy player trong LateUpdate: " + playerObj.name);
                }
            }

            if (player == null) return;
        }

        // Tính vị trí mong muốn
        Vector3 desiredPosition = player.position + offset;

        // Di chuyển camera smooth
        if (smoothSpeed > 0)
        {
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
        else
        {
            // Di chuyển ngay lập tức
            transform.position = desiredPosition;
        }
    }
    
    // Hàm để gán player từ script khác
    public void SetPlayer(Transform newPlayer)
    {
        player = newPlayer;
        Debug.Log("Camera đã được gán player: " + newPlayer.name);
    }
}
