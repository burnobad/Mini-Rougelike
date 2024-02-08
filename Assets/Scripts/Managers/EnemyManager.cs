using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    float maxDangerLevel = 1;
    float availabelDangerLevel = 0;

    void Start()
    {
        GameEventsManager.GameStarted += GameStarted;
        GameEventsManager.RestartGame += RestartGame;
        StartCoroutine(Test());
    }

    void Update()
    {
        if(!WorldData.Instance.gameStarted) 
        {
            if(WorldData.Instance.PlayerPos != new Vector3(0,0.8f,0))
            {
                WorldData.Instance.gameStarted = true;
                GameEventsManager.GameStarted();
            }
        }
    }

    IEnumerator Test()
    {
        while (true) 
        {
            if (WorldData.Instance.gameStarted) 
            {
                SpawnEnemy();

                yield return new WaitForSeconds(5f);
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }

    void SpawnEnemy()
    {
        GameObject pulledEnemy = null;
        
        maxDangerLevel++;
        availabelDangerLevel = maxDangerLevel;

        while(availabelDangerLevel > 1)
        {
            int randomPool = Random.Range(1, 3);

            if(randomPool == 1)
            {
                 pulledEnemy
                = ObjectPoolsManager.Instance.GetObjFromPool(ObjectPoolTypes.TestEnemy);
            }
            else if(randomPool == 2) 
            {
                 pulledEnemy
                = ObjectPoolsManager.Instance.GetObjFromPool(ObjectPoolTypes.DashEnemy);
            }
            else if(randomPool == 3)
            {
                pulledEnemy
               = ObjectPoolsManager.Instance.GetObjFromPool(ObjectPoolTypes.HeavyEnemy);
            }

            EnemyBase enemy = pulledEnemy.GetComponent<EnemyBase>();

            if(enemy.DangerLevel <= availabelDangerLevel)
            {
                availabelDangerLevel -= enemy.DangerLevel;
                float x = Random.Range(-11, 11);
                float y = Random.Range(-11, 11);

                enemy.transform.position = new Vector2(x, y);
                enemy.gameObject.SetActive(true);
            }
        }

    }

    void GameStarted()
    {
        WorldData.Instance.gameStarted = true;
    }
    void RestartGame()
    {
        maxDangerLevel = 1;
        WorldData.Instance.gameStarted = false;
    }
}
