using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WordDictionary : MonoBehaviour
{
    const int LETTER_COUNT = 5;
    readonly HashSet<string> completeWordList = new HashSet<string>(); // use for checking contains
    
    [SerializeField] TextAsset completeListOfWords;
    [SerializeField] TextAsset easyListOfWords; //Not used for now
    
    string[] allWordsCompleteList; // use for searching by index
    string[] allWordsEasyList; // use for searching by index

    public int WordsCountCompleteList => allWordsCompleteList.Length;
    public int WordsCountEasyList => allWordsEasyList.Length;
    void Awake()
    {
        CreateWordLists();
    }
    
    void CreateWordLists()
    {
        string fullTextFromComplete = completeListOfWords.text;
        string[] allLinesFromComplete = fullTextFromComplete.Split("\r\n");

        allWordsCompleteList = allLinesFromComplete;
        completeWordList.UnionWith(allLinesFromComplete);
        
        string fullTextFromEasy = easyListOfWords.text;
        string[] allLinesFromEasy = fullTextFromEasy.Split("\r\n");
        allWordsEasyList = allLinesFromEasy;
    }

    public string GetWordAtIndex(int index, bool takeFromEasyList = true)
    {
        string[] listToTakeFrom = takeFromEasyList ? allWordsEasyList : allWordsCompleteList;
        if (index<0 || index>= listToTakeFrom.Length)
        {
            Debug.LogError("The index is out of the array of Words");
            return "";
        }
        return listToTakeFrom[index];
    }
    public bool WordExists(string word)
    {
        if (word.Length != LETTER_COUNT)
        {
            return false;
        }
        return completeWordList.Contains(word);
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

    public string TheWord => theWord;
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