using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayBtn : MonoBehaviour
{
    [SerializeField] bool isEasyBtn;

    public void RestartLevel()
    {
        GameLoopManager gameLoopManager = FindObjectOfType<GameLoopManager>();
        gameLoopManager.RestartGame(isEasyBtn);
    }
}
