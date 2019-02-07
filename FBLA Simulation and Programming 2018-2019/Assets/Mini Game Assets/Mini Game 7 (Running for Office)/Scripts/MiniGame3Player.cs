using System.Collections;
using UnityEngine;

public class MiniGame3Player : MonoBehaviour {

    [SerializeField] private float slowMoVal;
    [SerializeField] private MiniGame3UI ui;

    private void Start()
    {
        // ui = FindObjectOfType<MiniGame3UI>(); // obtain ref to UI
    }

    private void OnTriggerEnter(Collider other)
    { 
        if(other.tag == "Obstacle Trigger")
        { 
            Time.timeScale = slowMoVal;
            Debug.Log("1");
            // STARTS A COUROUTINE in the UI script
            StartCoroutine(ui.IQuestion(other.gameObject));
        } 

    }

    private void OnTriggerExit(Collider other)
    {
        // revert to regular time scale when exit with collider trigger
        if(other.tag == "Obstacle Trigger")
        {
            Time.timeScale = 1f;
        }
    }

}
