using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridGenerator<T> : MonoBehaviour where T: Component
{
    [SerializeField]protected float cellSize = 1.0f;
    [SerializeField]protected float distanceOffset = 1.0f;
    [SerializeField]protected Vector2Int gridSize = new Vector2Int(5, 5);
    [SerializeField]protected Vector3 originalPos;
    [SerializeField]protected T componentPrefab;

    [SerializeField]protected List<T> generatedComponents;
    
    public Vector2Int GridSize => gridSize;
    

    protected virtual void GenerateGrid()
    {
        RemoveGrid();

        generatedComponents = new List<T>();
        originalPos = transform.position;
        for (int y = gridSize.y-1; y >= 0; y--)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                Vector3 position = new Vector3(x * cellSize + x*distanceOffset, y * cellSize + y*distanceOffset) + originalPos;
                T letterBlock = Instantiate(componentPrefab, position, Quaternion.identity, this.transform);
                letterBlock.transform.localScale *= cellSize;
                generatedComponents.Add(letterBlock);
            }
        }
    }
    protected virtual void RemoveGrid()
    {
        if (generatedComponents == null)
        {
            return;
        }
        
        foreach (var letterBlock in generatedComponents)
        {
            if (letterBlock ==null)
            {
                break;
            }
            DestroyImmediate(letterBlock.gameObject);
        }
        generatedComponents = null;
        
    }
    
}
