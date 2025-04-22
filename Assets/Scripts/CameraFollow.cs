using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the Player
    public Vector3 offset = new Vector3(0, 3, -5); // Default offset

    void Start()
    {
        if (player != null)
        {
            offset = transform.position - player.position;
        }
    }

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }
}
