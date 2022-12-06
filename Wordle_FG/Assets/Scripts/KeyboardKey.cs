using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyboardKey : MonoBehaviour
{
    [SerializeField] ColorFromState colorFromState;

    [SerializeField] TextMeshPro letterDisplay;
    [SerializeField,HideInInspector] char thisLetter;
    LetterState currentLetterState = LetterState.NotTested;

    GameManager gameManager;
    MeshRenderer meshRenderer;
    Material material;

    void Awake()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        gameManager.LetterChecked += UpdateLetterState;
        gameManager.GameIsOver += gameIsWon => Destroy(this);
    }

    void UpdateLetterState(LetterBlock letterBlock, LetterState letterState)
    {
        if (letterBlock.ChosenLetter != thisLetter)
        {
            return;
        }

        if (currentLetterState >= letterState)
        {
            return;
        }

        currentLetterState = letterState;
        UpdateColor();
    }
    void UpdateColor()
    {
        if (meshRenderer == null)
        {
            meshRenderer = GetComponentInChildren<MeshRenderer>();
        }

        if (material == null)
        {
            material = meshRenderer.material;
        }
        material.color = colorFromState.GetColorFromState(currentLetterState);
        meshRenderer.material =material;
    }

    public void AssignLetter(char newChar)
    {
        thisLetter = Char.ToUpper(newChar);
        letterDisplay.text = thisLetter.ToString();
    }
    void OnMouseDown()
    {
        gameManager.AddLetter(thisLetter);
    }
}
