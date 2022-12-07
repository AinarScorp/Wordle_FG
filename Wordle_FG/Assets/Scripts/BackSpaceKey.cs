using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackSpaceKey : MonoBehaviour
{
    GameVisuals gameVisuals;
    GameManager gameManager;

    bool gameHasStarted;
    void Awake()
    {
        FindAndSubscribeToGameManager();
        gameVisuals = GetComponent<GameVisuals>();
    }

    void FindAndSubscribeToGameManager()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        gameManager.GameIsOver += gameIsWon => Destroy(this);
        gameManager.LetterRemoved += (row, pos) => gameVisuals.PlayPressEffect();
        gameManager.GameHasStarted += () => gameHasStarted = true;
    }


    void OnMouseDown()
    {
        if (gameHasStarted)
        {
            gameManager.RemoveLetter();
            
        }
    }
    void OnMouseEnter()
    {
        if (gameHasStarted)
        {
            gameVisuals.PlayIncreaseScale();
        }
    }

    void OnMouseExit()
    {
        if (gameHasStarted)
        {
            gameVisuals.PlayDescreaseScale();
        }
    }
}
