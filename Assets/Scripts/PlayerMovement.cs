﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Needed to restart scene
using UnityEngine.UI; // Needed for UI
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed;
    public float runSpeed;
    public float jumpForce;
    public AudioSource deathSound;
    private float currentSpeed;


    private Rigidbody rb;
    private bool isGrounded;
    private bool canDoubleJump;
    private bool magnetActive;
    private bool hasShield;
    private Animator animator;

    // 🩸 Health System
    public int maxHealth = 3;
    public int currentHealth;

    public GameOverUI gameOverUI;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI scoreText;
    public ScreenShake screenShake;

    int score;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = walkSpeed;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        score = 0;

        if (healthText != null)
            UpdateHealthUI();
    }

    void Update()
    {
        UpdateSpeed();
        MovePlayer();
        HandleJumping();

        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            animator.SetBool("isWalking", true);
        } else if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            animator.SetBool("isWalking", false);

        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetBool("isRunning", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetBool("isRunning", false);
        }


        

    }

    private void FixedUpdate()
    {
        Vector3 lastPosition = Vector3.zero;
        float speed = Vector3.Distance(transform.position, lastPosition) / Time.deltaTime;
        Debug.Log("Speed: " + speed.ToString());

        if (speed > 30000f)
        {
            Time.timeScale = 0;
            speed = 0;
            currentHealth = 0;
            UpdateHealthUI();
            animator.SetTrigger("GameOver");
            if (deathSound != null)
                deathSound.Play();
            gameOverUI.ShowGameOver();
        }
        lastPosition = transform.position;
    }

    void UpdateSpeed()
    {
        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
        float movementSpeed = new Vector2(inputX, inputZ).magnitude;
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal") * currentSpeed;
        float moveZ = Input.GetAxis("Vertical") * currentSpeed;
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
                animator.SetBool("isJumping", true);
            }
            else if (canDoubleJump)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                canDoubleJump = false;
                animator.SetBool("isJumping", true);
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            canDoubleJump = false;
            animator.SetBool("isJumping", false);
            if(other.gameObject.layer == 3)
            {
                score++;
                scoreText.text = "Score: " + score.ToString();
            }
        }
    }

    public IEnumerator SpeedBoost()
    {
        currentSpeed *= 2;
        yield return new WaitForSeconds(5);
        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
    }

    public void EnableDoubleJump()
    {
        canDoubleJump = true;
        animator.SetTrigger("collectPowerUp");
    }

    public void EnableMagnet()
    {
        StartCoroutine(MagnetEffect());
        animator.SetTrigger("collectPowerUp");
    }

    private IEnumerator MagnetEffect()
    {
        magnetActive = true;
        GameObject[] orbs = GameObject.FindGameObjectsWithTag("Orb");

        foreach (GameObject orb in orbs)
        {
            StartCoroutine(AttractOrb(orb));
        }

        yield return new WaitForSeconds(5);
        magnetActive = false;
    }

    private IEnumerator AttractOrb(GameObject orb)
    {
        float duration = 1f;
        float elapsed = 0f;
        Vector3 startPosition = orb.transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            orb.transform.position = Vector3.Lerp(startPosition, transform.position, elapsed / duration);
            yield return null;
        }

        Destroy(orb);
    }

    public void EnableShield()
    {
        StartCoroutine(ShieldEffect());
        animator.SetTrigger("collectPowerUp");
    }

    private IEnumerator ShieldEffect()
    {
        hasShield = true;
        yield return new WaitForSeconds(5);
        hasShield = false;
    }

    // ✅ 💥 DAMAGE SYSTEM
    public void TakeDamage()
    {
        if (hasShield)
        {
            Debug.Log("Shield protected player!");

            if (screenShake != null)
            {
                screenShake.Shake(); // Shake even when blocked
            }

            return; // Exit early, no health loss
        }

        currentHealth--;
        Debug.Log("Player took damage! Health left: " + currentHealth);

        if (screenShake != null)
        {
            screenShake.Shake(); // Shake on actual damage too
        }

        if (healthText != null)
            UpdateHealthUI();

        animator.SetTrigger("Damage");

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Take_Damage") && !animator.IsInTransition(0))
        {
            animator.ResetTrigger("Damage");
        }
        

        if (currentHealth <= 0)
        {
            animator.SetTrigger("GameOver");
            if (deathSound != null)
                deathSound.Play();
                gameOverUI.ShowGameOver();

            //StartCoroutine(RestartSceneAfterDelay());
        }
    }

    private void UpdateHealthUI()
    {
        healthText.text = "Lives: " + currentHealth.ToString();
    }

    private IEnumerator RestartSceneAfterDelay()
    {
        yield return new WaitForSeconds(deathSound.clip.length+0.2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool HasShield()
    {
        return hasShield;
    }
}

