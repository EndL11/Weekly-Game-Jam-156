using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] prefabs;        //  noodles prefabs
    public Transform[] spawnPoints;     //  spawn points
    private float spawnDelay = .5f;     //  delay between spawns on single wave
    private float spawnWavesDelay = .3f;   //  delay between noodle waves
    private int waves = 30;              //  waves of spawns
    private int maxPointsSpawn = 3;      //  max of spawn points of single wave

    private void Start()
    {
        StartCoroutine(GenerateNoodles(spawnWavesDelay));
    }

    private IEnumerator GenerateNoodles(float spawnWaveDelay)
    {
        for(int i = 0; i < waves; i++)
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
                GenerateRandomNoodle(randomPoint);
                yield return new WaitForSeconds(spawnDelay);  //  spawning single noodle with delay

            }
            
            yield return new WaitForSeconds(spawnWaveDelay);    
        }
    }

    private void GenerateRandomNoodle(int spawnPointIndex)
    {
        //  generating random noodle on certain point
        int randomNoodleIndex = GetRandomNumber(prefabs.Length);
        Instantiate(prefabs[randomNoodleIndex], spawnPoints[spawnPointIndex].position, Quaternion.identity);
    }

    private int GetRandomNumber(int max, int min = 0)
    {
        return Random.Range(0, max);
    }
}
