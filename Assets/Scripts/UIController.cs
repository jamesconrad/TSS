using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : Singleton<UIController>
{
    public GameObject player;

    public Health playerHealth;
    public int activeWeapon;
    public Weapon equipedWeapon;
    public Weapon.WeaponStats[] weapons;

    public Text healthText;
    public Image healthBar;
    public Image[] weaponBoxes;
    public Text ammoText;

    private float clickTime;

    // Start is called before the first frame update
    void Start()
    {
        weapons[0] = equipedWeapon.gun;
        activeWeapon = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && weaponBoxes[0].isActiveAndEnabled == true)
            EquipWeapon(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2) && weaponBoxes[1].isActiveAndEnabled == true)
            EquipWeapon(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3) && weaponBoxes[2].isActiveAndEnabled == true)
            EquipWeapon(2);

        //health
        healthText.text = playerHealth.curHP.ToString();

        //weapons
        ammoText.text = equipedWeapon.gun.curammo + "/" + equipedWeapon.gun.ammoleft;

    }

    public int AddWeapon(Weapon.WeaponStats ws)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weaponBoxes[i].isActiveAndEnabled)
            {
                print("Box " + i + " has weapon " + weapons[i].name);
                continue;
            }
            AddWeaponToBox(ws, i);
            return i;
        }
        return -1;
    }

    private void AddWeaponToBox(Weapon.WeaponStats ws, int slotId)
    {
        weapons[slotId] = ws;
        weaponBoxes[slotId].sprite = ws.sprite;
        weaponBoxes[slotId].gameObject.SetActive(true);
    }

    public void RemoveWeapon(int slotId)
    {
        for (int i = 0; i < weaponBoxes.Length; i++)
        {
            if (weaponBoxes[i].gameObject.activeSelf && i != slotId)
            {
                weaponBoxes[slotId].gameObject.SetActive(false);
                EquipWeapon(i);
                break;
            }
        }
    }

    public void SwapWeaonBox(int slotA, int slotB)
    {
        Weapon.WeaponStats aW = weapons[slotA];
        Sprite aI = weaponBoxes[slotA].sprite;
        weapons[slotA] = weapons[slotB];
        weaponBoxes[slotA].sprite = weaponBoxes[slotB].sprite;
        weapons[slotB] = aW;
        weaponBoxes[slotB].sprite = aI;
        if (activeWeapon == slotA)
            activeWeapon = slotB;
        else if (activeWeapon == slotB)
            activeWeapon = slotA;
    }

    public void EquipWeapon(int slotId)
    {
        if (slotId == activeWeapon || !weaponBoxes[slotId].gameObject.activeSelf)
            return;
        equipedWeapon.EquipWeapon(weapons[slotId], ref weapons[activeWeapon]);
        activeWeapon = slotId;
        //print("Euiped: " + weapons[slotId].name+"\nReplaced: " + weapons[activeWeapon].name);
    }

    /// <summary>
    /// adds ammo to current weapon
    /// </summary>
    /// <param name="ammo">if -1 adds max ammo</param>
    /// <returns></returns>
    public int AddAmmo(int ammo)
    {
        if (ammo == -1)
            ammo = (int)equipedWeapon.gun.maxammo;
        int newammo = (int)equipedWeapon.gun.ammoleft + ammo;
        if (newammo > (int)equipedWeapon.gun.maxammo)
        {
            equipedWeapon.gun.ammoleft = equipedWeapon.gun.maxammo;
            return newammo - (int)equipedWeapon.gun.maxammo;
        }
        equipedWeapon.gun.ammoleft = newammo;
        return newammo;
    }
}
