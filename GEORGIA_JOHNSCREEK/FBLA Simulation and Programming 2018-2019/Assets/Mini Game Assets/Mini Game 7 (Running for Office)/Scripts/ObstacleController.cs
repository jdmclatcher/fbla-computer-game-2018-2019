using UnityEngine;

public class ObstacleController : MonoBehaviour {

    // move speed of the obstacle
    [SerializeField] private float moveSpeed;

    [SerializeField] private MiniGame3UI ui;

    private void Update()
    {
        ui = FindObjectOfType<MiniGame3UI>(); // get ref to game UI
        // move the object at constant speed
        transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
    }


    private void OnTriggerEnter(Collider other)
    { 
        if(other.tag == "Obstacle Destroyer")
        {
            Destroy(gameObject); // destory itself
        }

        // WARNING this will run regardless of if the user gets it correct or not
        if(other.tag == "Player")
        {
            ui.HitObstacle(gameObject); // call function on UI script, passed in itself
            
            Debug.Log("Hello, player.");
        }
    }
}
