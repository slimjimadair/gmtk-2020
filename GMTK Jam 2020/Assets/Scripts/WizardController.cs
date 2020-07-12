using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardController : MonoBehaviour
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
            if (collision.gameObject.GetComponent<PlayerController>().IsDangerous())
            {
                messageUI.SetActive(true);
                messageUI.GetComponent<MessageUI>().SetMessage("YOU WON!", "YOU KILLED THE EVIL WIZARD", "HE DEFINITELY LOOKED EVIL... RIGHT?", null);
                Destroy(this.gameObject);
            }
            else
            {
                game.GetComponent<GameController>().Die("YOU CAN'T JUST RUN INTO ME!", 2.0f);
            }
        }
    }
}
