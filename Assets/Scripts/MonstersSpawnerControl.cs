using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstersSpawnerControl : MonoBehaviour {
    public static MonstersSpawnerControl Instance { get; private set; }

    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject[] monsters;

    int randomSpawnPoint, randomMonster;
    public static bool spawnAllowed;
    
    public static int monsterCount = 0;

    private void Awake() {
        spawnAllowed = true;

    }

    private void Start() {
        InvokeRepeating("SpawnAMonster", 1f, 1f);
    }

    void SpawnAMonster() {
        if(spawnAllowed && monsterCount < 45) {
            randomSpawnPoint = Random.Range(0, spawnPoints.Length);
            randomMonster = Random.Range(0, monsters.Length);
            Instantiate(monsters[randomMonster], spawnPoints[randomSpawnPoint].position, Quaternion.identity);
            monsterCount++;
        }
    }
}
