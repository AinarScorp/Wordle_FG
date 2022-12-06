using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputController : MonoBehaviour
{

    [SerializeField] GameManager gameManager;
    [SerializeField] KeyCode deleteLetterKey = KeyCode.Backspace;
    [SerializeField] [Range(0, 4)] int removeAt;
    [SerializeField] KeyCode guessWordKey = KeyCode.Return;
    [SerializeField] float erasingPace = 2;
    Coroutine erasingLetters;
    void Awake()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        }
        gameManager.GameIsOver += gameIsWon => Destroy(this);

        
    }

    void Update()
    {
        HandleRemovingLetters();
        
        HandleKeyboardInputs();
        if (Input.GetKeyDown(guessWordKey))
        {
            gameManager.GuessWord();
        }
    }

    void HandleKeyboardInputs()
    {
        if (Input.anyKeyDown)
        {
            TryAddLetter();
        }
    }

    void TryAddLetter()
    {
        foreach (var inputChar in Input.inputString)
        {
            if (Char.IsLetter(inputChar))
            {
                gameManager.AddLetter(inputChar);
            }
        }
    }

    void HandleRemovingLetters()
    {
        if (Input.GetKeyDown(deleteLetterKey))
        {
            erasingLetters = StartCoroutine(EraseLetters());
        }

        if (Input.GetKeyUp(deleteLetterKey) && erasingLetters!=null)
        {
            StopCoroutine(erasingLetters);
        }
    }

    IEnumerator EraseLetters()
    { 
        WaitForSeconds wait = new WaitForSeconds(erasingPace);
        while (true)
        {
            gameManager.RemoveLetter();
            yield return wait;
        }
    }
}
