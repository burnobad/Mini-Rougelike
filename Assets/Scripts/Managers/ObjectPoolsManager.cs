using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ObjectPoolTypes { None, TestEnemy, DashEnemy, HeavyEnemy, UpgragePickUp, PlayerBullet}
public class ObjectPoolsManager : MonoBehaviour
{
    private static ObjectPoolsManager instance;
    public static ObjectPoolsManager Instance
    { get { return instance; } }

   [SerializeField]
   private List<ObjectPool> pools;

    private void OnEnable()
    {
        instance = this; 
    }

    public GameObject GetObjFromPool(ObjectPoolTypes _poolType)
    {
        return FindPool(_poolType).GetPoolObject();
    }

    public ObjectPool FindPool(ObjectPoolTypes _poolType)
    {
        ObjectPool pool = null;

        foreach (ObjectPool objPool in pools) 
        {
            if(objPool.Type == _poolType)
            {
                pool = objPool;
                break;
            }
        }

        return pool;
    }


}
