using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ObjectPoolManager{
    private Dictionary<string, ObjectPool> _poolDict;
    
    public ObjectPoolManager()
    {
        _poolDict = new Dictionary<string, ObjectPool>();
    }
    
    private static ObjectPoolManager Instance;
    public static ObjectPoolManager GetInstance()
    {
        if (Instance == null)
        {
            Instance = new ObjectPoolManager();
        }

        return Instance;
    }

    //public void CreatePool(string prefabName, Action<GameObject> reset, string poolName=null)
    public void CreatePool(string poolName, Action<GameObject> reset)
    {
        if (poolName != null && _poolDict.ContainsKey(poolName) == true)
        {
            return;
        }

        string name;
        if (poolName == null)
        {
            //name = prefabName;
            name = poolName;
        }
        else
        {
            name = poolName;
        }
        //ObjectPool pool = new ObjectPool(prefabName, reset);
        ObjectPool pool = new ObjectPool(reset);
        GameObject poolVirtualObject = new GameObject(name);
        pool.CachedTransform = poolVirtualObject.transform;
        _poolDict.Add(name, pool);
    }

    public ObjectPool GetPool(string poolName)
    {
        return _poolDict[poolName];
    }

    //public GameObject GetObject(string poolName)
    public GameObject GetObject(string poolName, GameObject prefab)
    {
        return _poolDict[poolName].GetObject(prefab);
    }

    public void StoreObject(string poolName, GameObject go)
    {
        _poolDict[poolName].StoreObject(go);
    }
}
