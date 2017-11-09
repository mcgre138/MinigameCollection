using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour {

    public int currentPlayer = 1;

    public GameMode currentGameMode;

    static public int hasWon = 0; //0 = lost, 1 = Won
    static public bool gameOver = false;

    static public void HasWonGame(bool won)
    {
        if (won) hasWon = 100;
        else hasWon = 0;
    }

}
