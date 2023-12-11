using System;
using UnityEngine;

public class TeleportationBorder : MonoBehaviour
{
    public Vector3 teleportDirection;
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with " + other.name);
        
        transform.position = new Vector3(transform.position.x * teleportDirection.x,
            transform.position.y * teleportDirection.y, 0);
    }
}
