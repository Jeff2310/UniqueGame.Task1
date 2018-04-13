using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private int _freeObjectCount = 0;
    private string _prefabName;
    //private GameObject _prefab;
    
    public Transform CachedTransform;
    
    private Stack<GameObject> _objectStack;
    public Stack<GameObject> ObjectStack
    {
        get { return _objectStack; }
    }

    private Action<GameObject> _reset;
    
    //public ObjectPool(string prefabName, Action<GameObject> reset, int preAllocObjectCount = 0)
    public ObjectPool(Action<GameObject> reset)
    {
        _objectStack = new Stack<GameObject>();
        //_prefabName = prefabName;
        //_prefab = Resources.Load(prefabName) as GameObject;
        
        _reset = reset;
        
        /*
        for (int i = 0; i < preAllocObjectCount; i++)
        {
            _objectStack.Push(CreateObject());
        }

        _freeObjectCount = preAllocObjectCount;
        */
    }
    
    public GameObject GetObject(GameObject prefab)
    {
        GameObject go;
        if (_freeObjectCount > 0)
        {
            go = _objectStack.Pop();
            _freeObjectCount--;
        }
        else
        {
            go = CreateObject(prefab);
        }
        go.SetActive(true);
        return go;
    }

    public void StoreObject(GameObject gameObject)
    {
        _reset(gameObject);
        _objectStack.Push(gameObject);
        _freeObjectCount++;
    }

    private GameObject CreateObject(GameObject prefab)
    {
        GameObject go = UnityEngine.Object.Instantiate(prefab);
        go.SetActive(false);
        go.transform.SetParent(CachedTransform);
        return go;
    }
}