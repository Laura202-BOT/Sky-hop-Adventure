using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public GameObject prefab;
    public Vector3 Offset;
    Vector3 SpawnPosition = new Vector3(-6.0f, 3.0f, 9.0f);

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnPlatform", 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnPlatform()
    {
        Instantiate(prefab, SpawnPosition, Quaternion.identity);
        Offset = new Vector3(Random.Range(-8.0f, 8.0f), Random.Range(1.0f, 6.0f), 4.5f);
        SpawnPosition += Offset;
    }
}
