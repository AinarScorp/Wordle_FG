using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] LetterBlocksGenerator letterBlocksGenerator;
    [SerializeField] float spinInterval = 1.0f;
    LetterBlock[][] rowOfLetters;

    public LetterBlock[][] RowOfLetters => rowOfLetters;
    public Vector2Int GridSize => letterBlocksGenerator.GridSize;


    void Awake()
    {
        if (letterBlocksGenerator ==null)
        {
            letterBlocksGenerator = FindObjectOfType<LetterBlocksGenerator>();
        }
        PopulateRowOfLetters();
    }

    void Start()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.WordGuessed += SpinRow;
        
    }

    void SpinRow(int rowIndex)
    {
        StartCoroutine(SpinAllLettersInARow(rowIndex));
    }

    IEnumerator SpinAllLettersInARow(int rowIndex)
    {
        foreach (var letterBlock in rowOfLetters[rowIndex])
        {
            letterBlock.Spin();
            yield return new WaitForSeconds(spinInterval);
        }
    }
    void PopulateRowOfLetters()
    {
        //populate jagged Array;
        int index = 0;
        rowOfLetters = new LetterBlock[GridSize.y][];
        for (int y = 0; y < GridSize.y; y++)
        {
            rowOfLetters[y] = new LetterBlock[GridSize.x];
            for (int x = 0; x < GridSize.x; x++)
            {
                rowOfLetters[y][x] = letterBlocksGenerator.LetterBlocks[index];
                rowOfLetters[y][x].GetInfoLetter(y,x);
                index++;
            }
        }
    }
}
