using System;
using UnityEngine;

public class BoxPlayerController : MonoBehaviour {

    // private floats to store values for translations and rotations of player
    private float walkX;
    private float walkY;

    #region Start and Update
    private void Start()
    {
        
    }

    private void Update()
    {
        if (!restrained)
        {
            transform.Translate(calculateXMovement(), 0, calculateZMovement());
        }


        // perform movement update functions 
        // transform.Rotate(0, calculateRotation(), 0);
        // X and Y are inverted
    }

    #endregion

    #region Movement
    [SerializeField] private float verticalMoveSpeed;
    [SerializeField] private float horizontalMoveSpeed;
    // [SerializeField] private float lookSens;
    [HideInInspector] public bool restrained;

    // MAY ADD ROTATION LATER
    //private float calculateRotation()
    //{
    //    // if the value from the input from the arrow keys is active
    //    if (Math.Abs(Input.GetAxis("Mouse X Keys")) > 0)
    //    {
    //        // sets the value of rotate based on the input from the arrow keys
    //        rotate = Input.GetAxis("Mouse X Keys") * Time.deltaTime * lookSens;
    //    }
    //    // if both are recieving no input, then the rotation is forced to 0 to prevent drifting
    //    else if ((Math.Abs(Input.GetAxis("Mouse X Keys")) < 0.1f) || (Math.Abs(Input.GetAxis("Mouse X")) < 0.1f))
    //    {
    //        rotate = 0f;
    //    }
    //    // returns the float
    //    return rotate;
    //}

    private float calculateZMovement()
    {
        // gets values for the Z positon of the player from input
        // inverted
        walkX = Input.GetAxis("Horizontal") * Time.deltaTime * -horizontalMoveSpeed;
        return walkX;
    }

    private float calculateXMovement()
    {
        // Y position (inverted)
        walkY = Input.GetAxis("Vertical") * Time.deltaTime * verticalMoveSpeed;
        
        return walkY;
    }

    #endregion

}
