using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    [System.Serializable]
    public struct WeaponStats {
        public string name;
        public float damage;
        [Tooltip("Gun roated by rand(-rec,rec)")]
        public float recoil;
        [Tooltip("Direction rotated by rand(-acc,acc)")]
        public float accuracy;
        [Tooltip("Rounds per second")]
        public float firerate;

        public float muzzleVelocity;

        public float maxammo;
        public float curammo;
        public enum AmmoType {STANDARD, ARMORPIERCING, EXPLOSIVE};
        public AmmoType ammotype;
        public float reloadtime;
        public float ammoleft;

        // mainly for shotguns
        public float ammopershot;
        public float bulletspershot;

        public Transform barrelExit;
    };

    public WeaponStats gun;
    public GameObject STDBullet;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetMouseButtonDown(0))
        {
            Fire(transform.up);
        }
	}

    // Returns a new direction to jump to, then decline back from
    public Vector2 Fire(Vector2 direction)
    {
        if (gun.curammo <= 0)
        {
            Reload();
            return direction;
        }
        //calcualte accuracy shift
        float accMod = Random.Range(-gun.accuracy, gun.accuracy);
        Vector2 bulletDir = Quaternion.Euler(new Vector3(0, 0, accMod)) * direction;

        //spawn bullet
        GameObject bullet = Instantiate(STDBullet);
        bullet.transform.position = gun.barrelExit.position;
        bullet.transform.rotation = Quaternion.LookRotation(new Vector3(bulletDir.x, bulletDir.y, 0));
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.damage = gun.damage;
        bulletScript.ammo = gun.ammotype;
        bulletScript.direction = bulletDir;
        bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(bulletDir.x, bulletDir.y) * gun.muzzleVelocity, ForceMode2D.Impulse);
        bulletScript.velocity = bullet.GetComponent<Rigidbody2D>().velocity.magnitude;

        gun.curammo -= gun.ammopershot;

        //calculate and return recoil shit
        float spreadMod = Random.Range(-gun.recoil, gun.recoil);
        return Quaternion.Euler(new Vector3(0,0,spreadMod)) * direction;
    }

    public void EjectCasing()
    {

    }
    
    public bool Reload()
    {
        if (gun.ammoleft <= 0)
            return false;

        // Dispatch reload animation, call this after completion
        float reloadcost = gun.maxammo - gun.curammo;
        float reloaded = gun.ammoleft > reloadcost ? reloadcost : gun.ammoleft;
        gun.curammo = reloaded;
        gun.ammoleft -= reloaded;
        return true;
    }
}
