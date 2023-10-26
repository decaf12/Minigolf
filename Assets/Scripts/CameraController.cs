using UnityEngine;

public class CameraController : MonoBehaviour
{
    public BallController ball;
    private Vector3 offset;

    void Awake()
    {
        offset = transform.position - ball.transform.position;
    }

    void LateUpdate()
    {
        transform.position = ball.transform.position + offset;
    }
}