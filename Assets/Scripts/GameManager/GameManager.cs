using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    MainMenu,
    Play,
    Pause,
}
public class GameManager : MonoBehaviour
{
    //this is the urrent state of th game
    public static GameState currentState = GameState.MainMenu;

    private PauseUI pauseUI;
    private CataloueScreen catalogueScreen;

    public GameObject mainMenuP;
    public GameObject GOPanel;

    // Start is called before the first frame update
    void Start()
    {
        //find our PauseUI script and save it in a variable
        pauseUI = FindObjectOfType<PauseUI>();
        catalogueScreen = FindObjectOfType<CataloueScreen>();
        ApplyGameState();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TogglePause();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
            currentState = GameState.MainMenu;
            ApplyGameState();
        }
    }


    public void MainMenu()
    {
        currentState = GameState.MainMenu;
        mainMenuP.SetActive(true);
        GOPanel.SetActive(false);
        ApplyGameState();
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene(0);
        currentState = GameState.Play;
        mainMenuP.SetActive(false);
        ApplyGameState();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void TogglePause()
    {

        switch (currentState)
        {
            case GameState.MainMenu:
                break;
            
            case GameState.Play:
                currentState = GameState.Pause;
                break;
            
            case GameState.Pause:
                currentState = GameState.Play;
                break;
            
            default:
                break;
        }
        ApplyGameState();
    }

    private void ApplyGameState()
    {
        switch (currentState)
        {
            case GameState.MainMenu:
                mainMenuP.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;
                break;
            
            case GameState.Play:
                pauseUI.SetPauseScreen(false);
                mainMenuP.SetActive(false);
                catalogueScreen.CloseScreen();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1;
                break;
                
            case GameState.Pause:
                pauseUI.SetPauseScreen(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;
                break;
            
            default:
                break;
        }
    }

}
