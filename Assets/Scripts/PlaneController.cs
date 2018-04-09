using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class PlaneController : MonoBehaviour, IResetable
{
	private GameObject _planeObject = null;
	public GameObject PlaneObject
	{
		get { return _planeObject; }
		protected internal set { _planeObject = value; }
	}
	
	private Quaternion _direction;
	private Vector3 _position;
	private static float _defaultSpeed = 3.0f;
	private float _speed;
	
	// Use this for initialization
	void Start ()
	{
		if (null != _planeObject)
		{
			_direction = _planeObject.transform.rotation;
			_position = _planeObject.transform.position;
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
		_planeObject.SetActive(false);
		_planeObject.transform.position = Vector3.zero;
		_planeObject.transform.rotation = Quaternion.AngleAxis(90.0f, Vector3.forward);
	}

	public void SetActive(bool value)
	{
		_planeObject.SetActive(value);
	}
	// private methods
	private void HandleMovement()
	{
		_direction = _planeObject.transform.rotation;
		_position = _planeObject.transform.position;

		Vector3 newPosition = _position + _speed * Time.deltaTime * gameObject.transform.up;
		_planeObject.transform.position = newPosition;
	}
}
