using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Rewired;

public class ButtonPrompt : MonoBehaviour {

    public Sprite[] buttons;
    private Sprite targetButton;

    public Image icon;

    public UnityEvent OnCorrect = new UnityEvent();
    public UnityEvent OnIncorrect = new UnityEvent();

    public bool isCorrect = false;

    [Header("Input")]
    [Range(1,4)] public int playerNumber = 1;
    private int playerIndex = 0;
    public string[] targetActions;

    private string currentTarget;

    void Awake()
    {
        playerIndex = playerNumber - 1;
    }

    void OnEnable()
    {
        RandomizeButton();
    }

    void Update()
    {
        if (ReInput.players.Players[playerIndex].GetAnyButtonDown() && !ReInput.players.Players[playerIndex].GetButtonDown(currentTarget))
        {
            OnIncorrect.Invoke();
        }
        else if (ReInput.players.Players[playerIndex].GetButtonDown(currentTarget))
        {
            OnCorrect.Invoke();
        }
    }

    public void RandomizeButton()
    {
        int rnd = Random.Range(0, buttons.Length);

        targetButton = buttons[rnd];

        icon.sprite = targetButton;

        currentTarget = targetActions[rnd];
    }
}
