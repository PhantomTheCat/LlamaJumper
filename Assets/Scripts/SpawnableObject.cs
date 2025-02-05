using UnityEngine;


namespace Spawners
{
    /// <summary>
    /// Class for each object that can be spawned
    /// </summary>
    public class SpawnableObject : MonoBehaviour
    {
        //Properties
        [Header("Stats")]
        [SerializeField] private float speed = 4f;
        [SerializeField] private Vector3 direction = new Vector3(-1, 0, 0);
        [SerializeField] private float timeBeforeDisappearing = 15f;

        //Methods
        private void FixedUpdate()
        {
            //Minusing the time passed
            timeBeforeDisappearing -= Time.deltaTime;

            //Moving in direction of choice
            Vector3 movement = (direction * speed) * Time.deltaTime;
            transform.Translate(movement);

            //Seeing if we've covered enough distance to destroy this object
            if (timeBeforeDisappearing <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
