using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class InputVisualisation : MonoBehaviour
{
    [SerializeField] GridManager gridManager;

    const char EMPTY_LETTER = ' ';


    void Awake()
    {
        if (gridManager == null)
        {
            gridManager = FindObjectOfType<GridManager>();
        }
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.LetterAdded += AddLetter;
        gameManager.LetterRemoved += RemoveLetter;
    }
    

    void AddLetter(int rowIndex,int letterPosition,Char letter)
    {
        LetterBlock letterBlock = gridManager.RowOfLetters[rowIndex][letterPosition];
        UpdateLetterDisplay(letterBlock,letter);
    }

    void RemoveLetter(int rowIndex,int letterPosition)
    {
        LetterBlock letterBlock = gridManager.RowOfLetters[rowIndex][letterPosition];
        UpdateLetterDisplay(letterBlock,EMPTY_LETTER);

    }

    void UpdateLetterDisplay(LetterBlock letterBlock, char newLetter)
    {
        letterBlock.UpdateLetter(newLetter);
    }





}
