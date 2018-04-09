using UnityEngine;

namespace DefaultNamespace
{
    public class Factories
    {
        public class PlaneFactory : MonoBehaviour
        {
            public static GameObject SpawnPlane()
            {
                GameObject planeObject = 
                    Instantiate(Resources.Load("plane"), Vector2.zero, Quaternion.AngleAxis(90.0f, Vector3.forward)) as GameObject;
                planeObject.AddComponent<PlaneController>();
                planeObject.GetComponent<PlaneController>().PlaneObject = planeObject;
                DontDestroyOnLoad(planeObject);
                return planeObject;
            }

            public static GameObject SpawnPlane(Vector2 position, Quaternion direction)
            {
                GameObject planeObject = Instantiate(Resources.Load("plane"), new Vector3(position.x, position.y, 0.0f), direction) as GameObject;
                planeObject.AddComponent<PlaneController>();
                planeObject.GetComponent<PlaneController>().PlaneObject = planeObject;
                DontDestroyOnLoad(planeObject);
                return planeObject;
            }
        }
    }
}