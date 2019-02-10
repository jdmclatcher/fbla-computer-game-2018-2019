using System;
using UnityEngine;

public class BoxPlayerController : MonoBehaviour {

    // private floats to store values for translations and rotations of player
    private float walkX;
    private float walkY;

    private Animator animator;

    #region Start and Update
    private void Start()
    {
        // get ref to animator
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!restrained)
        {
            // perform movement functions
            transform.Translate(calculateXMovement(), 0, calculateZMovement());
            // check and set walking status accordingly
            if (Mathf.Abs(calculateXMovement()) > 0 || Mathf.Abs(calculateZMovement()) > 0)
            {
                animator.SetBool("walking", true);
            } else
            {
                animator.SetBool("walking", false);
            }
        }

    }

    #endregion

    #region Movement
    [SerializeField] private float verticalMoveSpeed;
    [SerializeField] private float horizontalMoveSpeed;
    // [SerializeField] private float lookSens;
    [HideInInspector] public bool restrained;

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
