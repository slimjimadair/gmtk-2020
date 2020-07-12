using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public int pickupType;
    GameObject game;

    private void Start()
    {
        game = GameObject.FindGameObjectWithTag("GameController");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            game.GetComponent<GameController>().AddAbility(pickupType);
        }
    }
}
