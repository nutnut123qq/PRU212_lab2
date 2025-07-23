using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; // Player transform
    
    [Header("Follow Settings")]
    public float smoothSpeed = 0.125f; // Tốc độ smooth camera
    public Vector3 offset = new Vector3(0, 0, -10); // Offset từ player
    
    [Header("Boundary Settings")]
    public bool useBoundaries = false;
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -5f;
    public float maxY = 5f;
    
    [Header("Look Ahead Settings")]
    public bool useLookAhead = true;
    public float lookAheadDistance = 2f;
    public float lookAheadSpeed = 2f;
    
    private Vector3 velocity = Vector3.zero;
    private Vector3 currentLookAhead = Vector3.zero;
    private Rigidbody2D targetRigidbody;
    
    void Start()
    {
        // Tự động tìm player nếu chưa được gán
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
                targetRigidbody = player.GetComponent<Rigidbody2D>();
            }
            else
            {
                Debug.LogWarning("Không tìm thấy Player! Hãy gán target hoặc đặt tag 'Player' cho player object.");
            }
        }
        else
        {
            targetRigidbody = target.GetComponent<Rigidbody2D>();
        }
    }
    
    void LateUpdate()
    {
        if (target == null) return;
        
        // Tính toán vị trí mong muốn
        Vector3 desiredPosition = target.position + offset;
        
        // Thêm look ahead nếu được bật
        if (useLookAhead && targetRigidbody != null)
        {
            Vector3 targetVelocity = targetRigidbody.linearVelocity;
            Vector3 lookAheadTarget = targetVelocity.normalized * lookAheadDistance;
            
            // Smooth look ahead
            currentLookAhead = Vector3.Lerp(currentLookAhead, lookAheadTarget, lookAheadSpeed * Time.deltaTime);
            desiredPosition += currentLookAhead;
        }
        
        // Áp dụng boundaries nếu được bật
        if (useBoundaries)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
        }
        
        // Smooth camera movement
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        transform.position = smoothedPosition;
    }
    
    // Hàm để set target từ script khác
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        if (target != null)
        {
            targetRigidbody = target.GetComponent<Rigidbody2D>();
        }
    }
    
    // Hàm để set boundaries
    public void SetBoundaries(float minX, float maxX, float minY, float maxY)
    {
        this.minX = minX;
        this.maxX = maxX;
        this.minY = minY;
        this.maxY = maxY;
        useBoundaries = true;
    }
    
    // Vẽ gizmos để hiển thị boundaries trong Scene view
    void OnDrawGizmosSelected()
    {
        if (useBoundaries)
        {
            Gizmos.color = Color.yellow;
            Vector3 center = new Vector3((minX + maxX) / 2, (minY + maxY) / 2, transform.position.z);
            Vector3 size = new Vector3(maxX - minX, maxY - minY, 0);
            Gizmos.DrawWireCube(center, size);
        }
        
        if (target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.position + offset);
        }
    }
}
