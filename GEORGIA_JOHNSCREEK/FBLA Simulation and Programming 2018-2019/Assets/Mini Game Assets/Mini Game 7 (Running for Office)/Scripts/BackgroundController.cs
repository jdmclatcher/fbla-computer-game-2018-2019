using UnityEngine;

public class BackgroundController : MonoBehaviour {

    // moves indefinetly until collides with the background destroyer object

    [SerializeField] private float moveSpeed;

    private MiniGame3UI ui;

    private void Start()
    {
        ui = FindObjectOfType<MiniGame3UI>(); // get ref to UI  
    }

    private void Update()
    {
        
        // move the object at constant speed
        if (ui.hasStarted)
        {
            transform.Translate(0, -moveSpeed * Time.deltaTime, 0);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Background Destroyer")
        {
            Destroy(gameObject);
        }

    }
}
