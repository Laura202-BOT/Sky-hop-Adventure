using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Needed to restart scene
using UnityEngine.UI; // Needed for UI

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed;
    public float runSpeed;
    public float jumpForce;
    private float currentSpeed;

    private Rigidbody rb;
    private bool isGrounded;
    private bool canDoubleJump;
    private bool magnetActive;
    private bool hasShield;
    private Animator animator;

    // 🩸 Health System
    public int maxHealth = 3;
    private int currentHealth;

    public Text healthText;
    public ScreenShake screenShake;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = walkSpeed;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        if (healthText != null)
            UpdateHealthUI();
    }

    void Update()
    {
        Debug.Log("UPDATE IS RUNNING");
        UpdateSpeed();
        MovePlayer();
        HandleJumping();
    }

    void UpdateSpeed()
    {
        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
        float movementSpeed = new Vector2(inputX, inputZ).magnitude;

        animator.SetFloat("Speed", movementSpeed);
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

        animator.SetTrigger("die");

        if (currentHealth <= 0)
        {
            StartCoroutine(RestartSceneAfterDelay());
        }
    }

    private void UpdateHealthUI()
    {
        healthText.text = "Lives: " + currentHealth.ToString();
    }

    private IEnumerator RestartSceneAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool HasShield()
    {
        return hasShield;
    }
}

