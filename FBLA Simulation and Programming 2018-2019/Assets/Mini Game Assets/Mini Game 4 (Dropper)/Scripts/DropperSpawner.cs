using UnityEngine;
using System.Collections.Generic;

public class DropperSpawner : MonoBehaviour
{

    // spawns random drop element every fixed amount of seconds

    [SerializeField] private List<GameObject> objectsToSpawn;
    [SerializeField] private GameObject spawnLocation;
    [SerializeField] private float timeInterval;
    private float timeIntervalLOCAL;
    [SerializeField] private float xSpawnDif;

    [SerializeField] private Transform spawnPosCONST;

    [SerializeField] private MiniGame5UI ui;


    // when the object is activated, immediately spawn the object
    private void Start()
    {
        ResetTime();
        // get ref to UI
        // ui = FindObjectOfType<MiniGame5UI>();
        // set objects length var in ui to be length of array list
        ui.objectsLength = objectsToSpawn.Count; 
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
        //float randXVal = 0f;
        //int randomNum = 0;
        // gets random number from 0 to length of the array of objects
        int randomNum = Random.Range(0, objectsToSpawn.Count);
        // gets random x val based on spawn location constant
        float randXVal = Random.Range(-xSpawnDif, xSpawnDif);
        // spawns in the corresponding random object

        // spawns at new value
        spawnLocation.transform.position = new Vector3(spawnPosCONST.position.x + randXVal, spawnPosCONST.position.y, spawnPosCONST.position.z);


        for (int i = 0; i < objectsToSpawn.Count; i++)
        {
            if (i == randomNum)
            {
                Instantiate(objectsToSpawn[i], spawnLocation.transform.position, spawnLocation.transform.rotation);
                objectsToSpawn.Remove(objectsToSpawn[i]);
            }
        }

    }

    // reset local time to random value (within restraints)
    private void ResetTime()
    {
        timeIntervalLOCAL = timeInterval;
    }
}
