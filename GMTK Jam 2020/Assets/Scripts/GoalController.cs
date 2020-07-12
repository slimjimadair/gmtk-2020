using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    GameObject game;
    public GameObject messageUI;

    private void Start()
    {
        game = GameObject.FindGameObjectWithTag("GameController");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            messageUI.SetActive(true);
            messageUI.GetComponent<MessageUI>().SetMessage("YOU WON!", "LEVEL COMPLETE", "THE PRINCESS MUST BE IN ANOTHER CASTLE", null);
        }
    }
}
