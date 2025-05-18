using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private Vector3 targetPosition;
    private float moveSpeed = 3.5f;

    private void Start()
    {
        targetPosition = transform.position;
    }
    
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
    }

    public void MoveUp()
    {
        targetPosition += Vector3.up;
        ClampPosition();
    }

    public void MoveDown()
    {
        targetPosition += Vector3.down;
        ClampPosition();
    }

    public void MoveLeft()
    {
        targetPosition += Vector3.left;
        ClampPosition();
    }

    public void MoveRight()
    {
        targetPosition += Vector3.right;
        ClampPosition();
    }

    private void ClampPosition()
    {
        // Ограничение по оси X и Y 
        targetPosition.x = Mathf.Clamp(targetPosition.x, -6.25f, 6.25f); // Границы по X
        targetPosition.y = Mathf.Clamp(targetPosition.y, 0.25f, 4f); // Границы по Y
    }
}