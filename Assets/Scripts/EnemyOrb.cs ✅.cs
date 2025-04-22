using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOrb : MonoBehaviour
{
    public GameObject particles;
    public ParticleSystem deathEffect; // (Optional) particle effect when orb hits player
    public PlayerMovement player;

    void start()
    {
        particles = GameObject.Find("/Player/ParticleSystem");
        deathEffect = particles.GetComponent<ParticleSystem>();
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            if (player != null)
            {
                if (player.HasShield())
                {
                    Debug.Log("Shield active! Enemy orb destroyed.");
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("Enemy orb hit the player!");

                    // Optional: play effect on player before destruction
                    if (player.currentHealth<=0)
                        particles.SetActive(true);
                        deathEffect.Play();

                    player.TakeDamage(); // Reduce player health + animation
                    Destroy(gameObject);
                }
            }
        }
    }
}

