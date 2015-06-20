using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameStateControllerScript : MonoBehaviour {

    /// <summary>
    /// The canvas to be activated in main menu state.
    /// </summary>
    public GameObject mainMenuCanvas;

    /// <summary>
    /// The canvas to be activated in play state.
    /// </summary>
    public GameObject playCanvas;

    /// <summary>
    /// The canvas to be activated in game over state.
    /// </summary>
    public GameObject gameOverCanvas;

    /// <summary>
    /// The Text component to be updated with score during play state.
    /// </summary>
    public Text playScore;

    /// <summary>
    /// The Text component to be set with score during game over state.
    /// </summary>
    public Text gameOverScore;

    /// <summary>
    /// The score to be displayed in play and game over state.
    /// </summary>
    public int score;

    private GameObject currentCanvas;
    private string state;

    public void Start()
    {
        currentCanvas = null;

        MainMenu();
    }

    public void Update()
    {
        if (state == "play")
        {
            playScore.text = score.ToString();
        }
        else if (state == "mainmenu")
        {
            if (Input.GetButtonDown("Cancel"))
            {
                Application.Quit();
            }
            else if (Input.anyKeyDown)
            {
                Play();
            }
        }
        else if (state == "gameover")
        {
            if (Input.anyKeyDown)
            {
                MainMenu();
            }
        }
    }

    public void MainMenu()
    {
        CurrentCanvas = mainMenuCanvas;
        state = "mainmenu";

        GameObject.Find("LevelController").SendMessage("Reset");
        GameObject.FindGameObjectWithTag("Player").SendMessage("Reset");
        GameObject.FindGameObjectWithTag("MainCamera").SendMessage("Reset");
    }

    public void Play()
    {
        CurrentCanvas = playCanvas;
        state = "play";
        score = 0;

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementScript>().canMove = true;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovementScript>().moving = true;
    }

    public void GameOver()
    {
        CurrentCanvas = gameOverCanvas;
        state = "gameover";

        gameOverScore.text = score.ToString();

        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovementScript>().moving = false;
    }

    private GameObject CurrentCanvas
    {
        get
        {
            return currentCanvas;
        }
        set
        {
            if (currentCanvas != null)
            {
                currentCanvas.SetActive(false);
            }
            currentCanvas = value;
            currentCanvas.SetActive(true);
        }
    }
}
