using System;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEditorInternal;
using UnityEngine;

public class ObjectPool
{
    public abstract class GameObjectPool
    {
        private int _maximumObjectCount = 0;
        private int _freeObjectCount = 0;
        private Stack<GameObject> _objectStack;
        public Stack<GameObject> ObjectStack
        {
            get { return _objectStack; }
        }
        
        protected GameObjectPool(int capacity = 0)
        {
            _objectStack = new Stack<GameObject>(_maximumObjectCount);
            _maximumObjectCount = capacity;
            for (int i = 0; i < capacity; i++)
            {
                GameObject createdObject = CreateObject();
                createdObject.SetActive(false);
                _objectStack.Push(createdObject);
            }

            _freeObjectCount = capacity;
        }
        
        public GameObject GetObject()
        {
            if (_freeObjectCount > 0)
            {
                GameObject returnObject = _objectStack.Pop();
                returnObject.SetActive(true);
                _freeObjectCount--;
                return returnObject;
            }
            else
            {
                return null;
            }
        }

        public void StoreObject(GameObject gameObject)
        {
            ResetObject(gameObject);
            _objectStack.Push(gameObject);
            _freeObjectCount++;
        }
        
        protected abstract GameObject CreateObject();
        protected abstract void ResetObject(GameObject gameObject);
    }
    
    public class PlanePool : GameObjectPool
    {
        public PlanePool(int capacity = 0) : base(capacity) {}
        protected override GameObject CreateObject()
        {
            return Factories.PlaneFactory.SpawnPlane();
        }

        protected override void ResetObject(GameObject gameObject)
        {
            gameObject.GetComponent<PlaneController>().Reset();
        }
    }

    public class ProjectilePool : GameObjectPool
    {
        public ProjectilePool(int capacity = 0) : base (capacity){}

        protected override GameObject CreateObject()
        {
            return Factories.ProjectileFactory.SpawnProjectile();
        }

        protected override void ResetObject(GameObject gameObject)
        {
            gameObject.GetComponent<ProjectileController>().Reset();
        }
    }
}