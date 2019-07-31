using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLoader : Singleton<WeaponLoader>
{
    public TextAsset[] weaponJsons;
    private Weapon.WeaponStats[] cached;

    public void Start()
    {
        Sprite[] weaponSprites = Resources.LoadAll<Sprite>("weapons");
        cached = new Weapon.WeaponStats[weaponJsons.Length];
        for (int i = 0; i < weaponJsons.Length; i++)
        {
            cached[i] = JsonUtility.FromJson<Weapon.WeaponStats>(weaponJsons[i].text);
            cached[i].sprite = weaponSprites[i];
        }
    }

    public Weapon.WeaponStats Load(int gunId)
    {
        if (gunId > weaponJsons.Length || gunId < 0)
        {
            Debug.LogError("Invalid gunId for WeaponLoader: " + gunId);
            return cached[0];
        }
        return cached[gunId];
    }

    public int NumLoadedGuns()
    {
        return cached.Length;
    }
}
