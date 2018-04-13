using UnityEngine;

public class EnemyPlaneBeheviour : MonoBehaviour, IResetable
{
    public EnemyPlane Plane;

    private void Start()
    {
        Plane.OnTakenDamage += OnTakenDamge;
    }

    // Interface and public methods
    public virtual void Reset()
    {
        Plane.gameObject.SetActive(false);
        Plane.gameObject.transform.position = Vector3.zero;
        Plane.gameObject.transform.rotation = Quaternion.AngleAxis(90.0f, Vector3.forward);
        Plane.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Plane.Revive();
        //Debug.Log("base reset");
    }

    public virtual void SetAttachment()
    {
        
    }

    public virtual void OnTakenDamge() {}
}
