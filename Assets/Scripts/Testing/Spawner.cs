using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawnedGameObject;
    public float spawnTime;
    private float currentSpawnTimer;

    private void Awake()
    {
        currentSpawnTimer = spawnTime;
    }


    // Update is called once per frame
    void Update()
    {
        currentSpawnTimer -= Time.deltaTime;
        if (currentSpawnTimer <= 0)
        {
            Instantiate(spawnedGameObject, transform);
            spawnedGameObject.transform.parent = null;
            currentSpawnTimer = spawnTime;
        }
    }
}
