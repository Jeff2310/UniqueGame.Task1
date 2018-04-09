using System;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class TurretController : MonoBehaviour
{
	public GameObject GameObject { get; protected internal set; }

	private Quaternion _direction;

	// Use this for initialization
	void Start()
	{
		GameObject = GameObject.Find("turret");
	}

	// Update is called once per frame
	void Update()
	{
		AimAtMouse();
		CheckAndShoot();
	}

	private void AimAtMouse()
	{
		// the key point is setting the distance to the distance between camera and 2d plane
		Vector3 mousePoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
			Math.Abs(Camera.main.transform.position.z));
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mousePoint);
		Vector3 turretPosition = GameObject.transform.position;
		Vector3 aimVector = mousePosition - turretPosition;
		_direction = Quaternion.FromToRotation(Vector3.up, aimVector);
		GameObject.transform.rotation = _direction;
	}
	
	private void CheckAndShoot()
	{
		// todo: change to a event-based version
		if(Input.GetMouseButtonDown(0))
		{
			Vector3 gunpointPosition = GameObject.Find("gunpoint").transform.position;
			GameObject projectileObject = Instantiate(Resources.Load("projectile"), gunpointPosition, _direction) as GameObject;
			projectileObject.AddComponent<ProjectileController>();
			projectileObject.GetComponent<ProjectileController>().GameObject = projectileObject;
		}
	}
}
