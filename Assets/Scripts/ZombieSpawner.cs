using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject[] zombieRefs;
    public void SpawnZombies(Vector2 pos, int count = 1, float radius = 10)
    {
        for (int i = 0; i < count; i++)
        {
            AttemptSpawn(Random.insideUnitCircle * radius);
        }
    }

    private bool AttemptSpawn(Vector2 pos)
    {
        GameObject newZombie = Instantiate(zombieRefs[Random.Range(0,zombieRefs.Length)], transform);
        newZombie.transform.position = pos;
        return true;
    }
}
