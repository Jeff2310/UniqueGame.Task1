using UnityEngine;

public class Factories
{
    public class PlaneFactory : MonoBehaviour
    {
        public static GameObject SpawnPlane()
        {
            GameObject planeObject = 
                Instantiate(Resources.Load("plane"), Vector3.zero, Quaternion.AngleAxis(90.0f, Vector3.forward)) as GameObject;
            planeObject.AddComponent<PlaneController>();
            planeObject.GetComponent<PlaneController>().GameObject = planeObject;
            // might cause issue in multi-scene project
            //DontDestroyOnLoad(planeObject);
            return planeObject;
        }
            
        // todo: store the pool that spawned plane belongs to
        public static GameObject SpawnPlane(Vector2 position, Quaternion direction)
        {
            GameObject planeObject = Instantiate(Resources.Load("plane"), new Vector3(position.x, position.y, 0.0f), direction) as GameObject;
            planeObject.AddComponent<PlaneController>();
            planeObject.GetComponent<PlaneController>().GameObject = planeObject;
            //DontDestroyOnLoad(planeObject);
            return planeObject;
        }
    }
        
    public class ProjectileFactory : MonoBehaviour
    {
        public static GameObject SpawnProjectile()
        {
            GameObject projectileObject = Instantiate(Resources.Load("projectile"), Vector3.zero, Quaternion.AngleAxis(90.0f, Vector3.forward)) as GameObject;
            projectileObject.AddComponent<ProjectileController>();
            projectileObject.GetComponent<ProjectileController>().GameObject = projectileObject;
            //DontDestroyOnLoad(projectileObject);
            return projectileObject;
        }
        public static GameObject SpawnProjectile(Vector3 position, Quaternion direction)
        {
            GameObject projectileObject = Instantiate(Resources.Load("projectile"), position, direction) as GameObject;
            projectileObject.AddComponent<ProjectileController>();
            projectileObject.GetComponent<ProjectileController>().GameObject = projectileObject;
            //DontDestroyOnLoad(projectileObject);
            return projectileObject;
        }
    }
}