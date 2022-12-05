using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WordDictionary : MonoBehaviour
{
    const int LETTER_COUNT = 5;
    
    [SerializeField] TextAsset completeListOfWords;
    [SerializeField] TextAsset easyListOfWords; //Not used for now
    
    string[] allWords; // use for searching by index
    HashSet<string> wordList = new HashSet<string>(); // use for checking contains

    public int WordsCount => allWords.Length;
    void Awake()
    {
        CreateWordList();
    }
    
    void CreateWordList()
    {
        string fullText = completeListOfWords.text;
        string[] allLines = fullText.Split("\r\n");

        allWords = allLines;
        wordList.UnionWith(allLines);
    }

    public string GetWordAtIndex(int index)
    {
        if (index<0 || index>= allWords.Length)
        {
            Debug.LogError("The index is out of the array of Words");
            return "";
        }
        return allWords[index];
    }
    public bool WordExists(string word)
    {
        if (word.Length != LETTER_COUNT)
        {
            return false;
        }
        return wordList.Contains(word);
    }
}

[Serializable]
public class GoalWord
{
    const int LETTER_COUNT = 5;
    [SerializeField] Letter[] letters = new Letter[LETTER_COUNT];
    [SerializeField] string theWord;


    public GoalWord(string chosenWord)
    {
        if (chosenWord.Length != LETTER_COUNT)
        {
            Debug.LogError("wrong amount of Letters in the word");
            return;
        }

        theWord = chosenWord;
        for (int l = 0; l < LETTER_COUNT; l++)
        {
            letters[l] = new Letter(theWord[l]);
        }
    }

    public void ResetLetters()
    {
        for (int i = 0; i < letters.Length; i++)
        {
            letters[i].isDiscovered = false;
        }

    }
    public Letter[] Letters => letters;
}

[Serializable]
public struct Letter
{
    public bool isDiscovered;
    char character;


    public Letter(char character)
    {
        isDiscovered = false;
        this.character = character;
    }

    public char Character => character;
}