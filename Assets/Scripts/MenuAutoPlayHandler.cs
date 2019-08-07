using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAutoPlayHandler : Singleton<MenuAutoPlayHandler>
{
    public BasicCharacterController playerController;
    public PlayerAI playerAI;
    public ZombieSpawner zombieSpawner;
    public SpriteRenderer fadeSprite;
    
    Transform zombieContainer;
    float demoTime = 0;
    float demoDuration = 10;
    float halfFadeDuration = 2;
    Color fadeColour;

    private Vector2 cameraDir = new Vector2();
    private Vector2 cameraStart = new Vector2();
    private Vector2 cameraEnd = new Vector2();

    // Start is called before the first frame update
    void Start()
    {
        zombieContainer = zombieSpawner.transform;
        fadeColour = fadeSprite.color;
        ResetWorld();
    }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        demoTime += Time.deltaTime;
        if (zombieContainer.childCount == 0 || demoTime >= demoDuration)
            ResetWorld();

        if (demoTime >= demoDuration / 2)
            fadeColour.a = Mathf.Lerp(0, 1, Mathf.Clamp((demoTime - demoDuration + halfFadeDuration) * (1 / halfFadeDuration), 0, 1));
        else
            fadeColour.a = Mathf.Lerp(0, 1, Mathf.Clamp(1 - demoTime * (1 / halfFadeDuration), 0, 1));
        fadeSprite.color = fadeColour;

        //camera movement
        Camera.main.transform.position = (Vector3)playerAI.transform.position + Vector3.Lerp(cameraStart, cameraEnd, demoTime / demoDuration) + new Vector3(0,0,-10);

    }

    public void ResetWorld()
    {
        //reset timer
        demoTime = 0;

        //setup camera movement
        Vector2 playerPos = Random.insideUnitCircle * 10;
        playerAI.transform.position = playerPos;
        cameraStart = Random.insideUnitCircle * 6;
        Camera.main.transform.position = cameraStart + playerPos;
        Vector2 camToPlayer = playerPos - (Vector2)Camera.main.transform.position;
        cameraDir = (Quaternion.Euler(0, 0, Random.Range(-15, 15)) * camToPlayer).normalized;
        cameraEnd = cameraStart + cameraDir * camToPlayer.magnitude;
        Debug.DrawLine(cameraStart + playerPos, cameraEnd + playerPos, Color.cyan, 5);

        //cleanup things from last demo
        for (int i = 0; i < zombieContainer.childCount; i++)
            Destroy(zombieContainer.GetChild(i).gameObject);
        zombieSpawner.SpawnZombies(Random.insideUnitCircle * 10, 10);
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Projectile"))
        {
            Destroy(go);
        }

        UIController.Instance.RemoveWeapon(1);
        UIController.Instance.RemoveWeapon(2);
        UIController.Instance.AddAmmo(-1);
    }
}
