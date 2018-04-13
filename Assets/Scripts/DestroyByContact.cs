﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player") == true)
		{
			ObjectPoolManager.GetInstance().StoreObject("plane", gameObject);
		}
	}
}