using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class ProjectileController : MonoBehaviour, IResetable
{
	public float Speed = 15.0f;

	public int Damage = 1;
	
	// Use this for initialization
	void Start ()
	{
		Speed = 15.0f;
		Damage = 1;
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Rigidbody2D>().velocity = transform.up * Speed;
	}

	public void Reset()
	{
		gameObject.SetActive(false);
		gameObject.transform.position = Vector3.zero;
		gameObject.transform.rotation = Quaternion.AngleAxis(90.0f, Vector3.forward);
	}

	public void Destroy()
	{
		ObjectPoolManager.GetInstance().StoreObject("projectile", gameObject);
	}
}
