using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostRealmController : MonoBehaviour {
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyPlane>().SpeedModifier = 1.2f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyPlane>().SpeedModifier = 1.0f;
        }
    }
}
