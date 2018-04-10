using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour, IResetable
{
	public GameObject GameObject { get; protected internal set; }
	public GameObject Owner { get; protected internal set; }
	public Vector3 Position { get; protected internal set; }

	public event EventHandler HitByEnemy;
	public event EventHandler ReachBound;
	
	public float Speed;
	// Use this for initialization
	void Start ()
	{
		Speed = 15.0f;
	}
	
	// Update is called once per frame
	void Update () {
			handleMovement();
	}

	public void Reset()
	{
		GameObject.SetActive(false);
		GameObject.transform.position = Vector3.zero;
		GameObject.transform.rotation = Quaternion.AngleAxis(90.0f, Vector3.forward);
	}
	
	private void handleMovement()
	{
		Position = GameObject.transform.position;
		Vector3 newPosition = Position + Speed * Time.deltaTime * gameObject.transform.up;
		GameObject.transform.position = newPosition;
	}
	
	private void OnCollisionEnter2D(Collision2D other)
	{
		// hit by projectile
		if (other.gameObject.CompareTag("Enemy") == true)
		{
			HitByEnemy(this, EventArgs.Empty);
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
