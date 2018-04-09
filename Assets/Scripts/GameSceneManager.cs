using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
	public PlanePool _planePool;
	private Queue<GameObject> _planeQueue;

	public GameObject TurretGameObject;
	// Use this for initialization
	void Start ()
	{
		/*
		GameObject planePrefab = Resources.Load("plane") as GameObject;
		Instantiate(planePrefab, new Vector3(0.0f, 1.0f, 0.0f), Quaternion.AngleAxis(0, Vector3.up));
		GameObject planeObject = GameObject.Find("plane(Clone)");
		Debug.Log(planeObject.transform);
		planeObject.SetActive(false);
		*/
		_planePool = PlanePool.GetInstance();
		_planeQueue = new Queue<GameObject>();
		TurretGameObject = GameObject.Find("turret");
	}
	
	// Update is called once per frame
	void Update () {
		// todo: write a event system
		if (Input.GetKeyDown(KeyCode.S))
		{
			GameObject planeObject = _planePool.GetObject();
			_planeQueue.Enqueue(planeObject);
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			if (_planeQueue.Count > 0)
			{
				var plane = _planeQueue.Dequeue();
				_planePool.StoreObject(plane);
			}
		}
	}
}
