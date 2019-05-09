using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineController : MonoBehaviour {

    // move speed of the finish line
    [SerializeField] private float moveSpeed;

    private MiniGame3UI ui;

    private void Start()
    {
        ui = FindObjectOfType<MiniGame3UI>();
    }

    // Update is called once per frame
    void Update () {
        // move the object at constant speed
        transform.Translate(0, 0, -moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ui.GameCompleted();
            Debug.Log("Game Finished.");
        }
    }
}
