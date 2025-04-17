using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform boardTransform;
    public float height = 6f;
    public float angle = 50f;
    public float xPosition = 3.5f;
    public float yPosition = -2f;

    void Start()
    {
        WhitePerspective();
    }

    public void BlackPerspective()
    {
        transform.position = new Vector3(xPosition, height, 9f);
        transform.rotation = Quaternion.Euler(angle, 180f, 0f);
    }

    public void WhitePerspective()
    {
        Vector3 boardCenter = new Vector3(boardTransform.position.x + xPosition,
                                          boardTransform.position.y,
                                          boardTransform.position.z + yPosition);
        transform.position = new Vector3(boardCenter.x, height, boardCenter.z);
        transform.rotation = Quaternion.Euler(angle, 0f, 0f);
    }
}