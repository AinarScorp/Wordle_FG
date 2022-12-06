using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardKeyGenerator : GridGenerator<KeyboardKey>
{
    [SerializeField] char[] lettersInOrder;
    [ContextMenu("Generate Grid")]

    protected override void GenerateGrid()
    {
        base.GenerateGrid();
        for (int i = 0; i < generatedComponents.Count; i++)
        {
            generatedComponents[i].AssignLetter(lettersInOrder[i]);
        }
    }
    [ContextMenu("Remove Grid")]
    protected override void RemoveGrid()
    {
        base.RemoveGrid();
    }
}
