﻿using System.Collections;
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
        [Tooltip("Seconds between rounds")]
        public float firerate;

        public float muzzleVelocity;

        public float maxammo; //max ammo player can have for this gun
        public float curammo; //current ammo in magazine
        public float magazinesize; //max ammo in magazine
        public enum AmmoType {STANDARD, ARMORPIERCING, EXPLOSIVE, NONE};
        public AmmoType ammotype;
        public enum TriggerType { AUTO, SEMI};
        public TriggerType triggertype;
        public float reloadtime;
        public float ammoleft; //remaining total ammo

        // mainly for shotguns
        public float ammopershot;
        public float bulletspershot;

        public Sprite sprite;

        public Vector3 barrelExit;
        public Vector3 leftHand;
        public Vector3 rightHand;
    };

    public WeaponStats gun;
    public GameObject STDBullet;
    public bool saveToFile = false;
    private float firedelay = 0;
	// Use this for initialization
	void Start () {
        //force fetch from json
        gun = WeaponLoader.Instance.Load(0);
	}
	
	// Update is called once per frame
	void Update () {
        if (firedelay > 0)
            firedelay -= Time.deltaTime;
        if (saveToFile == true)
        {
            print(JsonUtility.ToJson(gun));
            saveToFile = false;
        }
	    if (gun.triggertype == WeaponStats.TriggerType.SEMI && Input.GetMouseButtonDown(0))
            Fire(transform.right);
        else if (gun.triggertype == WeaponStats.TriggerType.AUTO && Input.GetMouseButton(0))
            Fire(transform.right);
        if (Input.GetKey(KeyCode.R))
            Reload();
        if (Input.GetKeyDown(KeyCode.P))
            gun.ammoleft = gun.maxammo;
	}

    // Returns a new direction to jump to, then decline back from
    public Vector2 Fire(Vector2 direction)
    {
        if (gun.ammotype != WeaponStats.AmmoType.NONE && gun.curammo <= 0)
        {
            Reload();
            return direction;
        }
        if (firedelay > 0)
            return direction;
        //calcualte accuracy shift
        float accMod = Random.Range(-gun.accuracy, gun.accuracy);
        Vector2 bulletDir = Quaternion.Euler(new Vector3(0, 0, accMod)) * direction;

        //spawn bullet
        GameObject bullet = Instantiate(STDBullet);
        bullet.transform.position = transform.position + transform.rotation*Quaternion.Euler(0,0,-90)*gun.barrelExit;
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
        firedelay = gun.firerate;
        return Quaternion.Euler(new Vector3(0,0,spreadMod)) * direction;
    }

    public void EjectCasing()
    {

    }
    
    public bool Reload()
    {
        if (gun.ammotype != WeaponStats.AmmoType.NONE && gun.ammoleft <= 0)
            return false;

        // Dispatch reload animation, call this after completion
        float reloadcost = gun.maxammo - gun.curammo;
        float reloaded = gun.ammoleft > reloadcost ? reloadcost : gun.ammoleft;
        reloaded = reloaded > gun.magazinesize ? gun.magazinesize : reloaded;
        gun.curammo = reloaded;
        gun.ammoleft -= reloaded;
        return true;
    }

    public WeaponStats EquipWeapon(WeaponStats newWeapon, ref WeaponStats oldWeapon)
    {
        WeaponStats old = oldWeapon = gun;
        gun = newWeapon;
        GetComponent<SpriteRenderer>().sprite = newWeapon.sprite;
        return old;
    }
}
