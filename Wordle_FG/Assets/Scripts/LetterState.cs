using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LetterState
{
    NotTested = 0,
    NoSuchLetter = 1,
    LetterExistsButWrongPos = 2,
    FoundCorrectPosition = 3
}
