using UnityEngine;

public class ShieldPlaneController : EnemyPlaneBeheviour
{
    public ShieldPlaneController()
    {
        //Plane.Speed = 2.0f;
        //Plane.Health = 4;
    }
    
    public new void Reset()
    {
        /*  | base.Reset()
        GameObject.SetActive(false);
        GameObject.transform.position = Vector3.zero;
        GameObject.transform.rotation = Quaternion.AngleAxis(90.0f, Vector3.forward);
        GameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        CurrentHealth = Health;
        */
        base.Reset();
        gameObject.GetComponentInChildren<PlaneShieldController>().Reset();
    }
}
