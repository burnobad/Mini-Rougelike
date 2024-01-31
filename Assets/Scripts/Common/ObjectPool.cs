using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject objPrefab;

    [SerializeField]
    private int numSpawns = 10;

    private List<GameObject> spawnedObjects;

    [SerializeField]
    private bool canExpand;

    [SerializeField]
    private int numToExpand = 10;

    [SerializeField]
    private ObjectPoolTypes myType;
    public ObjectPoolTypes Type
    { get { return myType; } }

    private void Awake()
    {
        Initialize();
    }
    private void Start()
    {
        GameEventsManager.RestartGame += RestartGame;

    }

    public void RestartGame()
    {
        foreach (GameObject spawnedObj in spawnedObjects)
        {
            if (spawnedObj.activeInHierarchy)
            {
                spawnedObj.SetActive(false);
            }

            Destroy(spawnedObj);
        }
        Initialize();
    }

    private void Initialize()
    {
        spawnedObjects = new List<GameObject>();

        AddObjectsToPool(numSpawns);
    }

    public GameObject GetPoolObject()
    {
        GameObject obj = null;

        foreach (GameObject spawnedObj in spawnedObjects) 
        {
            if(!spawnedObj.activeInHierarchy)
            {
                obj = spawnedObj;
                break;
            }
        }

        if(obj == null && canExpand)
        {
            obj = AddObjectsToPool(numToExpand);
        }
        else if(obj == null)
        {
            Debug.LogWarning(this.name + " can't get pooled obj");
        }

        return obj;
    }

    private GameObject AddObjectsToPool(int _numToAdd)
    {
        GameObject obj = null;

        for (int i = 0; i < _numToAdd; i++)
        {
            obj = InstantiateObj();
            spawnedObjects.Add(obj);
        }
        return obj;
    }

    private GameObject InstantiateObj()
    {
        GameObject obj = null;

        if(objPrefab != null) 
        {
            int objNum = spawnedObjects.Count + 1;

            obj = Instantiate(objPrefab, Vector3.zero, Quaternion.identity, this.transform);
            obj.name = objPrefab.name + objNum.ToString();
            obj.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning(this.name + " doesn't have a prefab");
        }

        return obj;
    }

}
