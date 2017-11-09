using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour {

    [Range(1, 30)] public float amountOfTime = 8f;
    protected bool hasWon = false;

    protected float timeElasped = 0;
    protected float percentageTimeLeft;

    protected void Awake()
    {
        InitializeGameMode();
    }
    
    protected void InitializeGameMode()
    {
        StartCoroutine(Timer());
    }

    protected IEnumerator Timer()
    {
        while(timeElasped < amountOfTime)
        {
            timeElasped += Time.deltaTime;
            percentageTimeLeft = 1 - timeElasped / amountOfTime;
            yield return new WaitForEndOfFrame();
        }
        
        WinState(false);
    }

    public virtual void WinState()
    {
        WinState(hasWon);
    }

    public virtual void WinState(bool won)
    {
        hasWon = won;
        GameData.HasWonGame(hasWon);
        GameData.gameOver = true;
        Debug.Log("Game Over - Has Won: " + hasWon);
    }

}
