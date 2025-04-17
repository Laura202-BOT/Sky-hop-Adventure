using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    public GameObject player;
    public GameObject EnemyOrb;

    void Start()
    {
        InvokeRepeating("SpawnEnemy", 0f, Random.Range(1.0f,5.0f));
    }

    void SpawnEnemy()
    {
        Vector3 pos = player.transform.position;
        float randX = Random.Range(-3, 3);
        float randZ = Random.Range(-2, 2);
        Instantiate(EnemyOrb, new Vector3(pos.x + randX, pos.y + 8, pos.z + randZ), Quaternion.identity);
    }
}
