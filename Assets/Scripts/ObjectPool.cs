using System;
using System.Collections.Generic;
using System.Security.Principal;
using DefaultNamespace;
using UnityEditorInternal;
using UnityEngine;

public class PlanePool
{
    private static PlanePool _instance = null;
    private static bool _binit = false;
    private int _maximumObjectCount = 0;
    private int _freeObjectCount = 0;
    private Queue<GameObject> _objectQueue;

    private PlanePool(int maximumObjectCount)
    {
        _objectQueue = new Queue<GameObject>(_maximumObjectCount);
        _maximumObjectCount = maximumObjectCount;
        for (int i = 0; i < maximumObjectCount; i++)
        {
            GameObject planeObject = Factories.PlaneFactory.SpawnPlane();
            planeObject.SetActive(false);
            _objectQueue.Enqueue(planeObject);
        }
        _freeObjectCount = maximumObjectCount;
    }
    
    public static PlanePool GetInstance()
    {
        if (!_binit)
        {
            _instance = new PlanePool(50);
            _binit = true;
        }

        return _instance;
    }

    public GameObject GetObject()
    {
        if (_freeObjectCount > 0)
        {
            GameObject returnObject = _objectQueue.Dequeue();
            returnObject.SetActive(true);
            _freeObjectCount--;
            return returnObject;
        }
        else
        {
            GameObject returnObject = Factories.PlaneFactory.SpawnPlane();
            returnObject.SetActive(true);
            return returnObject;
        }
    }

    public void StoreObject(GameObject plane)
    {
        plane.GetComponent<PlaneController>().Reset();
        _objectQueue.Enqueue(plane);
        _freeObjectCount++;
    }
}