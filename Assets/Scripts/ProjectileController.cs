using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
	public GameObject GameObject { get; protected internal set; }
	public GameObject Owner { get; protected internal set; }
	public Vector3 Position { get; protected internal set; }

	public float Speed;
	// Use this for initialization
	void Start ()
	{
		Speed = 5.0f;
	}
	
	// Update is called once per frame
	void Update () {
			handleMovement();
	}

	private void handleMovement()
	{
		Position = GameObject.transform.position;

		Vector3 newPosition = Position + Speed * Time.deltaTime * gameObject.transform.up;
		GameObject.transform.position = newPosition;
	}

}
