using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Script for controlling the menu in the splash screen
    /// </summary>
    public class MenuBehavior : MonoBehaviour
    {
        //Properties//
        [Header("Buttons")]
        [SerializeField] private Button playButton;

        private GameManager gameManager;


        //Methods//
        private void Start()
        {
            //Getting the game manager for ease of access
            gameManager = GameManager.Instance;

            //Tying all the menu buttons to the game manager
            playButton.onClick.AddListener(gameManager.PlayButtonPressed.Invoke);
        }

    }
}

