using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 2.0f;
    public Transform target;

    private void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, (target.position.y - 0.46f), -8);
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }
}
