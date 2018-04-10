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
	private int maximumHealth = 3;
	public int Health;

	public event EventHandler HitByProjectile;
	public event EventHandler HitByPlayer;
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

		Health = maximumHealth;
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
		Health = maximumHealth;
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
			return;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Bound") == true)
		{
			ReachBound(this, EventArgs.Empty);
		}
		// hit by player turret
		if (other.gameObject.CompareTag("Player") == true)
		{
			HitByPlayer(this, EventArgs.Empty);
		}
	}
}
