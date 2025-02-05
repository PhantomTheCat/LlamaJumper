using UnityEngine;

namespace Spawners
{
    /// <summary>
    /// The type of spawner, with background looping and obstacles not looping
    /// </summary>
    public enum SpawnerType 
    {
        BACKGROUND = 0,
        OBSTACLES = 1,
    }

    /// <summary>
    /// Behavior for all spawners
    /// </summary>
    public class SpawnerBehavior : MonoBehaviour
    {
        //Properties//

        //Spawner TimeSheet
        [Header("Spawning Timesheet")]
        /// <summary>
        /// The time that occurs between spawns, placed in same order as the objects. 
        /// And starts happening after the first spawn.
        /// </summary>
        [SerializeField] protected float[] timeBetweenSpawns;
        /// <summary>
        /// The spawnable objects in order from first to last
        /// </summary>
        [SerializeField] protected GameObject[] spawnableObjects;

        //Type
        [Header("Spawner Type")]
        [SerializeField] protected SpawnerType spawnerType;

        //Range between randomized objects
        [Header("Time Range for Randomized Spawn")]
        [SerializeField] private float minValue = 0.5f;
        [SerializeField] private float maxValue = 2f;


        //Tracking Values
        /// <summary>
        /// Marker for where we are at in the spawning process
        /// </summary>
        private int spawnMarker = 0;
        /// <summary>
        /// The current time to wait before instantiating the next object
        /// </summary>
        private float timeToWait;
        /// <summary>
        /// The length of the two timesheet arrays
        /// </summary>
        private int spawnArrayLengths;



        //Methods
        private void Awake()
        {
            //Checking if we have same amount between time slots and objects
            if (spawnableObjects != null && timeBetweenSpawns != null)
            {
                if (timeBetweenSpawns.Length < spawnableObjects.Length)
                {
                    //If we don't, we are going to place random values
                    //in the time slots to go up to the spawnable objects level.
                    FillTimeSlots();

                    //Note: Wont matter for if the time slots one is already bigger as 
                    //that one can be bigger and wont cause an issue (tracking based off spawnable objects)
                }
            }

            //Starting the cycle
            spawnArrayLengths = spawnableObjects.Length;
            spawnMarker = 0;
            AdvanceArray();
        }

        private void Update()
        {
            //Making sure we are still in the arrays
            if (spawnMarker < spawnArrayLengths)
            {
                //Minusing the time passed
                timeToWait -= Time.deltaTime;

                //If the time has passed, we advance the array
                if (timeToWait <= 0)
                {
                    AdvanceArray();
                }
            }
            //Else if it's background, we will loop it by going back to the start
            else if (spawnerType == SpawnerType.BACKGROUND)
            {
                spawnMarker = 0;
                AdvanceArray();
            }
        }

        private void AdvanceArray()
        {
            //Advancing the array forward by instantiating the new object, 
            //Refreshing the time, and updating the spawnMarker
            GameObject instantiatedObject = Instantiate(spawnableObjects[spawnMarker], transform.position, Quaternion.identity);
            instantiatedObject.transform.SetParent(transform, true);
            timeToWait = timeBetweenSpawns[spawnMarker];
            spawnMarker++;
        }

        private void FillTimeSlots()
        {
            //Making the TimeSlot array be a new array with 
            //the same length as the Spawnable Objects Array
            timeBetweenSpawns = new float[spawnableObjects.Length];

            //Placing random values in each time slot between the range
            for (int i = 0; i < spawnableObjects.Length; i++)
            {
                timeBetweenSpawns[i] = Random.Range(minValue, maxValue);
            }
        }
    }
}
