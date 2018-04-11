using System;
using System.IO;
using UnityEngine;
using UnityEngine.Timeline;

public class Factories
{
    public class PlaneFactory : MonoBehaviour
    {
        public static GameObject SpawnPlane(string prefabName, Action<GameObject> attachScript)
        {
            GameObject planeObject = 
                Instantiate(Resources.Load(prefabName), Vector3.zero, Quaternion.AngleAxis(90.0f, Vector3.forward)) as GameObject;
            if(planeObject == null) throw new FileNotFoundException();
            // attach the script
            attachScript(planeObject);
            return planeObject;
        }
            
        // todo: store the pool that spawned plane belongs to
        public static GameObject SpawnPlane(string prefabName, Vector2 position, Quaternion direction, Action<GameObject> attachScript)
        {
            GameObject planeObject = Instantiate(Resources.Load(prefabName), new Vector3(position.x, position.y, 0.0f), direction) as GameObject;
            if(planeObject == null) throw new FileNotFoundException();
            attachScript(planeObject);
            return planeObject;
        }
    }
        
    public class ProjectileFactory : MonoBehaviour
    {
        public static GameObject SpawnProjectile(string prefabName, Action<GameObject> attachScript)
        {
            GameObject projectileObject = Instantiate(Resources.Load(prefabName), Vector3.zero, Quaternion.AngleAxis(90.0f, Vector3.forward)) as GameObject;
            if(projectileObject == null) throw new FileNotFoundException();
            attachScript(projectileObject);
            //DontDestroyOnLoad(projectileObject);
            return projectileObject;
        }
        public static GameObject SpawnProjectile(string prefabName, Vector3 position, Quaternion direction, Action<GameObject> attachScript)
        {
            GameObject projectileObject = Instantiate(Resources.Load(prefabName), position, direction) as GameObject;
            if(projectileObject == null) throw new FileNotFoundException();
            attachScript(projectileObject);
            return projectileObject;
        }
    }
}