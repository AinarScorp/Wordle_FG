using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CorrectWordRevealer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI displayCorrectWord;
    [SerializeField] TextMeshProUGUI displayGameResult;
    [SerializeField] string text = "Correct Word was ";
    [SerializeField] string loseText = "You lost...";
    [SerializeField] string winText = "You WON!!!";
    GameLoopManager gameLoopManager;
    void Start()
    {        
        gameLoopManager = FindObjectOfType<GameLoopManager>();


        UpdateCorrectWord();
        UpdateGameResultText();
    }

    void UpdateGameResultText()
    {
        displayGameResult.text = gameLoopManager.GameWasWon ? winText : loseText;
    }

    void UpdateCorrectWord()
    {
        text += gameLoopManager.TheWord;
        displayCorrectWord.text = text;
    }
    
}
