using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        SetGameState(GameState.MENU);
    }

    public void SetGameState(GameState gameState)
    {
        IEnumerable<IGameStateListner> gameStateListners =
            FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IGameStateListner>();

        foreach (IGameStateListner gameStateListner in gameStateListners)
        {
            gameStateListner.GameStateChangedCallback(gameState);
        }
    }
    public void WaveCompletedCallback()
    {
        if(Player.instance.HasLeveledUp())
        {
            SetGameState(GameState.WAVETRANSITION);
        }
        else
        {
            SetGameState(GameState.SHOP);
        }
    }
}
public interface IGameStateListner
{
    void GameStateChangedCallback(GameState gameState);
}
