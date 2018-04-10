using System;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class TurretController : MonoBehaviour
{
	public GameObject GameObject { get; protected internal set; }
	public event EventHandler HitByEnemy;

	public int Health = 4;

	// Use this for initialization
	void Start()
	{
		GameObject = GameObject.Find("turret");
	}

	// Update is called once per frame
	void Update()
	{
		AimAtMouse();
	}

	private void AimAtMouse()
	{
		// the key point is setting the distance to the distance between camera and 2d plane
		Vector3 mousePoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
			Math.Abs(Camera.main.transform.position.z));
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mousePoint);
		Vector3 turretPosition = GameObject.transform.position;
		Vector3 aimVector = mousePosition - turretPosition;
		var _direction = Quaternion.FromToRotation(Vector3.up, aimVector);
		GameObject.transform.rotation = _direction;
	}
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		// hit by plane
		if (other.gameObject.CompareTag("Enemy") == true)
		{
			HitByEnemy(this, EventArgs.Empty);
			return;
		}
	}
}
