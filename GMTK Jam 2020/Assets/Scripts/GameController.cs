using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Objects
    GameObject player;
    public GameObject UIHolder;
    public GameObject abilityUI;
    public GameObject messageUI;
    GameObject mainCamera;

    public AudioSource pickupAudio;
    

    // Ability Handling
    int[] abilityCounts;

    // UI Lists
    string[] uiLabels = new string[] { "JUMP", "AIR JUMP", "SPRINT", "DASH" };
    string[] uiInstructions = new string[]
    {
        "PRESS UP TO JUMP",
        "PRESS UP IN AIR TO JUMP",
        "HOLD SHIFT TO SPRINT",
        "PRESS SPACE TO DASH FORWARD",
    };
    public Sprite[] uiSpritesLight;
    string[][] uiComments;

    void Start()
    {
        // Set UI Comments
        uiComments = new string[4][];
        uiComments[0] = new string[] { // Jump Comments
            "HOW IS A JUMP IN A BOX?",
        };
        uiComments[1] = new string[] { }; // Air Jump Comments
        uiComments[2] = new string[] { }; // Air Jump Comments
        uiComments[3] = new string[] { }; // Air Jump Comments

        // Get Objects
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        // Set starting abilities
        abilityCounts = new int[] { 1, 0, 0, 0 };
        player.GetComponent<PlayerController>().UpdateAbilities(abilityCounts);

        // Turn on UI
        UIHolder.SetActive(true);

        // Intro screen
        messageUI.SetActive(true);
        messageUI.GetComponent<MessageUI>().SetMessage("CONTROLVANIA", "FIND YOUR CONTROLS", "PRESS UP TO JUMP AND COLLECT THE CHESTS", null);
        Invoke("Restart", 2.0f);
    }

    void Update()
    {
        // ESC to exit game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Backspace to restart
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Restart();
        }
    }

    public void AddAbility(int abilityID)
    {
        pickupAudio.Play();

        abilityCounts[abilityID] += 1;
        player.GetComponent<PlayerController>().UpdateAbilities(abilityCounts);
        messageUI.SetActive(true);
        string uiComment = "";
        if (uiComments[abilityID].Length > 0)
        {
            uiComment = uiComments[abilityID][Random.Range(0, uiComments[abilityID].Length)];
        }
        messageUI.GetComponent<MessageUI>().SetMessage(uiLabels[abilityID], uiInstructions[abilityID], uiComment, uiSpritesLight[abilityID]);
        Invoke("Restart", 2.0f);
    }

    public void Restart()
    {
        player.GetComponent<PlayerController>().Restart();
        mainCamera.GetComponent<CameraFollow>().Restart();
        messageUI.SetActive(false);
    }

    public void Die(string message = "", float showTime = 1.0f)
    {
        messageUI.SetActive(true);
        messageUI.GetComponent<MessageUI>().SetMessage("YOU DIED", message, "", null, "red");
        Invoke("Restart", showTime);
    }
}
