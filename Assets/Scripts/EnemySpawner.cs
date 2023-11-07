using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static int roundCounter = 0;
    public Bounds spawnArea;
    public GameObject enemyPrefab;
    public float spawnMultiplier = 1f;
    public List<GameObject> enemiesList;
    public static int remainingRoundEnemies;
    // Start is called before the first frame update
    void Start()
    {
        spawnArea = GetComponent<BoxCollider>().bounds;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesList.Count == 0)//if there are no enemies increment roundCounter and spawn new enemies
        {
            roundCounter++;
            SpawnEnemies();
        }
        UpdateEnemiesList();
        CalculateRemaining();
    }

    //assigns remainingRoundEnemies to use in UI
    void CalculateRemaining()
    {
        remainingRoundEnemies = enemiesList.Count;
    }

    //Removes null/destroyed enemies from list
    void UpdateEnemiesList()
    {
        for (int i = 0; i < enemiesList.Count; i++)
        {
            if (enemiesList[i] == null)
            {
                enemiesList.RemoveAt(i);
            }
        }
    }

    //Spawns Enemies based on (roundCounter * spawnMultiplier) + 5 and dynamically adds 5 extra enemies on rounds that are multiples of 5.
    //The Amount of extra enemies is increased to 10 from round 21 onwards  
    void SpawnEnemies()
    {
        int totalRoundEnemies = 0;
        totalRoundEnemies = Mathf.RoundToInt(roundCounter * spawnMultiplier);
        totalRoundEnemies += 5;

        if (roundCounter > 0 && roundCounter < 21)//from round 1 to 20
        {
            
            if (roundCounter % 5 == 0)//if a multiple of 5
            {
                totalRoundEnemies += 5;//add extra 5 enemies
            }
            
        }
        else if (roundCounter > 20)//from round 21 and above
        {

            if (roundCounter % 5 == 0)//if a multiple of 5
            {
                totalRoundEnemies += 10;//add extra 10 enemies
                //TODO: Change this to increase enemy dmg values;
            }
        }

        for (int i = 0; i < totalRoundEnemies; i++)
        {
            enemiesList.Add(Instantiate<GameObject>(enemyPrefab, GenerateSpawnPoint(), Quaternion.identity));
        }
    }


    //Generates random spawn point within the bounds of the spawnArea
    Vector3 GenerateSpawnPoint()
    {
        return new Vector3(Random.Range(spawnArea.min.x, spawnArea.max.x), Random.Range(spawnArea.min.y, spawnArea.max.y), Random.Range(spawnArea.min.z, spawnArea.max.z));
    }
}
