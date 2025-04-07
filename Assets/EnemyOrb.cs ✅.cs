using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOrb : MonoBehaviour
{
    public ParticleSystem deathEffect; // (Optional) particle effect when orb hits player

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();

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
                    if (deathEffect != null)
                        deathEffect.Play();

                    player.TakeDamage(); // Reduce player health + animation
                    Destroy(gameObject);
                }
            }
        }
    }
}

