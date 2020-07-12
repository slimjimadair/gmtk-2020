using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;

public class MessageUI : MonoBehaviour
{
    public void SetMessage(string mainMessage, string instruction = "", string subMessage = "", Sprite image = null, string messageColor = "white")
    {
        // Main Message
        GameObject mainText = transform.GetChild(1).gameObject;
        Text txt = mainText.GetComponent<Text>();
        txt.text = mainMessage;
        if (messageColor == "red") {
            txt.color = new Color32(229, 110, 86, 255);
        } else {
            txt.color = new Color32(245, 240, 234, 255); // WHITE
        }

        // Instruction
        GameObject instText = transform.GetChild(2).gameObject;
        txt = instText.GetComponent<Text>();
        txt.text = instruction;

        // Sub Message
        GameObject subText = transform.GetChild(3).gameObject;
        txt = subText.GetComponent<Text>();
        txt.text = subMessage;

        // Sub Message
        GameObject icon = transform.GetChild(4).gameObject;
        Image img = icon.GetComponent<Image>();
        img.sprite = image;
        img.color = (image == null) ? new Color32(255, 255, 255, 0) : new Color32(255, 255, 255, 255);
    }
}
