using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubmitKey : MonoBehaviour
{
    enum SubmitState
    {
        Ready, notReady, noSuchWord
    }
    [SerializeField] Color noSuchWordColor = Color.red;
    [SerializeField] Color readyToSubmitColor = Color.blue;
    [SerializeField] Color notReadyToSubmitColor = Color.gray;
    [SerializeField] string noSuchWordText= "Not a \nword";
    [SerializeField] string submitText = "Submit";
    
    [SerializeField] TextMeshPro displayText;
    SubmitState submitState = SubmitState.notReady;
    GameManager gameManager;

    MeshRenderer meshRenderer;
    Material material;

    void Awake()
    {
        if (displayText == null)
        {
            displayText = GetComponentInChildren<TextMeshPro>();
        }
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        AssignMaterial();
    }

    void AssignMaterial()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        material = meshRenderer.material;
        meshRenderer.material = material;
    }

    void Start()
    {
        gameManager.LetterRemoved += (row, pos) => ResetState();
        gameManager.WordGuessed+= row => ResetState();
        
        gameManager.LetterAdded += (row, pos, letter) => ChangeColorCorrect(pos); 

        gameManager.GameIsOver += gameIsWon => Destroy(this);

        ResetState();
    }

    void ResetState()
    {
        submitState = SubmitState.notReady;
        UpdateColor();
    }
    void UpdateColor()
    {
        switch (submitState)
        {
            case SubmitState.Ready:
                material.color = readyToSubmitColor;
                break;
            case SubmitState.notReady:
                material.color = notReadyToSubmitColor;
                break;
            case SubmitState.noSuchWord:
                material.color = noSuchWordColor;
                break;
        }
        UpdateText();
    }

    void UpdateText()
    {
        bool submitStateIsRed = submitState == SubmitState.noSuchWord;
        displayText.text = submitStateIsRed ? noSuchWordText : submitText;
    }

    void ChangeColorCorrect(int pos)
    {
        if (pos !=gameManager.WordLength - 1)
        {
            return;
        }

        submitState = gameManager.FilledWordIsValid() ? SubmitState.Ready : SubmitState.noSuchWord;
        UpdateColor();
    }

    void OnMouseDown()
    {
        if (submitState != SubmitState.Ready)
        {
            return;
        }
        gameManager.GuessWord();
    }
    
}

