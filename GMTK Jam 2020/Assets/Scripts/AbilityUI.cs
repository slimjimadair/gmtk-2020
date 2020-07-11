using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    // Settings
    public Sprite[] uiSprites;
    public GameObject uiAbilityBlock;
    public GameObject uiAbilityPrefab;

    // UI Lists
    string[] uiLabels = new string[] { "JUMP", "AIR JUMP", "SPRINT", "DASH" };

    private void Start()
    {
        int[] uiCounts = new int[] { 1, 0, 1, 0 };
        BuildUI(uiCounts);
    }

    public void BuildUI(int[] uiCounts)
    {
        // Clear previous UI
        foreach (Transform child in uiAbilityBlock.transform) {
            Destroy(child.gameObject);
        }

        // Add new UI
        for (int i = 0; i < uiLabels.Length; i++)
        {
            var newAbility = Instantiate(uiAbilityPrefab);
            newAbility.transform.SetParent(uiAbilityBlock.transform);
            newAbility.transform.localScale = Vector3.one;

            // Set Icon
            GameObject newAbilityImage = newAbility.transform.GetChild(0).gameObject;
            Image img = newAbilityImage.GetComponent<Image>();
            img.sprite = uiSprites[i];
            img.color = new Color32(255, 255, 255, 255);

            // Set Label
            GameObject newAbilityText = newAbility.transform.GetChild(1).gameObject;
            Text txt = newAbilityText.GetComponent<Text>();
            txt.text = uiLabels[i];

            // Set Start Count
            GameObject newAbilityCount = newAbility.transform.GetChild(2).gameObject;
            txt = newAbilityCount.GetComponent<Text>();
            txt.text = uiCounts[i].ToString();

            // Set Activation Status
            newAbility.SetActive(uiCounts[i] > 0 ? true : false);
        }
    }

    public void UpdateUI(int[] uiCounts)
    {
        // Update Counts
        for (int i = 0; i < uiLabels.Length; i++)
        {
            var curAbility = uiAbilityBlock.transform.GetChild(i);

            // Set Current Count
            GameObject curAbilityCount = curAbility.GetChild(2).gameObject;
            Text txt = curAbilityCount.GetComponent<Text>();
            txt.text = uiCounts[i].ToString();

            // Set Inactive if needed
            if (uiCounts[i] < 1)
            {
                GameObject curAbilityHider = curAbility.GetChild(3).gameObject;
                curAbilityHider.SetActive(true);
            }
        }
    }
}
