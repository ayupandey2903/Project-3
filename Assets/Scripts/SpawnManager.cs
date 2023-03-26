using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // public variables
    [Header("Obstacle Settings")]
    [Tooltip("Array of obstacles prefabs")] public GameObject[] obstaclePrefabs;                // array of obstacle prefabs
    // [Tooltip("The prefab to spawn")] public GameObject obstaclePrefab;

    [Header("Spawn Settings")]
    [Tooltip("Spawn position")] public Vector3 spawnPos = new(25, 0, 0);
    [Tooltip("Delay before starting spawn")] public float startDelay = 2f;
    [Tooltip("Spawn delay")] public float spawnDelay = 2f;

    // private variables
    private PlayerController playerControllerScript;

    // Start is called before the first frame update
    void Start()
    {
        // get player controller script
        playerControllerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        spawnDelay = Random.Range(spawnDelay, spawnDelay + 2.0f);
        InvokeRepeating(nameof(SpawnObstacle), startDelay, spawnDelay);
    }


    void SpawnObstacle()
    {
        int obstacleIndex;
        if (playerControllerScript.gameOver == false) 
        {
            obstacleIndex = Random.Range(0, obstaclePrefabs.Length);                                                    // get random index for obstacle
            Instantiate(obstaclePrefabs[obstacleIndex], spawnPos, obstaclePrefabs[obstacleIndex].transform.rotation);   // spawn a random obstacle
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
