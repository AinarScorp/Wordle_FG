using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] WordDictionary wordDictionary;
    [SerializeField] InputVisualisation _inputVisualisation; // DELETE ME
    GoalWord goalWord;

    
    public event Action<Char> LetterAdded;
    public event Action<int> RemovedLetterAt;



    void Awake()
    {
        if (wordDictionary == null)
        {
            wordDictionary = FindObjectOfType<WordDictionary>();
        }
    }

    void Start()
    {
        GetRandomWord();
    }

    public void AddLetter(Char letter)
    {
        string currentText = _inputVisualisation.CurrentDisplay;
        if (_inputVisualisation.CurrentDisplay.Length>=5)
        {
            return;
        }
        LetterAdded?.Invoke(letter);
    }

    public void RemoveLetterAt(int index)
    {
        RemovedLetterAt?.Invoke(index);
    }
    
    public void GuessWord()
    {
        string word = _inputVisualisation.CurrentDisplay;
        string wordToCheck = word.ToUpper();
        bool validWord = wordDictionary.WordExists(word);
        if (!validWord)
        {
            print("There is no such word");
            return;
        }

        goalWord.ResetLetters();
        foreach (var letter in wordToCheck)
        {
            CheckLetter(letter);
        }
        
    }
    void CheckLetter(char guessingLetter)
    {
        int index = -1;
        bool letterExists = false;
        for (int i = 0; i < goalWord.Letters.Length; i++)
        {
            Letter letterToCheck = goalWord.Letters[i];
            if (letterToCheck.isDiscovered)
            {
                continue;
            }
            if (letterToCheck.Character == Char.ToUpper(guessingLetter))
            {
                goalWord.Letters[i].isDiscovered = true;
                letterExists = true;
                index = i;
                break;
            }
        }

        if (!letterExists)
        {
            return;
        }

        print(index);
    }
    
    [ContextMenu("Get random Word")]
    void GetRandomWord()
    {
        int randomNumber = Random.Range(0, wordDictionary.WordsCount);
        string randomWord = wordDictionary.GetWordAtIndex(randomNumber);
        goalWord = new GoalWord(randomWord);
    }
}
