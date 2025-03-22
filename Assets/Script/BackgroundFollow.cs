using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    [SerializeField] private GameObject player;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = new Vector3 (player.transform.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, 2f);
    }
}
