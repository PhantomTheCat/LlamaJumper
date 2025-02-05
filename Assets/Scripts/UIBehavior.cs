using Player;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace UI
{
    /// <summary>
    /// Script for controlling the UI during the game
    /// </summary>
    public class UIBehavior : MonoBehaviour
    {
        //Properties//
        [Header("UI Icons and Text")]
        [SerializeField] private Image[] heartIcons;
        [SerializeField] private Button[] resetButtons;
        [SerializeField] private GameObject deathScreen;
        [SerializeField] private TextMeshProUGUI endText;
        [SerializeField] private GameObject winScreen;
        [SerializeField] private GameObject instructionPanel;
        [SerializeField] private TextMeshProUGUI currentLevelIndex;
        [SerializeField] private Button nextLevelButton;

        [Header("Reference Images")]
        [SerializeField] private Sprite healthyHeart;
        [SerializeField] private Sprite deadHeart;

        private PlayerController player;
        private GameManager gameManager;

        private float timeForInstruction = 5f;
        private bool timerTriggered = false;

        private string[] endTextResponses =
        {
        "You died of inexplainable reasoning.",
        "Your horoscope predicted you will die today and you did.",
        "You died of eating too much ice cream.",
        "Ooooo, you bonked your head real good...",
        "Why did you die there? ;)",
        "Guess you could say it's 'Game Over'?",
        "You died of motion.",
        "Who died? YOU DID!",
        "You died?",
    };


        //Methods//
        private void Start()
        {
            //Getting player component
            GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
            player = playerGO.GetComponent<PlayerController>();

            //Setting up GameManager
            gameManager = GameManager.Instance;

            //Setting up the index
            currentLevelIndex.text = $"Current Level: {gameManager.CurrentLevel}";

            //Setting up events
            SetUpButtons();
            gameManager.PlayerHit.AddListener(DamagePlayerUI);
            gameManager.PlayerDeath.AddListener(DeathUI);
            gameManager.PlayerWinLevel.AddListener(WinUI);
        }

        private void Update()
        {
            //Passing time
            if (timeForInstruction > 0)
            {
                timeForInstruction -= Time.deltaTime;
            }
            else if (!timerTriggered)
            {
                //Triggering timer if it hasn't been, then
                //making sure it isn't triggering again for performance
                instructionPanel.SetActive(false);
                timerTriggered = true;
            }
        }

        private void SetUpButtons()
        {
            //Setting up each reset button in the scene
            for (int i = 0; i < resetButtons.Length; i++)
            {
                resetButtons[i].onClick.AddListener(gameManager.ResetLevel.Invoke);
            }

            //Setting up the next level button
            if (nextLevelButton != null)
            {
                nextLevelButton.onClick.AddListener(gameManager.NextLevelUpdate.Invoke);
            }
        }

        private void DamagePlayerUI()
        {
            if (player != null)
            {
                //Getting health and what icon to change because of that
                int health = player.PlayerHealth;

                //Changing the heart
                heartIcons[health].sprite = deadHeart;
            }
        }

        private void DeathUI()
        {
            //Setting the Death Screen to be active
            deathScreen.SetActive(true);

            //Getting a random end text for shits and giggles
            int randomIndex = Random.Range(0, endTextResponses.Length);

            //Setting the end text to be the random string
            endText.text = endTextResponses[randomIndex];
        }

        private void WinUI()
        {
            //Setting the win screen to be active
            winScreen.SetActive(true);
        }
    }
}
