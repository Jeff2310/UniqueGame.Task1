using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Random = System.Random;

public class GameSceneManager : MonoBehaviour
{
	private ObjectPool.PlanePool _freePlanePool;
	private Queue<GameObject> _planeQueue;

	private ObjectPool.ProjectilePool _freeProjectilePool;
	private Queue<GameObject> _projectileQueue;

	public GameObject TurretGameObject;
	// Use this for initialization
	void Start ()
	{
		_freePlanePool = new ObjectPool.PlanePool(30);
		_freeProjectilePool = new ObjectPool.ProjectilePool(50);
		
		foreach (var planeObject in _freePlanePool.ObjectStack)
		{
			planeObject.GetComponent<PlaneController>().HitByProjectile += OnHitByProjectile;
			planeObject.GetComponent<PlaneController>().HitByPlayer+= OnHitByPlayer;
			planeObject.GetComponent<PlaneController>().ReachBound += OnReachBound;
		}
		
		foreach (var projectileObject in _freeProjectilePool.ObjectStack)
		{
			projectileObject.GetComponent<ProjectileController>().HitByEnemy += OnHitByEnemy;
			projectileObject.GetComponent<ProjectileController>().ReachBound += OnReachBound;
		}
		
		_planeQueue = new Queue<GameObject>();
		_projectileQueue = new Queue<GameObject>();
		
		TurretGameObject = GameObject.Find("turret");
		TurretGameObject.GetComponent<TurretController>().HitByEnemy += OnHitByEnemy;
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
			Vector3 aimToTurret = TurretGameObject.transform.position - planeObject.transform.position;
			planeObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, aimToTurret);
			_planeQueue.Enqueue(planeObject);
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			if (_planeQueue.Count > 0)
			{
				var plane = _planeQueue.Dequeue();
				_freePlanePool.StoreObject(plane);
			}
		}

		CheckAndShoot();
	}

	void Quit()
	{
		foreach (var go in _planeQueue)
		{
			_freePlanePool.StoreObject(go);
		}

		foreach (var go in _projectileQueue)
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
			_projectileQueue.Enqueue(projectileObject);
		}
	}
	
	void OnHitByProjectile(object sender, EventArgs args)
	{
		if (sender.GetType().FullName == "PlaneController")
		{
			var _sender= sender as PlaneController;
			_sender.Health--;
			if (_sender.Health == 0)
			{
				_freePlanePool.StoreObject(_sender.GameObject);
			}
		}
	}
	
	void OnHitByPlayer(object sender, EventArgs args)
	{
		if (sender.GetType().FullName == "PlaneController")
		{
			var _sender= sender as PlaneController;
			_freePlanePool.StoreObject(_sender.GameObject);
		}
	}
	
	void OnHitByEnemy(object sender, EventArgs args)
	{
		var typename = sender.GetType().FullName;
		if (typename == "TurretController")
		{
			var _sender = sender as TurretController;
			_sender.Health--;
			if (_sender.Health == 0)
			{
				Quit();
			}
			return;
		}
		if (typename == "ProjectileController")
		{
			var _sender= sender as ProjectileController;
			_freeProjectilePool.StoreObject(_sender.GameObject);
			return;
		}
	}

	void OnReachBound(object sender, EventArgs args)
	{
		var typename = sender.GetType().FullName;
		//Debug.Log(String.Format("a {0} has reached the bound!", typename));
		if (typename == "PlaneController")
		{
			var _sender = sender as PlaneController;
			_freePlanePool.StoreObject(_sender.GameObject);
		}
		else if (typename == "ProjectileController")
		{
			var _sender = sender as ProjectileController;
			_freeProjectilePool.StoreObject(_sender.GameObject);
		}
	}
}
