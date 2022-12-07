using System;
using UnityEngine;
public class KeyboardKey : MonoBehaviour
{
    [SerializeField] ColorFromState colorFromState;

    [SerializeField]GameVisuals gameVisuals;
    [SerializeField,HideInInspector] char thisLetter;

    bool gameHasStarted;
    
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
                gameVisuals.PlayPressEffect();
            }
        };
        gameManager.GameHasStarted += () => gameHasStarted = true;
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
        gameVisuals.PlayUpdateColor(newColor, ()=> gameVisuals.UpdateText(Color.white));
    }


    public void AssignLetter(char newChar)
    {
        thisLetter = Char.ToUpper(newChar);
        gameVisuals.UpdateText(Color.black,thisLetter.ToString());
    }
    void OnMouseDown()
    {
        if (gameHasStarted)
        {
            gameManager.AddLetter(thisLetter);
        }
    }

    void OnMouseEnter()
    {
        if (gameHasStarted)
        {
            gameVisuals.PlayIncreaseScale();
        }
    }

    void OnMouseExit()
    {
        if (gameHasStarted)
        {
            gameVisuals.PlayDescreaseScale();
        }
    }
}
