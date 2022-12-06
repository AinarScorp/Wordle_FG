using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuessVisualisation : MonoBehaviour
{
    [SerializeField] ColorFromState colorFromState;
    void Start()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.LetterChecked += ChangeLetterBlockColor;

    }

    void ChangeLetterBlockColor(LetterBlock letterBlock, LetterState letterState)
    {
        Color appropriateColor = colorFromState.GetColorFromState(letterState);
        letterBlock.GetUpdatedColor(appropriateColor);
    }
}
