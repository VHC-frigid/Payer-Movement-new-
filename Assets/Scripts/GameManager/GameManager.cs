using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    MainMenu,
    Play,
    Pause,
}
public class GameManager : MonoBehaviour
{
    //this is the urrent state of th game
    public static GameState currentState = GameState.Play;

    private PauseUI pauseUI;
    private CataloueScreen catalogueScreen;

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
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

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
                break;
            
            case GameState.Play:
                pauseUI.SetPauseScreen(false);
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
