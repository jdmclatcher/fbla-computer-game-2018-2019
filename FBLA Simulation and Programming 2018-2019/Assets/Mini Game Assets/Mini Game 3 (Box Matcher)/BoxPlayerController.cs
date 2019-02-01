using System;
using UnityEngine;

public class BoxPlayerController : MonoBehaviour {

    // private floats to store values for translations and rotations of player
    private float rotate;
    private float walkX;
    private float walkY;

    #region Start and Update
    private void Start()
    {
        
    }

    private void Update()
    {
        // TODO if not restained
        // perform movement update functions 
        transform.Rotate(0, calculateRotation(), 0);
        // X and Y are inverted
        transform.Translate(calculateXMovement(), 0, calculateYMovement());
    }

    #endregion

    #region Movement
    [SerializeField] private float moveSpeed;
    [SerializeField] private float lookSens;

    private float calculateRotation()
    {
        // if the absolute value of the input from the mouse is greater than zero/is active
        if (Math.Abs(Input.GetAxis("Mouse X")) > 0)
        {
            // sets value of the float "rotate" based on the value received from the mouse input
            rotate = Input.GetAxis("Mouse X") * Time.deltaTime * lookSens;
        }
        // if the value from the input from the arrow keys is active
        else if (Math.Abs(Input.GetAxis("Mouse X Keys")) > 0)
        {
            // sets the value of rotate based on the input from the arrow keys instead
            rotate = Input.GetAxis("Mouse X Keys") * Time.deltaTime * lookSens;
        }
        // if both are recieving no input, then the rotation is forced to 0 to prevent drifting
        else if ((Math.Abs(Input.GetAxis("Mouse X Keys")) < 0.1f) || (Math.Abs(Input.GetAxis("Mouse X")) < 0.1f))
        {
            rotate = 0f;
        }
        // returns the float
        return rotate;
    }

    private float calculateYMovement()
    {
        // gets values for the X positon of the player from input
        // inverted
        walkX = Input.GetAxis("Horizontal") * Time.deltaTime * -moveSpeed;
        return walkX;
    }

    private float calculateXMovement()
    {
        // y position (inverted)
        walkY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        
        return walkY;
    }

    #endregion

}
