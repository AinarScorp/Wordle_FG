using System;
using UnityEngine;
public class KeyboardKey : MonoBehaviour
{
    [SerializeField] ColorFromState colorFromState;

    [SerializeField]KeyboardKeyVisuals keyboardKeyVisuals;
    [SerializeField,HideInInspector] char thisLetter;

    
    LetterState currentLetterState = LetterState.NotTested;

    GameManager gameManager;

    void Awake()
    {
        SubscribeToGameManager();
    }

    void SubscribeToGameManager()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        gameManager.LetterChecked += UpdateLetterState;
        gameManager.GameIsOver += gameIsWon => Destroy(this);
        gameManager.LetterAdded += (row, pos1, pressedLetter) =>
        {
            if (Char.ToUpper(pressedLetter) == Char.ToUpper(thisLetter))
            {
                keyboardKeyVisuals.PlayPressEffect();
            }
        };
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
        Color newColor = colorFromState.GetColorFromState(currentLetterState);
        keyboardKeyVisuals.PlayUpdateColor(newColor, ()=> keyboardKeyVisuals.UpdateText(Color.white));
    }


    public void AssignLetter(char newChar)
    {
        thisLetter = Char.ToUpper(newChar);
        keyboardKeyVisuals.UpdateText(Color.black,thisLetter.ToString());
    }
    void OnMouseDown()
    {
        gameManager.AddLetter(thisLetter);
    }

    void OnMouseEnter()
    {
        keyboardKeyVisuals.PlayOnHoverEnter();
    }

    void OnMouseExit()
    {
        keyboardKeyVisuals.PlayOnHoverExit();
    }
}
