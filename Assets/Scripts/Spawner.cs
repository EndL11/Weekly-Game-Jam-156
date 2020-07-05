using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] trashPrefabs;       //  trash prefabs
    public GameObject[] noodlePrefabs;      //  noodles prefabs
    public Transform[] spawnPoints;         //  spawn points
    private float spawnDelay = .3f;         //  delay between spawns on single wave
    private float spawnWavesDelay = .2f;    //  delay between noodle waves
    private int maxPointsSpawn = 4;         //  max of spawn points of single wave

    private void Start()
    {
        StartCoroutine(GenerateNoodles(spawnWavesDelay));
    }

    private IEnumerator GenerateNoodles(float spawnWaveDelay)
    {
        while(true)
        {
            //List<int> spawnedPoints = new List<int>();
            for(int j = 0; j < maxPointsSpawn; j++)
            {
                int randomPoint = GetRandomNumber(spawnPoints.Length);
                //while(spawnedPoints.Contains(randomPoint))
                //{
                //    randomPoint = GetRandomNumber(spawnPoints.Length);    //  for spawning by rows
                //}
                //spawnedPoints.Add(randomPoint);
                int rand = Random.Range(0, 2);
                if(rand == 0)
                {
                    GenerateRandomPrefab(randomPoint, noodlePrefabs);
                }
                else
                {
                    GenerateRandomPrefab(randomPoint, trashPrefabs);
                }
                yield return new WaitForSeconds(spawnDelay);  //  spawning single noodle with delay

            }
            
            yield return new WaitForSeconds(spawnWaveDelay);    
        }
    }

    private void GenerateRandomPrefab(int spawnPointIndex, GameObject[] prefabs)
    {
        //  generating random prefab on certain point
        int randomNoodleIndex = GetRandomNumber(prefabs.Length);
        Instantiate(prefabs[randomNoodleIndex], spawnPoints[spawnPointIndex].position, Quaternion.identity);
    }

    private int GetRandomNumber(int max, int min = 0)
    {
        return Random.Range(0, max);
    }
}
