using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using Object = UnityEngine.Object;
using Random = System.Random;

public class GameSceneManager : MonoBehaviour
{
	private ObjectPool.PlanePool _freePlanePool;
	private ObjectPool.PlanePool _freeLeaderPool;
	private ObjectPool.ProjectilePool _freeProjectilePool;

	public GameObject TurretGameObject;
	// Use this for initialization

	public event EventHandler Quit;

	void Awake ()
	{
		_freePlanePool = new ObjectPool.PlanePool("normal_plane", 
			delegate(GameObject go) { return go.GetComponent<NormalPlaneController>(); },
			delegate(GameObject go) { go.AddComponent<NormalPlaneController>(); },
			30);
		_freeProjectilePool = new ObjectPool.ProjectilePool("projectile", 
			delegate(GameObject go) { return go.GetComponent<ProjectileController>(); },
			delegate(GameObject go) { go.AddComponent<ProjectileController>(); },
			50);
		_freeLeaderPool = new ObjectPool.PlanePool("leader_plane", 
			delegate(GameObject go) { return go.GetComponent<LeaderPlaneController>(); },
			delegate(GameObject go) { go.AddComponent<LeaderPlaneController>(); },
			30);
		
		Quit += OnQuit;
		
		foreach (var planeObject in _freePlanePool.ObjectStack)
		{
			planeObject.GetComponent<NormalPlaneController>().HitByProjectile += OnHitByProjectile;
			planeObject.GetComponent<NormalPlaneController>().ShouldDestroy += OnShouldDestroy;
		}
		
		foreach (var planeObject in _freeLeaderPool.ObjectStack)
		{
			planeObject.GetComponent<LeaderPlaneController>().HitByProjectile += OnHitByProjectile;
			planeObject.GetComponent<LeaderPlaneController>().ShouldDestroy += OnShouldDestroy;
		}
		
		foreach (var projectileObject in _freeProjectilePool.ObjectStack)
		{
			projectileObject.GetComponent<ProjectileController>().ShouldDestroy += OnShouldDestroy;
		}
		
		TurretGameObject = GameObject.Find("turret");
		TurretGameObject.GetComponent<TurretController>().ShouldDestroy += OnShouldDestroy;
	}
	
	private Random rand = new Random(); 
	// Update is called once per frame
	void Update () {
		// todo: write a event system
		if (Input.GetKeyDown(KeyCode.S))
		{
			GameObject planeObject = _freePlanePool.GetObject();
			if (planeObject == null) return;
			planeObject.transform.position = new Vector3(10.0f, (float)rand.Next(-8, 8), 0.0f);
			Vector3 aimToTurret = Vector3.Normalize(TurretGameObject.transform.position - planeObject.transform.position);
			planeObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, aimToTurret);
			//planeObject.GetComponent<Rigidbody2D>().velocity = planeObject.GetComponent<NormalPlaneController>().Speed * aimToTurret;
		}

		if (Input.GetKeyDown(KeyCode.D))
		{
			GameObject planeObject = _freeLeaderPool.GetObject();
			if (planeObject == null) return;
			planeObject.transform.position = new Vector3(10.0f, (float)rand.Next(-8, 8), 0.0f);
			Vector3 aimToTurret = Vector3.Normalize(TurretGameObject.transform.position - planeObject.transform.position);
			planeObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, aimToTurret);
			//planeObject.GetComponent<Rigidbody2D>().velocity = planeObject.GetComponent<LeaderPlaneController>().Speed * aimToTurret;
		}
		
		CheckAndShoot();
	}
	
	void Cleanup()
	{
		foreach (var go in GameObject.FindGameObjectsWithTag("Enemy"))
		{
			_freePlanePool.StoreObject(go);
		}

		foreach (var go in GameObject.FindGameObjectsWithTag("Projectile"))
		{
			_freeProjectilePool.StoreObject(go);
		}
		SceneManager.LoadScene(0);
	}

	private void CheckAndShoot()
	{
		if(Input.GetMouseButtonDown(0))
		{
			GameObject projectileObject = _freeProjectilePool.GetObject();
			if (projectileObject == null) return;
			projectileObject.transform.rotation = TurretGameObject.transform.rotation;
			projectileObject.transform.position = GameObject.Find("gunpoint").transform.position;
			projectileObject.SetActive(true);
		}
	}
	
	void OnQuit(object sender, EventArgs args)
	{
		Cleanup();
	}
	
	void OnHitByProjectile(object sender, EventArgs args)
	{
	}
	
	void OnShouldDestroy(object sender, EventArgs args)
	{
		if (sender is NormalPlaneController)
		{
			var _sender= sender as NormalPlaneController;
			_freePlanePool.StoreObject(_sender.GameObject);
		}

		if (sender is LeaderPlaneController)
		{
			var _sender = sender as LeaderPlaneController;
			_freeLeaderPool.StoreObject(_sender.GameObject);
		}

		if (sender is ProjectileController)
		{
			var _sender = sender as ProjectileController;
			_freeProjectilePool.StoreObject(_sender.GameObject);
		}

		if (sender is TurretController)
		{
			Quit(this, EventArgs.Empty);
		}
	}
}
