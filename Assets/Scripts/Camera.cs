using UnityEngine;

public class Camera : MonoBehaviour
{
    private GameObject player;
    private Vector3 offset;
    
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        offset = transform.position - player.transform.position;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x,player.transform.position.y + offset.y, -10);
    }
}
