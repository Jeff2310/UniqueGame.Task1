using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class EnemyPlane : MonoBehaviour, IResetable
{
    public GameObject GameObject { get; protected internal set; }

    public float MaximumSpeed = 3.0f;
    public float Speed;
    public float SpeedModifier = 1.0f;
    public int MaximumHealth = 3;
    public int Health;
    public int DamageByContact;

    // Use this for initialization
    void Awake ()
    {
        // no magic number!
        Speed = MaximumSpeed;
        Health = MaximumHealth;
    }
	
    // Update is called once per frame
    void Update () {
        if (Health <= 0)
        {
            ShouldDestroy(this, EventArgs.Empty);
        }

        Speed = MaximumSpeed * SpeedModifier;
        GameObject.GetComponent<Rigidbody2D>().velocity = GameObject.transform.up * Speed;
    }
	
    // Interface and public methods
    public void Reset()
    {
        GameObject.SetActive(false);
        GameObject.transform.position = Vector3.zero;
        GameObject.transform.rotation = Quaternion.AngleAxis(90.0f, Vector3.forward);
        GameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Health = MaximumHealth;
    }

    public void SetActive(bool value)
    {
        GameObject.SetActive(value);
    }
    
    public event EventHandler HitByProjectile;
    public event EventHandler ShouldDestroy;
    
    protected void hitByProjectile(Object sender, EventArgs args)
    {
        HitByProjectile(sender, args);
    }

    public void shouldDestroy(Object sender, EventArgs args)
    {
        ShouldDestroy(sender, args);
    }

    // custom process 
    
    private void OnHit()
    {
        Health--;
        HitByProjectile(this, EventArgs.Empty);
    }

    // collision related
    
    protected void OnCollisionEnter2D(Collision2D other)
    {
        // hit by projectile
        if (other.gameObject.CompareTag("Projectile") == true)
        {
            OnHit();
            var projectileController = other.gameObject.GetComponent<ProjectileController>();
            projectileController.shouldDestroy(projectileController, EventArgs.Empty);
            return;
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bound") == true)
        {
            shouldDestroy(this, EventArgs.Empty);
            return;
        }
        // hit by player turret
        if (other.gameObject.CompareTag("Player") == true)
        {
            shouldDestroy(this, EventArgs.Empty);
            return;
        }
    }
}
