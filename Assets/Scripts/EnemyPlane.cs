using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class EnemyPlane : MonoBehaviour
{
    public float Speed = 3.0f;
    public float CurrentSpeed;
    public float SpeedModifier = 1.0f;
    public int Health = 3;
    public int CurrentHealth;
    public int DamageByContact = 1;
    
    // events
    public event EventHandler HitByProjectile;
    public event EventHandler ShouldDestroy;
    
    // custom callbacks for controller;
    public Action OnTakenDamage;

    // Use this for initialization
    void Awake ()
    {
        // no magic number!
        CurrentSpeed = Speed;
        CurrentHealth = Health;
    }
	
    // Update is called once per frame
    void Update () {
        if (CurrentHealth <= 0)
        {
            //ShouldDestroy(this, EventArgs.Empty);
            ObjectPoolManager.GetInstance().StoreObject("plane", gameObject);
        }
        CurrentSpeed = Speed * SpeedModifier;
        gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * CurrentSpeed;
    }

    
    // behaviours

    public void Revive()
    {
        CurrentHealth = Health;
    }
    
    public void LoseHealth(int damage)
    {
        CurrentHealth -= damage;
    }

    // collision related
    
    protected void OnTriggerEnter2D(Collider2D other)
    {
        // hit by projectile
        if (other.gameObject.CompareTag("Projectile") == true)
        {
            var projectileController = other.gameObject.GetComponent<ProjectileController>();
            LoseHealth(projectileController.Damage);
            projectileController.Destroy();
            OnTakenDamage();
            return;
        }
    }
}
