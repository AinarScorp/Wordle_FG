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
    [Header("Colors")]
    [SerializeField] Color noSuchWordColor = Color.red;
    [SerializeField] Color readyToSubmitColor = Color.blue;
    [SerializeField] Color notReadyToSubmitColor = Color.gray;
    [Header("Texts")]
    [SerializeField][TextArea] string noSuchWordText= "Not a \nword";
    [SerializeField] string submitText = "Submit";

    bool gameHasStarted;
    
    GameVisuals gameVisuals;
    
    SubmitState submitState = SubmitState.notReady;
    GameManager gameManager;

    
    
    void Awake()
    {
        gameVisuals = GetComponent<GameVisuals>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }



    void Start()
    {
        SubscribeToEvents();
        UpdateText();
    }

    void SubscribeToEvents()
    {
        gameManager.LetterRemoved += (row, pos) => ResetState();
        gameManager.WordGuessed += row =>
        {
            gameVisuals.PlayPressEffect();
            ResetState();
        };
        gameManager.LetterAdded += (row, pos, letter) => ChangeColorCorrect(pos);
        gameManager.GameIsOver += gameIsWon => Destroy(this);
        gameManager.GameHasStarted += () => gameHasStarted = true;
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
        gameVisuals.PlayUpdateColor(newColor, UpdateText);
    }

    void UpdateText()
    {
        bool submitStateIsRed = submitState == SubmitState.noSuchWord;
        string newText = submitStateIsRed ? noSuchWordText : submitText;
        gameVisuals.UpdateText(newText);
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
        if (!gameHasStarted || !ButtonReady()) return;

        gameManager.GuessWord();
    }
    void OnMouseEnter()
    {
        if (!gameHasStarted ||!ButtonReady()) return;

        gameVisuals.PlayIncreaseScale();
    }

    void OnMouseExit()
    {
        if (!gameHasStarted ||!ButtonReady()) return;
        
        gameVisuals.PlayDescreaseScale();
    }

    bool ButtonReady()
    {
        return submitState == SubmitState.Ready;
    }
}

