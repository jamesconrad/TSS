using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbleSpawner : MonoBehaviour
{
    public Sprite[] validRoofSprites;
    public Sprite[] validWallSprites;
    public enum RubbleType { ROOF, WALL };
    public RubbleType rubbleType;
    public int spriteInstances = 3;
    private static GameObject baseRubble;

    // Start is called before the first frame update
    void Start()
    {
        if (baseRubble == null)
            baseRubble = Resources.Load<GameObject>("Rubble");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateRubble()
    {
        for (int i = 0; i < spriteInstances; i++)
        {
            int rand = Random.Range(0, rubbleType == RubbleType.ROOF ? validRoofSprites.Length : validWallSprites.Length);
            GameObject rubble = Instantiate<GameObject>(baseRubble, transform.position + (Vector3)Random.insideUnitCircle / 2, Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f)), transform);
            rubble.GetComponent<SpriteRenderer>().sprite = rubbleType == RubbleType.ROOF ? validRoofSprites[rand] : validWallSprites[rand];
        }
    }
}
