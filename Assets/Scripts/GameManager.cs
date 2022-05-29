﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { mainMenu, pause, inGame, courseMenu, inShop, gameOver };

public class GameManager : MonoBehaviour
{
    public static GameManager sharedInstance;
    public GameState currentGameState;
    //public GameObject CanvasCursos, CanvasMinijuegoQuiz, CanvasTienda;


    public int platziCoins;
    public int platziRank;



    // Start is called before the first frame update
    void Start()
    {
        platziCoins = 0;
        platziRank = 350;
        //Mas adelante este metodo debe cambiarse a MainMenu();
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MainMenu()
    {
        SetGameState(GameState.mainMenu);
    }

    public void PauseGame()
    {
        SetGameState(GameState.pause);
    }

    public void StartGame()
    {
        SetGameState(GameState.inGame);
    }

    public void GameOver()
    {
        SetGameState(GameState.gameOver);
    }

    void SetGameState(GameState newGameState)
    {
        switch (newGameState)
        {
            case GameState.mainMenu:
                currentGameState = GameState.mainMenu;
                //Implementar logica de menu principal
                break;
            case GameState.pause:
                currentGameState = GameState.pause;
                //Implementar logica de menu de pausa

                break;
            case GameState.inGame:
                //Implementar logica de empezar el juego
                currentGameState = GameState.inGame;

                //La siguiente linea de codigo ocasiona un error

                //CursosManager.sharedInstance.HideCursosMenu();
                ShopManager.sharedInstance.HideShop();
                break;
            case GameState.gameOver:
                currentGameState = GameState.gameOver;
                //Implementar logica de game over
                
                break;
            case GameState.courseMenu:
                currentGameState = GameState.courseMenu;
                CursosManager.sharedInstance.ShowCursosMenu();
                
                break;
            case GameState.inShop:
                currentGameState = GameState.inShop;
                ShopManager.sharedInstance.ShowShop();
                
                break;
        }

        currentGameState = newGameState;
    }

    public void CourseMenu()
    {
        SetGameState(GameState.courseMenu);
    }

    public void inShop(){
        SetGameState(GameState.inShop);
    }
    private void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
