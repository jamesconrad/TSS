using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedWeapon : MonoBehaviour
{
    private float t;
    private int id;

    // Start is called before the first frame update
    void Start()
    {
        id = Random.Range(0, WeaponLoader.Instance.NumLoadedGuns());
        id = 1;
        GetComponent<SpriteRenderer>().sprite = WeaponLoader.Instance.Load(id).sprite;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        float s = Mathf.Sin(t);
        transform.localRotation = Quaternion.Euler(0, 0, 45 * t);
        transform.localScale = new Vector2(1.5f, 1.5f) + (new Vector2(0.25f, 0.25f) * Mathf.Abs(s));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            print(WeaponLoader.Instance.Load(id).name);
            if (UIController.Instance.AddWeapon(WeaponLoader.Instance.Load(id)))
                Destroy(gameObject);
        }
    }
}
