using UnityEngine;

public class DebugPlayerPosition : MonoBehaviour
{
    public Transform player;
    public bool autoFindPlayer = true;
    public bool showDebugInfo = true;
    
    private Vector3 lastPosition;
    private float timer = 0f;
    
    void Start()
    {
        if (autoFindPlayer && player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj == null)
            {
                playerObj = GameObject.Find("Barry");
            }
            
            if (playerObj != null)
            {
                player = playerObj.transform;
                Debug.Log("DebugPlayerPosition: Tìm thấy player: " + playerObj.name);
            }
            else
            {
                Debug.LogWarning("DebugPlayerPosition: Không tìm thấy player!");
            }
        }
        
        if (player != null)
        {
            lastPosition = player.position;
        }
    }
    
    void Update()
    {
        if (player == null) return;
        
        timer += Time.deltaTime;
        
        // Log vị trí player mỗi 2 giây
        if (timer >= 2f)
        {
            timer = 0f;
            
            Vector3 currentPosition = player.position;
            Vector3 movement = currentPosition - lastPosition;
            
            if (showDebugInfo)
            {
                Debug.Log($"Player Position: {currentPosition:F2}");
                Debug.Log($"Movement: {movement:F2}");
                Debug.Log($"Camera Position: {transform.position:F2}");
                Debug.Log("---");
            }
            
            lastPosition = currentPosition;
        }
    }
    
    void OnGUI()
    {
        if (!showDebugInfo || player == null) return;
        
        GUILayout.BeginArea(new Rect(10, 10, 300, 150));
        GUILayout.Box("Debug Info");
        
        GUILayout.Label($"Player: {player.name}");
        GUILayout.Label($"Player Pos: {player.position:F2}");
        GUILayout.Label($"Camera Pos: {transform.position:F2}");
        
        Vector3 distance = transform.position - player.position;
        GUILayout.Label($"Distance: {distance:F2}");
        
        GUILayout.EndArea();
    }
}
