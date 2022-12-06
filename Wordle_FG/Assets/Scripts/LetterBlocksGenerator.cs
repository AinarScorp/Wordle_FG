using System.Collections.Generic;
using UnityEngine;

public class LetterBlocksGenerator : GridGenerator<LetterBlock>
{

    public List<LetterBlock> LetterBlocks => generatedComponents;

    [ContextMenu("Generate Grid")]
    protected override void GenerateGrid()
    {
        base.GenerateGrid();
    }
    [ContextMenu("Remove Grid")]
    protected override void RemoveGrid()
    {
        base.RemoveGrid();
    }
}
