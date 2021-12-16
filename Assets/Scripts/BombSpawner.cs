using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BombSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isSpawning = false;
    private GameObject bombPrefab;
    void Start()
    {
        bombPrefab = (GameObject)Resources.Load("Prefabs/Bomb", typeof(GameObject));
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSpawning)
        {
            StartCoroutine(SpawnBombs());
        }
        
    }

    IEnumerator SpawnBombs()
    {
        isSpawning = true;
        yield return new WaitForSeconds(Random.value * 2);
        float randomZ = Random.value * 80 - 10;
        float randomX = Random.value * (randomZ + 10 - 40);
        float randomY = randomZ * 0.9F;
        Vector3 spawnPos = new Vector3(randomX, randomY, randomZ);
        GameObject bomb = Instantiate(bombPrefab, transform.position + spawnPos, Quaternion.identity);
        Bomb bombScript = bomb.GetComponent<Bomb>();
        bombScript.customTime = Random.value * 8;
        bombScript.force = Random.value * 60;
        bombScript.canAffectOthers = false;
        isSpawning = false;
    }
}
