using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackSpaceKey : MonoBehaviour
{
    GameManager gameManager;

    void Awake()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        gameManager.GameIsOver += gameIsWon => Destroy(this);
    }

    void OnMouseDown()
    {
        gameManager.RemoveLetter();
    }
}
