using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuWeaponDemo : MonoBehaviour
{
    public UnityEngine.UI.Text weaponStatsText;
    public void RunWeaponDemo(int weaponId)
    {
        int slotId = UIController.Instance.AddWeapon(WeaponLoader.Instance.Load(weaponId));
        ShowWeaponStats(weaponId);
        UIController.Instance.SwapWeaonBox(slotId, 0);
        UIController.Instance.EquipWeapon(0);
        UIController.Instance.RemoveWeapon(slotId);
    }

    private void ShowWeaponStats(int weaponId)
    {
        string stats = "";
        Weapon.WeaponStats ws = WeaponLoader.Instance.Load(weaponId);
        stats += ws.name + "\n";
        stats += "Damage: " + ws.damage + "\n";
        stats += "Recoil: " + ws.recoil + "\n";
        stats += "Accuracy: " + ws.accuracy + "\n";
        stats += "Firerate: " + ws.firerate + "\n";
        stats += "Magazine Size: " + ws.magazinesize + "\n";
        stats += "Max Ammo: " + ws.maxammo + "\n";
        stats += "Reload Speed: " + ws.reloadtime + "\n";
        weaponStatsText.text = stats;
    }
}
