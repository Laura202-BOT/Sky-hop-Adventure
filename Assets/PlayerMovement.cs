using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;
    private Rigidbody rb;
    private bool isGrounded;
    private bool canDoubleJump;
    private bool magnetActive;
    private bool hasShield;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        MovePlayer();
        HandleJumping();
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal") * speed;
        float moveZ = Input.GetAxis("Vertical") * speed;
        Vector3 move = new Vector3(moveX, 0, moveZ);
        rb.MovePosition(transform.position + move * Time.deltaTime);
    }

    void HandleJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }
            else if (canDoubleJump)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                canDoubleJump = false;
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            canDoubleJump = false;
        }
    }

    public IEnumerator SpeedBoost()
    {
        speed *= 2;
        yield return new WaitForSeconds(5);
        speed /= 2;
    }

    public void EnableDoubleJump()
    {
        canDoubleJump = true;
    }

    public void EnableMagnet()
    {
        StartCoroutine(MagnetEffect());
    }

    private IEnumerator MagnetEffect()
    {
        magnetActive = true;
        yield return new WaitForSeconds(5);
        magnetActive = false;
    }

    public void EnableShield()
    {
        StartCoroutine(ShieldEffect());
    }

    private IEnumerator ShieldEffect()
    {
        hasShield = true;
        yield return new WaitForSeconds(5);
        hasShield = false;
    }
}

