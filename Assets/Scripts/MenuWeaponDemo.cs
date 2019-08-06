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
        if (ws.type == Weapon.WeaponStats.WeaponType.GUN)
        {
            stats += "Recoil: " + ws.recoil + "\n";
            stats += "Accuracy: " + (ws.accuracy == 0 ? "100" : (ws.accuracy/10*100).ToString()) + "%\n";
            stats += "Firerate: " + 1/ws.firerate + "/s\n";
            stats += "Magazine Size: " + ws.magazinesize + "\n";
            stats += "Max Ammo: " + ws.maxammo + "\n";
            stats += "Reload Speed: " + ws.reloadtime + "s\n";
            stats += "Trigger Type: " + ws.triggertype.ToString() + "\n";
        }


        weaponStatsText.text = stats;
    }
}
