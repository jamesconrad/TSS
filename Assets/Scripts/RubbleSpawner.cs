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

    // Start is called before the first frame update
    void Start()
    {
        
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
            Sprite sprite = Instantiate(rubbleType == RubbleType.ROOF ? validRoofSprites[rand] : validWallSprites[rand]);
            GameObject rubble = Instantiate(new GameObject(), transform.position + (Vector3)Random.insideUnitCircle / 2, Quaternion.Euler(0,0,Random.Range(0.0f,360.0f)), transform);
            SpriteRenderer sr = rubble.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.sortingOrder = 1;
        }
    }
}
