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
    [SerializeField][TextArea] string noSuchWordText= "Not a \nword";
    [SerializeField] string submitText = "Submit";
    
    [SerializeField]KeyboardKeyVisuals keyboardKeyVisuals;
    
    SubmitState submitState = SubmitState.notReady;
    GameManager gameManager;

    
    
    void Awake()
    {
        keyboardKeyVisuals = GetComponent<KeyboardKeyVisuals>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

    }



    void Start()
    {
        gameManager.LetterRemoved += (row, pos) => ResetState();
        gameManager.WordGuessed+= row =>
        {
            keyboardKeyVisuals.PlayPressEffect();
            ResetState();
        };
        
        gameManager.LetterAdded += (row, pos, letter) => ChangeColorCorrect(pos); 

        gameManager.GameIsOver += gameIsWon => Destroy(this);

        ResetState();
    }

    void ResetState()
    {
        if (submitState ==SubmitState.notReady )
        {
            return;
        }
        submitState = SubmitState.notReady;
        UpdateColor();
    }
    void UpdateColor()
    {
        Color newColor = Color.white;
        switch (submitState)
        {
            case SubmitState.Ready:
                newColor = readyToSubmitColor;
                break;
            case SubmitState.notReady:
                newColor = notReadyToSubmitColor;
                break;
            case SubmitState.noSuchWord:
                newColor = noSuchWordColor;
                break;
        }
        keyboardKeyVisuals.PlayUpdateColor(newColor, UpdateText);
    }

    void UpdateText()
    {
        bool submitStateIsRed = submitState == SubmitState.noSuchWord;
        string newText = submitStateIsRed ? noSuchWordText : submitText;
        keyboardKeyVisuals.UpdateText(newText);
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
        if (!ButtonReady()) return;

        gameManager.GuessWord();
    }
    void OnMouseEnter()
    {
        if (!ButtonReady()) return;

        keyboardKeyVisuals.PlayOnHoverEnter();
    }

    void OnMouseExit()
    {
        if (!ButtonReady()) return;
        
        keyboardKeyVisuals.PlayOnHoverExit();
    }

    bool ButtonReady()
    {
        return submitState == SubmitState.Ready;
    }
}

