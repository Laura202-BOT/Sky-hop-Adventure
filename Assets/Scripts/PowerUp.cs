using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpEffect : MonoBehaviour
{
    public enum PowerUpType { SpeedBoost, DoubleJump, Magnet, Shield }
    public PowerUpType powerUp;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();

            if (player != null)
            {
                switch (powerUp)
                {
                    case PowerUpType.SpeedBoost:
                        player.StartCoroutine(player.SpeedBoost());
                        break;
                    case PowerUpType.DoubleJump:
                        player.EnableDoubleJump();
                        break;
                    case PowerUpType.Magnet:
                        player.EnableMagnet();
                        break;
                    case PowerUpType.Shield:
                        player.EnableShield();
                        break;
                }
            }

            Destroy(gameObject); // Remove power-up after collection
        }
    }
}
