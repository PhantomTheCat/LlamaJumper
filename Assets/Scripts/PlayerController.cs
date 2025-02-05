using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /// <summary>
    /// Class for controlling the player movement and actions
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        //Properties//
        //Movement Stats
        [Header("Movement Stats")]
        [SerializeField] private float gravityMagnitude = -9.8f;
        [SerializeField] private float jumpForce = 9000f;
        //Health
        public int PlayerHealth = 3;
        //Prefabs
        [Header("Spit")]
        [SerializeField] private GameObject spitProjectile;
        [SerializeField] private Transform spitNormalSpawnPoint;
        public GameObject CurrentSpit;
        //Components
        private Rigidbody rigidBody;
        private Animator animator;
        //Inputs
        private PlayerControls controls;
        private InputAction jumpAction;
        private InputAction slideAction;
        private InputAction spitAction;
        //Identifiers for when hit
        private string[] tagsThatDamage =
        {
            "DamagingObject",
            "BreakableObject",
        };
        //Booleans
        private bool isGrounded = false;


        //Methods
        private void Awake()
        {
            //Getting components
            rigidBody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();

            //Getting the controls and tying them together
            controls = new PlayerControls();
            jumpAction = controls.Actions.Jump;
            slideAction = controls.Actions.Slide;
            spitAction = controls.Actions.Spit;
        }

        private void OnEnable()
        {
            //Enabling controls only while player is enabled
            jumpAction.Enable();
            slideAction.Enable();
            spitAction.Enable();
        }

        private void OnDisable()
        {
            //Disabling controls when player is disabled
            jumpAction.Disable();
            slideAction.Disable();
            spitAction.Disable();
        }

        private void FixedUpdate()
        {
            //Handling the movement and gravity of our character
            HandleMovement();

            //Checking for if player wants to spit
            if (spitAction.ReadValue<float>() == 1f && spitProjectile != null)
            {
                Spit();
            }
        }

        private void HandleMovement()
        {
            //Applying gravity to our character
            Vector3 gravityMovement = Vector3.zero;
            gravityMovement.y = gravityMagnitude * Time.deltaTime;
            rigidBody.AddForce(gravityMovement);

            //Checking for movement (jump and slide) while we are on the ground
            if (isGrounded)
            {
                //Seeing if we are jumping or sliding
                if (jumpAction.ReadValue<float>() == 1f)
                {
                    Jump();
                }
                else if (slideAction.ReadValue<float>() == 1f)
                {
                    Slide();
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            //Getting the tag of the collider we left
            string collisionTag = collision.gameObject.tag;

            //Seeing if the collider was the floor
            if (collisionTag == "Ground")
            {
                //Giving our animator information
                animator.SetBool("IsGrounded", false);
                isGrounded = false;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            //Getting the tag of the collider we entered
            string collisionTag = collision.gameObject.tag;

            //Seeing if the collider was the floor
            if (collisionTag == "Ground")
            {
                isGrounded = true;

                //Sending signal for audio
                GameManager.Instance.PlayerLanded.Invoke();

                //Giving animator information
                animator.SetBool("IsGrounded", true);
                animator.SetBool("IsJumping", false);
                animator.SetBool("IsSliding", false);
            }
            else if (collisionTag == "WinArea")
            {
                //Seeing if we won by getting to the right area
                GameManager.Instance.PlayerWinLevel.Invoke();
            }
            else
            {
                //Then checking if the collider was an object that would damage the player
                for (int i = 0; i < tagsThatDamage.Length; i++)
                {
                    if (collisionTag == tagsThatDamage[i])
                    {
                        //Saying we got hit
                        TakeDamage();

                        //Destroying the other object
                        Destroy(collision.gameObject);
                    }
                }
            }
        }

        private void Spit()
        {
            //Shooting spit projectile if we don't have a spit already in orbit
            //Spit will disappear if it hits something
            if (CurrentSpit == null)
            {
                CurrentSpit = Instantiate(spitProjectile, spitNormalSpawnPoint.position, Quaternion.identity);
                GameManager.Instance.PlayerSpit.Invoke();
            }
        }

        private void Jump()
        {
            //Jumping
            Vector3 jumpMovement = Vector3.zero;
            jumpMovement.y = jumpForce * Time.deltaTime;
            rigidBody.AddForce(jumpMovement);

            //Sending signal for audio
            GameManager.Instance.PlayerJump.Invoke();

            //Sending signal to animator
            animator.SetBool("IsJumping", true);
        }

        private void Slide()
        {
            //Sending signal to animator
            animator.SetBool("IsSliding", true);
        }

        private void TakeDamage()
        {
            //Taking damage and sending signal
            PlayerHealth--;
            GameManager.Instance.PlayerHit.Invoke();

            //Checking if player is dead
            if (PlayerHealth <= 0)
            {
                GameManager.Instance.PlayerDeath.Invoke();
            }
        }
    }
}
