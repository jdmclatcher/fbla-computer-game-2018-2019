using UnityEngine;

public class BackgroundController : MonoBehaviour {

    // moves indefinetly until collides with the background destroyer object

    [SerializeField] private float moveSpeed;

    private void Update()
    {
        // move the object at constant speed
        transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Background Destroyer")
        {
            Destroy(gameObject);
        }

    }
}
