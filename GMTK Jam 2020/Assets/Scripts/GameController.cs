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

    void Start()
    {
        // Get Objects
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        // Set starting abilities
        abilityCounts = new int[] { 1, 0, 0, 0 };
        player.GetComponent<PlayerController>().UpdateAbilities(abilityCounts);

        // Turn on UI
        UIHolder.SetActive(true);
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
        abilityCounts[abilityID] += 1;
        player.GetComponent<PlayerController>().UpdateAbilities(abilityCounts);
        messageUI.SetActive(true);
        messageUI.GetComponent<MessageUI>().SetMessage(uiLabels[abilityID], uiInstructions[abilityID], "", uiSpritesLight[abilityID]);
        Invoke("Restart", 1.0f);
    }

    public void Restart()
    {
        player.GetComponent<PlayerController>().Restart();
        mainCamera.GetComponent<CameraFollow>().Restart();
        messageUI.SetActive(false);
    }

    public void Die()
    {
        messageUI.SetActive(true);
        messageUI.GetComponent<MessageUI>().SetMessage("YOU DIED", "", "", null, "red");
        Invoke("Restart", 1.0f);
    }
}
