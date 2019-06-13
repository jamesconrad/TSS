using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public float maxHP = 100;
    public float curHP;
    public enum physicalMaterial { FLESH, INDESTRUCTABLE, WOOD, STONE, GLASS };
    public physicalMaterial physMat;

	// Use this for initialization
	void Start () {
        curHP = maxHP;
	}

    // Use for healing and taking damage, returns new % hp for impact particle scaling
    public float ChangeHP(float mod)
    {
        float newHP = curHP + mod;
        curHP = newHP > maxHP ? maxHP : newHP < 0 ? 0 : newHP;
        return curHP / maxHP;
    }
}
