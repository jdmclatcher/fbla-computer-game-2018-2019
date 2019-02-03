using System;
using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    
    [Header("Movement")]
    [SerializeField] private float moveSpeed; // speed of player translation (movement)
    [SerializeField] private float speedHindranceMultiplier; // amount to divide speed by for horizontal movement

    // MAY ADD JUMPING LATER -- 
    // [SerializeField] private float jumpForce;
    // [SerializeField] private float gravityForce; 

    [SerializeField] private float mouseLookSensitivity; // for the mouse rotation (using mouse)
    [SerializeField] private float keyLookSensitivity; // for the mouse rotation (using arrow keys)

    // private floats to store values for translations and rotations of player
    private float rotate;
    private float walkX;
    private float walkY;

    // check if player can move, defaults to false
    [SerializeField] private bool restrained;

    [SerializeField] private float collisionRestrainDuration; // amount of time to be stunned for

    UI ui;

    // check that becomes true when game initially starts, but wears off when leaving trigger
    private bool respawnInvincible;

    private int firstTime; // int to track if player has played game at all before

    [SerializeField] private Transform startPosition;

    [Header("Attached Objects")]
    [SerializeField] private GameObject backupCamera;

    #region Start and Update

    // Use this for initialization
    void Start()
    {
        // TEMP reset playerprefs on scene enter
        ResetPrefs(); // resets everything, so it will always be the first time playing


        // Finds the UI component in the scene
        ui = FindObjectOfType<UI>();
        Cursor.visible = false;
        restrained = false;
        // multiplier to reduce (using division) the X movement (side to side) 
        // and also the Y backwards movement
        if (speedHindranceMultiplier < 1)
        {
            // needs to be above 1 to be slower than the Y forward movement
            speedHindranceMultiplier = 1;
        }

        firstTime = PlayerPrefs.GetInt("First Time");

        if(firstTime == 0)
        {
            // not invincible to collider because wont be spawning in one
            respawnInvincible = false;

            // spawn in starting position
            PlayerPrefs.SetFloat("xLOC", startPosition.position.x);
            PlayerPrefs.SetFloat("yLOC", startPosition.position.y);
            PlayerPrefs.SetFloat("zLOC", startPosition.position.z);
            LoadSaveLocation();

            // set first time to true (1)
            PlayerPrefs.SetInt("First Time", 1);
        }
        else
        {
            // load save data
            // load save location from playerprefs for respawning
            LoadSaveLocation();
            LoadSaveRotation();

            // invincible to collider when arrive back in main world
            respawnInvincible = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        // TEMPORARY - until a game UI and HUD script is created
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = false;
        }

        // runs the movement and rotation code only if the player is allowed to move
        if (!restrained)
        {
            // "backup camera" follows the player
            backupCamera.transform.position = transform.position;
            backupCamera.transform.rotation = transform.rotation;
            // applies the rotations and translations (movement) using returned values
            // from movement functions
            transform.Rotate(0, calculateRotation(), 0);
            transform.Translate(calculateXMovement(), 0, calculateYMovement());

            // BROKEN...
            // only activates the "backup camera" if the player is moving backwards
            //if (walkY < 0)
            //{
            //    backupCamera.gameObject.GetComponent<BoxCollider>().enabled = true;
            //} else
            //{
            //    backupCamera.gameObject.GetComponent<BoxCollider>().enabled = false;
            //}
        }

        // TEMP escape key quits game
        if (Input.GetKey(KeyCode.Escape))
        {
            Debug.Log("Game quit.");
            Application.Quit();
        }
    }
    #endregion

    #region Collision

    private void OnTriggerEnter(Collider other)
    {
        // when the player colides with a test box...
        if (other.tag == "Wall")
        {
            Debug.Log("Hey! Watch where you're going!");
            // ...stops player movement temporarily using a coroutine
            StartCoroutine(_restrain());
        }

        // switch statement that will validate each mini game number
        // and proceed accordingly
        switch (other.tag)
        {
            // mini game booth checks
            case "MiniGame/1":
                Validate(1);
                break;
            case "MiniGame/2":
                Validate(2);
                break;
            case "MiniGame/3":
                Validate(3);
                break;
            case "MiniGame/4":
                Validate(4);
                break;
            case "MiniGame/5":
                Validate(5);
                break;
            case "MiniGame/6":
                Validate(6);
                break;
            case "MiniGame/7":
                Validate(7);
                break;
            case "MiniGame/8":
                Validate(8);
                break;
            // wall checks
            case "Wall/1":
                // if player has correct amount of supporters, they can pass through
                if (PlayerPrefs.GetInt("Supporters") >= ui.supportersToFinish1)
                {
                    // TODO open the gate!
                    Debug.Log("Congrats! You can move on now.");
                    ui.OpenTheGate1(); // go to next level
                } else
                {
                    // TODO give a helpful message of rejection
                    Debug.Log("You need to gather more supporters.");
                }
                break;
            case "Wall/2":
                // WIN THE GAME
                // if player has correct amount of supporters, they can pass through
                if (PlayerPrefs.GetInt("Supporters") >= ui.supportersToFinish2)
                {
                    // TODO win the game
                    Debug.Log("Congrats! You have completed the game!");
                    ui.OpenTheGate2(); // (win the game)
                }
                else
                {
                    Debug.Log("You need to get more supporters.");
                }
                break;
            
            default:
                break;
        }
        
    }
    
    private void Validate(int gameNumber)
    {
        // calls function on the UI script to handle scene transition
        ui.gameNumber = gameNumber; // define the game number in the ui script
        if (!respawnInvincible)
        {
            // Debug.Log("Ok, transitioning to the Mini-Game");
            ui.ShowMiniGameUI(); 
            ui.freeToMove = false;
            restrained = true; // so the player can't move around while in the menu
        }

        // Save location using PlayerPrefs for respawning ... 
        SaveLocation();
        SaveRotation();
    }

    private void OnTriggerStay(Collider other)
    {
        // only keeps player restrained if they have not
        // pressed the quit button
        if(other.tag == "MiniGame/" + ui.gameNumber)
        {
            if (ui.freeToMove)
            {
                restrained = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // when the player leaves the mini game box when returning
        // to the world level, the invincibility will wear off
        if(other.tag == "MiniGame/" + ui.gameNumber)
        {
            // removes the invincibility
            respawnInvincible = false;
        }
    }

    // current system suspends all player movement for certain amount of time
    IEnumerator _restrain()
    {
        restrained = true;
        // waits a determined amount of time for a "stun" effect
        yield return new WaitForSeconds(collisionRestrainDuration);
        restrained = false;
    }

    #endregion

    #region Movement Functions
    private float calculateRotation()
    {
        // if the absolute value of the input from the mouse is greater than zero/is active
        if (Math.Abs(Input.GetAxis("Mouse X")) > 0)
        {
            // sets value of the float "rotate" based on the value received from the mouse input
            rotate = Input.GetAxis("Mouse X") * Time.deltaTime * mouseLookSensitivity;
        }
        // if the value from the input from the arrow keys is active
        else if (Math.Abs(Input.GetAxis("Mouse X Keys")) > 0)
        {
            // sets the value of rotate based on the input from the arrow keys instead
            rotate = Input.GetAxis("Mouse X Keys") * Time.deltaTime * keyLookSensitivity;
        }
        // if both are recieving no input, then the rotation is forced to 0 to prevent drifting
        else if ((Math.Abs(Input.GetAxis("Mouse X Keys")) < 0.1f) || (Math.Abs(Input.GetAxis("Mouse X")) < 0.1f))
        {
            rotate = 0f;
        }
        // returns the float
        return rotate;
    }

    private float calculateXMovement()
    {
        // gets values for the X positon of the player from input
        walkX = Input.GetAxis("Horizontal") * Time.deltaTime * (moveSpeed / speedHindranceMultiplier);
        return walkX;
    }

    private float calculateYMovement()
    {
        // checks to see if player in going backward
        if (Input.GetAxis("Vertical") < 0)
        {
            // applies the speed hindrance when calculating the movement
            walkY = Input.GetAxis("Vertical") * Time.deltaTime * (moveSpeed / speedHindranceMultiplier);
        }
        else
        {
            // does NOT apply the speed hindrance and uses the full movement speed
            walkY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        }
        // gets values for a Y position for the player based on input
        return walkY;
    }

    #endregion

    #region Saving and Loading Position and Rotation

    // save and load player position
    private void SaveLocation()
    {
        PlayerPrefs.SetFloat("xLOC", transform.position.x);
        PlayerPrefs.SetFloat("yLOC", transform.position.y);
        PlayerPrefs.SetFloat("zLOC", transform.position.z);
    }
    private void LoadSaveLocation()
    {
        float x = PlayerPrefs.GetFloat("xLOC");
        float y = PlayerPrefs.GetFloat("yLOC");
        float z = PlayerPrefs.GetFloat("zLOC");
        Vector3 posVec = new Vector3(x, y, z);
        transform.position = posVec;
    }

    // save and load player rotation
    private void SaveRotation()
    {
        PlayerPrefs.SetFloat("xROT", transform.eulerAngles.x);
        PlayerPrefs.SetFloat("yROT", transform.eulerAngles.y);
        PlayerPrefs.SetFloat("zROT", transform.eulerAngles.z);
    }
    private void LoadSaveRotation()
    {
        float x = PlayerPrefs.GetFloat("xROT");
        float y = PlayerPrefs.GetFloat("yROT");
        float z = PlayerPrefs.GetFloat("zROT");
        Quaternion rotVec = Quaternion.Euler(x, y, z);
        transform.rotation = rotVec;
    }

    #endregion

    #region TEMP UTIL

    // reset playerprefs rep points value
    private void ResetPrefs()
    {
        // PlayerPrefs.SetInt("Rep Points", 0);
        // PlayerPrefs.SetInt("Supporters", 0);
        PlayerPrefs.SetFloat("xLOC", startPosition.position.x);
        PlayerPrefs.SetFloat("yLOC", startPosition.position.y);
        PlayerPrefs.SetFloat("zLOC", startPosition.position.z);
        PlayerPrefs.SetInt("First Time", 0);
    }

    #endregion

}
