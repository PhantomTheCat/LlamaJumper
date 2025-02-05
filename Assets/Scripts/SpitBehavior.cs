using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class for controlling how the spit moves and functions
/// </summary>
public class SpitBehavior : MonoBehaviour
{
    //Properties
    [Header("Stats")]
    [SerializeField] private float spitSpeed = 7.0f;
    [SerializeField] private float timeBeforeDestruction = 3.0f;
    public UnityAction SpitDestroyedEvent;
    private float timeLeft;

    //Methods
    private void Awake()
    {
        //Setting time
        timeLeft = timeBeforeDestruction;
    }

    private void FixedUpdate()
    {
        //Minusing the time passed
        timeLeft -= Time.deltaTime;

        //Seeing if the spit has timed out
        if (timeLeft <= 0)
        {
            DestroySpit();
        }
        else
        {
            MoveSpit();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Seeing what we hit before destroying
        string collisionTag = collision.gameObject.tag;
        bool isPlayer = false;

        if (collisionTag == "BreakableObject")
        {
            //Destroy the breakable object
            Destroy(collision.gameObject);

            //Sending signal out
            GameManager.Instance.ObjectBroken.Invoke();
        }
        else if (collisionTag == "Player")
        {
            isPlayer = true;
        }

        if (!isPlayer)
        {
            //Destroy this game object at the end
            //As long as it wasn't a player
            DestroySpit();
        }
    }

    private void MoveSpit()
    {
        //Spit is traveling straight along the positive x axis
        Vector3 movement = new Vector3(spitSpeed * Time.deltaTime, 0, 0);
        transform.Translate(movement);
    }

    private void DestroySpit()
    {
        Destroy(this.gameObject);
        //this.SpitDestroyedEvent.Invoke();
    }
}
