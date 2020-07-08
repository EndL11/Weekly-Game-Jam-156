using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSpawnChance
{
    [Range(0, 100)]
    public float chance;
    public GameObject prefab;
    public bool done = false;

    public float Done(float divideValue)
    {
        done = true;
        float oldChance = chance;
        chance /= divideValue;        
        return (oldChance - chance);
    }
}

public class Spawner : MonoBehaviour
{
    [Range(0, 100)]
    public float noodleChance;
    public ItemSpawnChance[] trashPrefabs;       //  trash prefabs
    public ItemSpawnChance[] noodlePrefabs;      //  noodles prefabs
    public Transform[] spawnPoints;         //  spawn points
    private float spawnDelay = .3f;         //  delay between spawns on single wave
    private float spawnWavesDelay = .2f;    //  delay between noodle waves
    private int maxPointsSpawn = 4;         //  max of spawn points of single wave

    private void Start()
    {
        noodleChance /= 100f;
        StartCoroutine(GenerateNoodles(spawnWavesDelay));
    }

    private IEnumerator GenerateNoodles(float spawnWaveDelay)
    {
        while(true)
        {
            //List<int> spawnedPoints = new List<int>();
            for (int j = 0; j < maxPointsSpawn; j++)
            {
                int randomPoint = GetRandomNumber(spawnPoints.Length);
                float itemCategoryChance = Random.value;
                if (itemCategoryChance <= noodleChance)
                {
                    GenerateRandomPrefab(randomPoint, noodlePrefabs);
                }
                else
                {
                    GenerateRandomPrefab(randomPoint, trashPrefabs);
                }
                //while(spawnedPoints.Contains(randomPoint))
                //{
                //    randomPoint = GetRandomNumber(spawnPoints.Length);    //  for spawning by rows
                //}
                //spawnedPoints.Add(randomPoint);
                yield return new WaitForSeconds(spawnDelay);  //  spawning single noodle with delay
            }            
            yield return new WaitForSeconds(spawnWaveDelay);    
        }
    }

    private void GenerateRandomPrefab(int spawnPointIndex, ItemSpawnChance[] prefabs)
    {
        //  generating random prefab on certain point
        int randomNoodleIndex = 0;
        float chance = prefabs[0].chance / 100f;
        float randomChance = Random.value;
        for (int i = 0; i < prefabs.Length; i++)
        {
            if(randomChance <= chance)
            {
                randomNoodleIndex = i;
                break;
            }
            if(i == prefabs.Length - 1)
            {
                randomNoodleIndex = i;
                break;
            }
            chance += (prefabs[i + 1].chance / 100f);
        }
        Instantiate(prefabs[randomNoodleIndex].prefab, spawnPoints[spawnPointIndex].position, Quaternion.identity);
    }

    private int GetRandomNumber(int max, int min = 0)
    {
        return Random.Range(0, max);
    }

    public void DivideDelay(float divideValue)
    {
        spawnWavesDelay /= divideValue;
        spawnDelay /= divideValue;
    }

    public ItemSpawnChance FindNoodleByType(NoodleTypes.types type)
    {
        foreach (var item in noodlePrefabs)
        {
            if(item.prefab.GetComponent<Noodle>().noodleType == type)
            {
                return item;
            }
        }
        return null;
    }

    public void AddChanceToUnDoneNoodles(float value)
    {
        List<ItemSpawnChance> unDoneItems = new List<ItemSpawnChance>();
        foreach (var item in noodlePrefabs)
        {
            if(item.done == false)
            {
                unDoneItems.Add(item);
            }
        }

        float addValue = value / unDoneItems.Count;
        foreach (var item in unDoneItems)
        {
            item.chance += addValue;
        }
    }
    
    public void IncreaseSpawnpointsValue()
    {
        if(maxPointsSpawn != spawnPoints.Length - 1)
        {
            maxPointsSpawn++;
        }
    }
}
