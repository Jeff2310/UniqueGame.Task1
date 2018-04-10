using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour, IResetable
{
	public GameObject GameObject { get; protected internal set; }
	
	private Quaternion _direction;
	private Vector3 _position;
	private static float _defaultSpeed = 3.0f;
	private float _speed;

	public event EventHandler HitByProjectile;
	public event EventHandler ReachBound;
	
	// Use this for initialization
	void Start ()
	{
		if (null != GameObject)
		{
			_direction = GameObject.transform.rotation;
			_position = GameObject.transform.position;
			_speed = _defaultSpeed;
		}
	}
	
	// Update is called once per frame
	void Update () {
		HandleMovement();
	}
	
	// Interface and public methods
	public void Reset()
	{
		GameObject.SetActive(false);
		GameObject.transform.position = Vector3.zero;
		GameObject.transform.rotation = Quaternion.AngleAxis(90.0f, Vector3.forward);
	}

	public void SetActive(bool value)
	{
		GameObject.SetActive(value);
	}
	// private methods
	private void HandleMovement()
	{
		_direction = GameObject.transform.rotation;
		_position = GameObject.transform.position;

		Vector3 newPosition = _position + _speed * Time.deltaTime * gameObject.transform.up;
		GameObject.transform.position = newPosition;
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		// hit by projectile
		if (other.gameObject.CompareTag("Projectile") == true)
		{
			HitByProjectile(this, EventArgs.Empty);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Bound") == true)
		{
			ReachBound(this, EventArgs.Empty);
		}
	}
}
