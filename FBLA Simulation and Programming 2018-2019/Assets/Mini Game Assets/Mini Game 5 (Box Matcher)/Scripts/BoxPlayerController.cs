using System;
using UnityEngine;

public class BoxPlayerController : MonoBehaviour {

    // private floats to store values for translations and rotations of player
    private float walkX;
    private float walkY;

    // the player will be a box moving device - remote controlled

    #region Start and Update

    [HideInInspector] public bool restrained;

    private void Update()
    {
        if (!restrained)
        {
            // perform movement functions
            transform.Translate(calculateXMovement(), 0, calculateZMovement());

        }

    }

    #endregion

    

    #region Movement
    [SerializeField] private float verticalMoveSpeed;
    [SerializeField] private float horizontalMoveSpeed;
    

    private float calculateZMovement()
    {
        // gets values for the Z positon of the player from input
        // inverted
        walkX = Input.GetAxis("Vertical") * Time.deltaTime * horizontalMoveSpeed;
        return walkX;
    }

    private float calculateXMovement()
    {
        // Y position (inverted)
        walkY = Input.GetAxis("Horizontal") * Time.deltaTime * verticalMoveSpeed;
        
        return walkY;
    }

    #endregion

}
