using UnityEngine;

public class ObstacleController : MonoBehaviour {

    // move speed of the obstacle
    [SerializeField] private float moveSpeed;

    private void Update()
    {
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
            // TODO play an animation or something
            Debug.Log("Hello, player.");
        }
    }
}
