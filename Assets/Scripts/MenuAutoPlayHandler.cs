using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAutoPlayHandler : MonoBehaviour
{
    public BasicCharacterController playerController;
    public PlayerAI playerAI;
    public ZombieSpawner zombieSpawner;
    
    Transform zombieContainer;
    float demoTime = 0;
    float demoDuration = 10;

    private Vector2 cameraDir = new Vector2();
    private Vector2 cameraStart = new Vector2();
    private Vector2 cameraEnd = new Vector2();
    // Start is called before the first frame update
    void Start()
    {
        zombieContainer = zombieSpawner.transform;
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

        //camera movement
        Vector2 playerPos = playerAI.transform.position;
        Vector2 cameraPos = Camera.main.transform.position;
        //float offset = (playerPos - cameraPos).magnitude;
        //if (offset > Camera.main.rect.width)
        Camera.main.transform.position = (Vector3)playerPos + Vector3.Lerp(cameraStart, cameraEnd, demoTime / demoDuration) + new Vector3(0,0,-10);


    }

    void ResetWorld()
    {
        demoTime = 0;
        Vector2 playerPos = Random.insideUnitCircle * 10;
        playerAI.transform.position = playerPos;
        cameraStart = Random.insideUnitCircle * 6;
        Camera.main.transform.position = cameraStart + playerPos;
        Vector2 camToPlayer = playerPos - (Vector2)Camera.main.transform.position;
        cameraDir = (Quaternion.Euler(0, 0, Random.Range(-15, 15)) * camToPlayer).normalized;
        cameraEnd = cameraStart + cameraDir * camToPlayer.magnitude;
        Debug.DrawLine(cameraStart + playerPos, cameraEnd + playerPos, Color.cyan, 5);
        for (int i = 0; i < zombieContainer.childCount; i++)
            Destroy(zombieContainer.GetChild(i).gameObject);
        zombieSpawner.SpawnZombies(Random.insideUnitCircle * 10, 10);
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Projectile"))
        {
            Destroy(go);
        }
    }
}
