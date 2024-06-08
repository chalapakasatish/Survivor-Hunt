using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour,IGameStateListner
{
    [Header("Panels")]
    [SerializeField] private GameObject menuPanel, gamePanel, waveTransitionPanel, shopPanel;

    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.MENU:
                menuPanel.SetActive(true);
                gamePanel.SetActive(false);
                waveTransitionPanel.SetActive(false);
                shopPanel.SetActive(false);
                break;
            case GameState.GAME:
                menuPanel.SetActive(false);
                gamePanel.SetActive(true);
                shopPanel.SetActive(false);
                break;
            case GameState.WAVETRANSITION:
                gamePanel.SetActive(false);
                waveTransitionPanel.SetActive(true);
                break;
            case GameState.SHOP:
                gamePanel.SetActive(false);
                shopPanel.SetActive(true);
                waveTransitionPanel.SetActive(false);
                break;
            default:
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
