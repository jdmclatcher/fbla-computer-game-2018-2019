using UnityEngine;

public class TrackMouse : MonoBehaviour {

    [SerializeField] private float handXSpeed; // X look sensitivity
    [SerializeField] private float handYSpeed; // Y look sensitivity

    private void Update()
    {
        // rotates the player hand on screen based on the sensitivities and mouse input
        transform.Rotate(Input.GetAxis("Mouse Y") * Time.deltaTime * -handYSpeed, Input.GetAxis("Mouse X") * Time.deltaTime * handXSpeed, 0);
    }

}
