using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Color From State", menuName = "ScriptableObjects/Color From State")]
public class ColorFromState : ScriptableObject
{
    [SerializeField] Color
        wrongLetter = Color.gray,
        wrongPos = Color.yellow,
        correctLetter = Color.green,
        notTestedLetter =Color.white;
    
    public Color GetColorFromState(LetterState letterState)
    {
        switch (letterState)
        {
            case LetterState.NotTested:
                return notTestedLetter;
            case LetterState.NoSuchLetter:
                return wrongLetter;
            case LetterState.LetterExistsButWrongPos:
                return wrongPos;
            case LetterState.FoundCorrectPosition:
                return correctLetter;
        }

        return default;
    }


}
