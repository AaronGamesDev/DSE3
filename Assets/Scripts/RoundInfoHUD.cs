using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundInfoHUD : MonoBehaviour
{
    public Text roundCounter;
    public Text remainingEnemyCounter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        roundCounter.text = "Round: " + EnemySpawner.roundCounter;
        remainingEnemyCounter.text = "Remaining Enemies: " + EnemySpawner.remainingRoundEnemies;
    }
}
