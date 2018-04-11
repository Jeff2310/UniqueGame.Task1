using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
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

        protected string _prefabName;
        protected Action<GameObject> _attachScript;

        public DelegateHelper.GetController getObjectController;
        public DelegateHelper.SetController setObjectController;
        
        protected GameObjectPool(string prefabName, DelegateHelper.GetController getController, DelegateHelper.SetController setController, int capacity = 0)
        {
            _objectStack = new Stack<GameObject>(_maximumObjectCount);
            _maximumObjectCount = capacity;
            _prefabName = prefabName;
            getObjectController = getController;
            setObjectController = setController;
            
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
        public PlanePool(string prefabName, DelegateHelper.GetController getController, DelegateHelper.SetController setController, int capacity = 0) : base(prefabName, getController, setController, capacity) {}
        protected override GameObject CreateObject()
        {
            return Factories.PlaneFactory.SpawnPlane(_prefabName, (GameObject go) =>{
                setObjectController(go);
                (getObjectController(go) as EnemyPlane).GameObject = go;
            });
        }

        protected override void ResetObject(GameObject gameObject)
        {
            var controller = getObjectController(gameObject);
            if (controller is IResetable)
            {
                (controller as IResetable).Reset();
            }
        }
    }

    public class ProjectilePool : GameObjectPool
    {
        public ProjectilePool(string prefabName, DelegateHelper.GetController getController, DelegateHelper.SetController setController, int capacity = 0) : base(prefabName, getController, setController, capacity) {}

        protected override GameObject CreateObject()
        {
            return Factories.ProjectileFactory.SpawnProjectile(_prefabName, (GameObject go) =>
            {
                setObjectController(go);
                (getObjectController(go) as ProjectileController).GameObject = go;
            });
        }

        protected override void ResetObject(GameObject gameObject)
        {
            gameObject.GetComponent<ProjectileController>().Reset();
        }
    }
}