using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal") * speed;
        float moveZ = Input.GetAxis("Vertical") * speed;

        Vector3 move = new Vector3(moveX, rb.velocity.y, moveZ);
        rb.velocity = move;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
