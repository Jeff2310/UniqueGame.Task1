using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageByContact : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Enemy") == true)
		{
			this.GetComponent<TurretController>().LoseHealth( other.gameObject.GetComponent<EnemyPlane>().DamageByContact );
		}
	}
}
