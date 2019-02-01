using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackupCamera : MonoBehaviour {

    [SerializeField] private GameObject backupWarning;

    private void Start()
    {
        backupWarning.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    { 
        // if the player gets close to the "walls", warning message is put in the UI
        if (other.tag == "Wall")
        {
            backupWarning.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // warning message removed if player moves out of the way
        if (other.tag == "Wall")
        {
            backupWarning.SetActive(false);
        }
    }
    
}
