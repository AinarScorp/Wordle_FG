using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackSpaceKey : MonoBehaviour
{
    KeyboardKeyVisuals keyboardKeyVisuals;
    GameManager gameManager;

    void Awake()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        gameManager.GameIsOver += gameIsWon => Destroy(this);
        gameManager.LetterRemoved += (row, pos) => keyboardKeyVisuals.PlayPressEffect();
        keyboardKeyVisuals = GetComponent<KeyboardKeyVisuals>();
    }

    void OnMouseDown()
    {
        gameManager.RemoveLetter();
    }
    void OnMouseEnter()
    {
        keyboardKeyVisuals.PlayOnHoverEnter();
    }

    void OnMouseExit()
    {
        keyboardKeyVisuals.PlayOnHoverExit();
    }
}
