using UnityEngine;

namespace Audio
{
    /// <summary>
    /// Class for handling the audio in the game
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : Singleton<AudioManager>
    {
        //Properties//
        [Header("Audio Clips")]
        [SerializeField] private AudioClip damaged;
        [SerializeField] private AudioClip death;
        [SerializeField] private AudioClip win;
        [SerializeField] private AudioClip spit;
        [SerializeField] private AudioClip uiClick;
        [SerializeField] private AudioClip respawn;
        [SerializeField] private AudioClip jump;
        [SerializeField] private AudioClip landing;
        [SerializeField] private AudioClip objectBreaking;

        private AudioSource source;
        private GameManager gameManager;


        //Methods//
        private void Start()
        {
            //Getting components
            source = GetComponent<AudioSource>();
            gameManager = GameManager.Instance;

            //Tying events to clips
            gameManager.PlayerHit.AddListener(PlayDamagedClip);
            gameManager.PlayerDeath.AddListener(PlayDeathClip);
            gameManager.PlayerWinLevel.AddListener(PlayWin);
            gameManager.PlayerSpit.AddListener(PlaySpit);
            gameManager.PlayerRespawn.AddListener(PlayRespawn);
            gameManager.UIButtonClicked.AddListener(PlayUIClick);
            gameManager.PlayerJump.AddListener(PlayJump);
            gameManager.PlayerLanded.AddListener(PlayLand);
            gameManager.ObjectBroken.AddListener(PlayObjectBreaking);
        }

        private void PlayClip(AudioClip clip)
        {
            source.clip = clip;
            source.Play();
        }

        private void PlayDamagedClip()
        {
            PlayClip(damaged);
        }

        private void PlayDeathClip()
        {
            PlayClip(death);
        }

        private void PlayWin()
        {
            PlayClip(win);
        }

        private void PlaySpit()
        {
            PlayClip(spit);
        }

        private void PlayUIClick()
        {
            PlayClip(uiClick);
        }

        private void PlayRespawn()
        {
            PlayClip(respawn);
        }

        private void PlayJump()
        {
            PlayClip(jump);
        }

        private void PlayLand()
        {
            PlayClip(landing);
        }

        private void PlayObjectBreaking()
        {
            PlayClip(objectBreaking);
        }
    }
}
