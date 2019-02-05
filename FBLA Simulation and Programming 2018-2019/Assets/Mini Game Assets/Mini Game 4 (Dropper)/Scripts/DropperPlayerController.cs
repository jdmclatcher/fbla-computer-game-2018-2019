using System.Collections;
using UnityEngine;

public class DropperPlayerController : MonoBehaviour {

    [HideInInspector] public bool canMove; // public, but dont show in inspector
    private float walkX;

    [SerializeField] private float moveSpeed;

    [SerializeField] private GameObject xMark;
    [SerializeField] private GameObject checkMark;
    

    [SerializeField] private MiniGame5UI ui;

    private void Start()
    {
        // ui = FindObjectOfType<MiniGame5UI>();
        IWaitForCountdown();
        
    }

    private void Update () {
        

        // TODO fix movement restriction
        if (canMove)
        {
            transform.Translate(calculateXMovement(), 0, 0);
        }

        
    }

    private float calculateXMovement()
    {
        // gets values for the X positon of the player from input
        // raw so there's no smoothing

        walkX = Input.GetAxisRaw("Horizontal") * Time.deltaTime * moveSpeed; 

        return walkX;
    }


    IEnumerator IWaitForCountdown()
    {
        // canMove = false;
        // wait 3 and a half seconds before can move
        yield return new WaitForSeconds(3.5f);
        //canMove = true;
        //yield return null;

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Good Box")
        {
            Debug.Log("That's correct!");
            ui.AddCorrect(); // add rep points
            // spawn in check mark at object postion
            Instantiate(checkMark, other.transform.position, other.transform.rotation);
            // destroy the object
            Destroy(other.gameObject);
        }
        if(other.tag == "Bad Box")
        {
            Debug.Log("Incorrect.");
            ui.AddError(); // calls function that will add 1 to the total errors
            // spawn in X mark at object postion
            Instantiate(xMark, other.transform.position, other.transform.rotation);
            // destroy the object
            Destroy(other.gameObject);
        }
    }

}
