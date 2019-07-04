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

    public bool AddWeapon(Weapon.WeaponStats ws)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weaponBoxes[i].isActiveAndEnabled)
            {
                print("Box " + i + " has weapon " + weapons[i].name);
                continue;
            }
            weapons[i] = ws;
            weaponBoxes[i].sprite = ws.sprite;
            weaponBoxes[i].gameObject.SetActive(true);
            print("Equiped " + ws.name + " to slot " + i + ".");
            return true;
        }
        return false;
    }

    public void EquipWeapon(int slotId)
    {
        if (slotId == activeWeapon)
            return;
        equipedWeapon.EquipWeapon(weapons[slotId], ref weapons[activeWeapon]);
        activeWeapon = slotId;
        print("Euiped: " + weapons[slotId].name+"\nReplaced: " + weapons[activeWeapon].name);
    }
}
