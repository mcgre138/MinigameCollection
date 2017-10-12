using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPrompt : MonoBehaviour {

    public Sprite[] buttons;
    private Sprite targetButton;

    public Image icon;

    void OnEnable()
    {
        int rnd = Random.Range(0, buttons.Length);

        targetButton = buttons[rnd];

        icon.sprite = targetButton;
    }
}
