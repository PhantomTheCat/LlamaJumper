using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// Class responsible for controlling all the 
/// events in the game and acting as a hub
/// </summary>
public class GameManager : Singleton<GameManager>
{
    //Properties//
    public int CurrentLevel = 1;
    private bool isPaused = false;

    //Events//
    /// <summary>
    /// Unity event for when player gets hit by something
    /// </summary>
    public UnityEvent PlayerHit = new UnityEvent();

    /// <summary>
    /// Unity event for when player dies
    /// </summary>
    public UnityEvent PlayerDeath = new UnityEvent();

    /// <summary>
    /// Unity Event for when the player wins the level
    /// </summary>
    public UnityEvent PlayerWinLevel = new UnityEvent();

    /// <summary>
    /// Unity Event for when the level is reset
    /// </summary>
    public UnityEvent ResetLevel = new UnityEvent();

    /// <summary>
    /// Unity event for when the play button is pressed
    /// </summary>
    public UnityEvent PlayButtonPressed = new UnityEvent();

    /// <summary>
    /// Unity event for when the player spits
    /// </summary>
    public UnityEvent PlayerSpit = new UnityEvent();

    /// <summary>
    /// Unity event for the audio source once a UI Button is clicked
    /// </summary>
    public UnityEvent UIButtonClicked = new UnityEvent();

    /// <summary>
    /// Unity event for when the player respawns, meant for the audio
    /// </summary>
    public UnityEvent PlayerRespawn = new UnityEvent();

    /// <summary>
    /// Unity event for when the player jumps, meant for audio
    /// </summary>
    public UnityEvent PlayerJump = new UnityEvent();

    /// <summary>
    /// Unity event for when the player lands after jumping, meant for audio
    /// </summary>
    public UnityEvent PlayerLanded = new UnityEvent();

    /// <summary>
    /// Unity event for transitioning to next level
    /// </summary>
    public UnityEvent NextLevelUpdate = new UnityEvent();

    /// <summary>
    /// Unity Event for when breakable objects break
    /// </summary>
    public UnityEvent ObjectBroken = new UnityEvent();


    //Methods//
    public override void Awake()
    {
        //Doing the base things
        base.Awake();

        //Tying events to methods
        PlayerDeath.AddListener(PauseGame);
        PlayerWinLevel.AddListener(PauseGame);
        ResetLevel.AddListener(ResetGame);
        PlayButtonPressed.AddListener(PlayPressed);
        NextLevelUpdate.AddListener(MoveToNextLevel);
    }

    private void PauseGame()
    {
        //Checking if paused already or not
        if (!isPaused)
        {
            //If we're not paused, going to pause
            Time.timeScale = 0f;
            isPaused = true;
        }
        else
        {
            //Otherwise, we are going to unpause
            Time.timeScale = 1f;
            isPaused = false;
        }
    }

    private void ResetGame()
    {
        //Unpausing the game then reloading the scene
        PauseGame();
        UIButtonClicked.Invoke();
        PlayerRespawn.Invoke();
        SceneManager.LoadScene(CurrentLevel);
    }

    private void PlayPressed()
    {
        //Loading the scene
        UIButtonClicked.Invoke();
        SceneManager.LoadScene(CurrentLevel);
    }

    private void MoveToNextLevel()
    {
        //Unpausing the game then loading the next scene
        PauseGame();
        CurrentLevel++;
        SceneManager.LoadScene(CurrentLevel);
    }
}
