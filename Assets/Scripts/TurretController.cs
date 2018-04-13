using System;
using System.Diagnostics;
using NUnit.Framework;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.XR.WSA.Input;
using Debug = UnityEngine.Debug;

public class TurretController : MonoBehaviour
{
	public int MaximumHealth = 4;
	public int Health;

	public float CooldownTime = 0.5f;
	public float CooldownOffset;
	public bool IsInCooldown;

	public GameObject _projectilePrefab;
	
	// Use this for initialization
	void Start()
	{
		Health = MaximumHealth;
		CooldownOffset = 0.0f;
		IsInCooldown = false;
	}

	// Update is called once per frame
	void Update()
	{
		AimAtMouse();
		CheckAndShoot();
		if (Health <= 0)
		{
			ShouldDestroy(this, EventArgs.Empty);
		}
	}
	
	// events
	
	public event EventHandler FireProjectile;
	public event EventHandler TakenDamage;
	public event EventHandler ShouldDestroy;

	// behaviours

	public void LoseHealth(int damage)
	{
		Health -= damage;
	}


	// actions
	private void AimAtMouse()
	{
		// the key point is setting the distance to the distance between camera and 2d plane
		Vector3 mousePoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
			Math.Abs(Camera.main.transform.position.z));
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mousePoint);
		Vector3 turretPosition = transform.position;
		Vector3 aimVector = mousePosition - turretPosition;
		var _direction = Quaternion.FromToRotation(Vector3.up, aimVector);
		transform.rotation = _direction;
	}
	
	private void CheckAndShoot()
	{
		if (IsInCooldown)
		{
			CooldownOffset += Time.deltaTime;
			if (CooldownOffset > CooldownTime)
			{
				IsInCooldown = false;
				CooldownOffset = 0.0f;
			}
			return;
		}
		if (Input.GetMouseButton(0))
		{
			var f = Time.time;
			var projectileObject = ObjectPoolManager.GetInstance().GetObject("projectile", _projectilePrefab);
			if (projectileObject == null) return;
			projectileObject = projectileObject as GameObject;
			projectileObject.transform.rotation = transform.rotation;
			projectileObject.transform.position = GameObject.Find("gunpoint").transform.position;
			projectileObject.SetActive(true);

			IsInCooldown = true;
			try
			{
				GameObject sound = GameObject.Find("FireSound");
				sound.GetComponent<AudioSource>().Play();
				//FireProjectile(this, EventArgs.Empty);
			}
			catch (NullReferenceException e)
			{
				Debug.Log(e.Message);
			}
		}
	}
}
