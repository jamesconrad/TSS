using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActionHandler : MonoBehaviour
{
    public void ButtonStartGame()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerAI>().enabled = false;
        player.GetComponent<BasicCharacterController>().enabled = true;
        MenuAutoPlayHandler.Instance.ResetWorld();
        transform.GetChild(1).gameObject.SetActive(false);
    }

    public void ToggleGameObject(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
