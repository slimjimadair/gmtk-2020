using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Objects
    GameObject player;
    public GameObject abilityUI;
    GameObject mainCamera;

    // Ability Handling
    int[] abilityCounts;

    // Control Variables
    bool gameIsPaused = false;

    void Start()
    {
        // Get Objects
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        // Set starting abilities
        abilityCounts = new int[] { 1, 0, 0, 0 };
        player.GetComponent<PlayerController>().UpdateAbilities(abilityCounts);
        player.GetComponent<PlayerController>().Restart();
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
        Restart();
    }

    public void Restart()
    {
        player.GetComponent<PlayerController>().Restart();
        mainCamera.GetComponent<CameraFollow>().Restart();
    }
}
