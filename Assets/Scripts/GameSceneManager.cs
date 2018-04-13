using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class GameSceneManager : MonoBehaviour
{
	public GameObject TurretGameObject;
	// Use this for initialization

	public event EventHandler Quit;

	public GameObject normalPrefab;
	public GameObject leaderPrefab;
	public GameObject shieldPrefab;
	public GameObject projectilePrefab;

	void Awake ()
	{
		ObjectPoolManager.GetInstance().CreatePool("projectile",
			(GameObject go) => { go.GetComponent<ProjectileController>().Reset();});
		ObjectPoolManager.GetInstance().CreatePool("plane",
			(GameObject go) => { go.GetComponent<EnemyPlaneBeheviour>().Reset(); });

		normalPrefab = Resources.Load("normal_plane") as GameObject;
		leaderPrefab = Resources.Load("leader_plane") as GameObject;
		shieldPrefab = Resources.Load("shield_plane") as GameObject;
		projectilePrefab = Resources.Load("projectile") as GameObject;
		
		Quit += OnQuit;
		
		TurretGameObject = GameObject.Find("turret");
		var turret = TurretGameObject.GetComponent <TurretController>();
		turret.FireProjectile += OnFired;
		turret.TakenDamage += OnTakenDamage;
		turret.ShouldDestroy += OnShouldDestroy;
		turret._projectilePrefab = projectilePrefab;
	}
	
	private Random rand = new Random(); 
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown(KeyCode.S))
		{
			GameObject planeObject = ObjectPoolManager.GetInstance().GetObject("plane", normalPrefab);
			planeObject.transform.position = new Vector3(10.0f, (float)rand.Next(-8, 8), 0.0f);
			Vector3 aimToTurret = Vector3.Normalize(TurretGameObject.transform.position - planeObject.transform.position);
			planeObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, aimToTurret);
		}
		
		if (Input.GetKeyDown(KeyCode.D))
		{
			GameObject planeObject = ObjectPoolManager.GetInstance().GetObject("plane", leaderPrefab);
			planeObject.transform.position = new Vector3(10.0f, (float)rand.Next(-8, 8), 0.0f);
			Vector3 aimToTurret = Vector3.Normalize(TurretGameObject.transform.position - planeObject.transform.position);
			planeObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, aimToTurret);
		}
		
		if (Input.GetKeyDown(KeyCode.A))
		{
			GameObject planeObject = ObjectPoolManager.GetInstance().GetObject("plane", shieldPrefab);
			planeObject.transform.position = new Vector3(10.0f, (float)rand.Next(-8, 8), 0.0f);
			Vector3 aimToTurret = Vector3.Normalize(TurretGameObject.transform.position - planeObject.transform.position);
			planeObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, aimToTurret);
		}
	}
	
	void Cleanup()
	{
		
	}
	
	void OnQuit(object sender, EventArgs args)
	{
		Cleanup();
		SceneManager.LoadScene(0);
	}

	void OnFired(object sender, EventArgs args)
	{
		// Sounds, Effects goes here;
		GameObject sound = GameObject.Find("FireSound");
		sound.GetComponent<AudioSource>().Play();
	}

	void OnTakenDamage(object sender, EventArgs args)
	{
		GameObject sound = GameObject.Find("GameOver");
		sound.GetComponent<AudioSource>().Play();
	}
	
	void OnShouldDestroy(object sender, EventArgs args)
	{
		Quit(this, EventArgs.Empty);
	}
}
