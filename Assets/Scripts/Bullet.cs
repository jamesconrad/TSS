using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public Weapon.WeaponStats.AmmoType ammo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print(collision.gameObject.name);

        Health hitHealth = collision.gameObject.GetComponent<Health>();
        if (hitHealth == null)
            return;
        else if (hitHealth.physMat == Health.physicalMaterial.INDESTRUCTABLE)
        {
            Destroy(gameObject);
        }
        else if (hitHealth.physMat == Health.physicalMaterial.FLESH)
        {
            hitHealth.ChangeHP(-damage);
            if (ammo == Weapon.WeaponStats.AmmoType.ARMORPIERCING)
                damage /= 2;
            else
                Destroy(gameObject);
        }
        else if (hitHealth.physMat == Health.physicalMaterial.STONE)
        {
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
            Vector3 normal = collision.GetContact(0).normal;
            Vector3 vel = rigidbody.velocity;

            if (Vector3.Angle(vel, -normal) > 45)
                rigidbody.velocity = Vector3.Reflect(vel, normal);
            else
            {
                damage = damage / 4;
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        transform.DetachChildren();
    }
}
