using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] GoalWord goalWord;
    [SerializeField] bool easyMode = true;
    
    [SerializeField] WordDictionary wordDictionary;
    [SerializeField] GridManager gridManager;

    int currentRow;
    int currentLetter;

    public event Action<bool> GameIsOver; //isWon
    public event Action<LetterBlock, LetterState> LetterChecked;
    public event Action<int, int, Char> LetterAdded; //currentRow, currentLetter, newChar
    public event Action<int, int> LetterRemoved; //currentRow, currentLetter
    public event Action<int> WordGuessed; //currentRow

    public int WordLength => gridManager.GridSize.x;
    int overFinalRow => gridManager.GridSize.y;

    void Awake()
    {
        if (wordDictionary == null)
        {
            wordDictionary = FindObjectOfType<WordDictionary>();
        }

        if (gridManager == null)
        {
            gridManager = FindObjectOfType<GridManager>();
        }
    }


    void Start()
    {
        GetRandomWord();
    }

    public void AddLetter(Char letter)
    {
        if (currentLetter >= WordLength)
        {
            return;
        }

        LetterAdded?.Invoke(currentRow, currentLetter, letter);
        currentLetter++;
    }

    public void RemoveLetter()
    {
        if (currentLetter <= 0)
        {
            return;
        }

        currentLetter--;
        LetterRemoved?.Invoke(currentRow, currentLetter);
    }

    public void GuessWord()
    {
        var word = GetGuessedWord();
        if (!WordIsValid(word)) return;

        if (word == goalWord.TheWord)
        {
            ConcludeWinningGame();
            return;
        }

        CheckWordByLetter();
        currentRow++;
        currentLetter = 0;
        if (currentRow == overFinalRow)
        {
            //game is lost
            GameIsOver?.Invoke(false);
        }
    }
    void ConcludeWinningGame()
    {
        foreach (var letterBlock in gridManager.RowOfLetters[currentRow])
        {
            LetterChecked?.Invoke(letterBlock, LetterState.FoundCorrectPosition);
        }
        WordGuessed?.Invoke(currentRow);
        GameIsOver?.Invoke(true);
    }
    void CheckWordByLetter()
    {
        goalWord.ResetLetters();
        foreach (var letterBlock in gridManager.RowOfLetters[currentRow])
        {
            CheckLetter(letterBlock);
        }
        WordGuessed?.Invoke(currentRow);
    }

    void CheckLetter(LetterBlock guessingLetter)
    {
        bool letterExists = false;
        bool correctPos = false;
        int indexOfFirstDiscoveredLetter = -1;
        for (int i = 0; i < goalWord.Letters.Length; i++)
        {
            Letter letterToCheck = goalWord.Letters[i];

            if (letterToCheck.isDiscovered)
            {
                continue;
            }

            if (letterToCheck.Character == guessingLetter.ChosenLetter)
            {
                if (!letterExists)
                {
                    indexOfFirstDiscoveredLetter = i;
                }
                letterExists = true;
                correctPos = i == guessingLetter.LetterPosition;
                if (correctPos)
                {
                    indexOfFirstDiscoveredLetter = i;
                    break;
                }
            }
        }

        if (indexOfFirstDiscoveredLetter !=-1)
        {
            goalWord.Letters[indexOfFirstDiscoveredLetter].isDiscovered = true;
        }

        LetterState letterState;
        letterState = correctPos ? LetterState.FoundCorrectPosition : letterExists ? LetterState.LetterExistsButWrongPos : LetterState.NoSuchLetter;
        LetterChecked?.Invoke(guessingLetter, letterState);
    }

    public bool FilledWordIsValid()
    {
        string word = GetGuessedWord();
        return WordIsValid(word);
    }
    bool WordIsValid(string word)
    {
        bool isValidWord = wordDictionary.WordExists(word);
        return isValidWord;
    }

    string GetGuessedWord()
    {
        string word = "";
        foreach (var letter in gridManager.RowOfLetters[currentRow])
        {
            word += letter.ChosenLetter;
        }
        return word;
    }

    
    [ContextMenu("Get random Word")]
    void GetRandomWord()
    {
        int wordCount = easyMode ? wordDictionary.WordsCountEasyList : wordDictionary.WordsCountCompleteList;
        int randomNumber = Random.Range(0, wordCount);
        string randomWord = wordDictionary.GetWordAtIndex(randomNumber,easyMode);
        goalWord = new GoalWord(randomWord);
    }

    public void SetDifficulty(bool isEasy)
    {
        easyMode = isEasy;
    }
}