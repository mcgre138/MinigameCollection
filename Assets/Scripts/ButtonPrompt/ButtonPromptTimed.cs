using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPromptTimed : GameMode {

    private ButtonPrompt prompt;
    public Image timerImage;

    void Start()
    {
        prompt = GetComponent<ButtonPrompt>();
        prompt.OnCorrect.AddListener(WinState);
    }

    void Update()
    {
        timerImage.fillAmount = percentageTimeLeft;
    }

    public override void WinState(bool won)
    {
        hasWon = prompt.isCorrect;
        
        gameObject.SetActive(false);
    }

}
