using UnityEngine;

public class BackgroundSpawner : MonoBehaviour {

    // spawns random background every fixed amount of second
    // every backgroudn has the same width, so spawning works perfectly

    [SerializeField] private GameObject[] objectsToSpawn;
    [SerializeField] private GameObject spawnLocation;
    [SerializeField] private float timeInterval;
    private float timeIntervalLOCAL;


    // when the object is activated, immediately spawn the object
    private void Start()
    {
        SpawnObject();
        ResetTime();
    }

    private void Update()
    {
        // counts down until local time var is 0, then spawns object and
        // resets local time for next spawn
        timeIntervalLOCAL -= Time.deltaTime;
        if (timeIntervalLOCAL <= 0)
        {
            SpawnObject();
            ResetTime();
        }
    }

    private void SpawnObject()
    {
        // gets random number from 0 to length of the array of objects
        int randomNum = Random.Range(0, objectsToSpawn.Length);
        
        // spawns in the corresponding random object
        for(int i = 0; i < objectsToSpawn.Length; i++)
        {
            if(i == randomNum)
            {
                // spawn in a clone
                GameObject obstacleClone = Instantiate(objectsToSpawn[i], spawnLocation.transform.position, spawnLocation.transform.rotation, spawnLocation.transform);
            }
        }   
    }

    // reset local time to random value (within restraints)
    private void ResetTime()
    {
        timeIntervalLOCAL = timeInterval;
    }
}
