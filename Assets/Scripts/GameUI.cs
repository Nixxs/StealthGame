using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public GameObject gameLoseUI;
    public GameObject gameWinUI;
    bool gameIsOver;

    // Start is called before the first frame update
    void Start()
    {
        gameIsOver = false;

        Guard.GuardHasSpottedPlayer += ShowGameLoseUI;
        FinishZone.PlayerWins += ShowGameWinUI;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsOver && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }
    }

    void ShowGameWinUI()
    {
        OnGameOver(gameWinUI);
    }

    void ShowGameLoseUI()
    {
        OnGameOver(gameLoseUI);
    }

    void OnGameOver(GameObject gameOverUI)
    {
        gameOverUI.SetActive(true);
        gameIsOver = true;
        Guard.GuardHasSpottedPlayer -= ShowGameLoseUI;
        FinishZone.PlayerWins -= ShowGameWinUI;
    }
}
