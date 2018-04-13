using System;
using UnityEngine;

public class PlaneShieldController : MonoBehaviour, IResetable
{
        
        public int MaximumHealth = 2;
        public int Health;

        public bool isInCoolDown;
        public float CoolDownTime = 0.5f;
        public float CoolDownOffset;

        // events
        public event EventHandler HitByProjectile;
        public event EventHandler ShouldDestroy;
        
        void Start()
        {
                Health = MaximumHealth;
                isInCoolDown = false;
                CoolDownOffset = 0.0f;
        }

        void Update()
        {
                if (isInCoolDown)
                {
                        CoolDownOffset += Time.deltaTime;
                        if (CoolDownOffset > CoolDownTime)
                        {
                                Refresh();
                        }
                }
                if (Health <= 0)
                {
                        //ShouldDestroy(this, EventArgs.Empty);
                        CoolDown();
                }
        }
        // custom callbacks
        private void OnHit()
        {
                //HitByProjectile(this, EventArgs.Empty);
        }
        
        // behaviours
        public void LoseHealth(int damage)
        {
                Health -= damage;
        }

        public void CoolDown()
        {
                Health = MaximumHealth;
                isInCoolDown = true;
                CoolDownOffset = 0.0f;
                GetComponent<Collider2D>().isTrigger = true;
                GetComponent<SpriteRenderer>().enabled = false;
        }

        public void Refresh()
        {
                isInCoolDown = false;
                CoolDownOffset = 0.0f;
                GetComponent<Collider2D>().isTrigger = false;
                GetComponent<SpriteRenderer>().enabled = true;
        }
        
        // interfaces
        public void Reset()
        {
                Health = MaximumHealth;
                Refresh();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
                if (other.gameObject.CompareTag("Projectile") == true)
                {
                       OnHit();
                       var projectileController = other.gameObject.GetComponent<ProjectileController>();
                       LoseHealth(projectileController.Damage);
                       projectileController.Destroy();
                }
        }
}
