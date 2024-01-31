using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        while (true) 
        {
            SpawnEnemy();

            yield return new WaitForSeconds(2.3f);
        }
    }

    void SpawnEnemy()
    {
        GameObject enemy 
            = ObjectPoolsManager.Instance.GetObjFromPool(ObjectPoolTypes.DashEnemy);

        float x = Random.Range(-11, 11);
        float y = Random.Range(-11, 11);

        enemy.transform.position = new Vector2(x,y);
        enemy.SetActive(true);
    }
}
