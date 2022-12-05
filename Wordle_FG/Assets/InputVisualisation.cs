using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class InputVisualisation : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI displayWord;

    string currentDisplay = "-----";

    public string CurrentDisplay => currentDisplay;

    void Start()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.LetterAdded += AddLetter;
        gameManager.RemovedLetterAt += RemoveLetter;
        UpdateDisplay();
    }
    void AddLetter(Letter letter)
    {
        AddLetter(letter.Character);
    }
    void AddLetter(Char letter)
    {
        currentDisplay += letter;
        UpdateDisplay();
    }

    void RemoveLetter(int index)
    {
        index = currentDisplay.Length - 1;
        // if (currentDisplay.Length < 0 ||currentDisplay.Length >= index)
        // {
        //     return;
        // }
        StringBuilder temp = new StringBuilder(currentDisplay);
        temp[index] = '-';
        currentDisplay = temp.ToString();
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        displayWord.text = currentDisplay;

    }


    
    
}
