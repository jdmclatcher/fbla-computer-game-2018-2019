using System.Collections;
using UnityEngine;
using TMPro;

public class DropperPlayerController : MonoBehaviour {

    [HideInInspector] public bool canMove; // public, but dont show in inspector
    private float walkX;

    [SerializeField] private float moveSpeed;

    [SerializeField] private GameObject xMark;
    [SerializeField] private GameObject checkMark;
    

    [SerializeField] private MiniGame5UI ui;

    [SerializeField] private GameObject correctText;
    [SerializeField] private GameObject incorrectText;
    [SerializeField] private Transform responsesPos;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>(); // get ref to animator
        // ui = FindObjectOfType<MiniGame5UI>();
        IWaitForCountdown();
    }

    private void Update () {
        

        // TODO fix movement restriction
        if (canMove)
        {
            transform.Translate(calculateXMovement(), 0, 0);
            
            // if moving, say so in animator to switch anim states
            if (Mathf.Abs(calculateXMovement()) > 0)
            {
                animator.SetBool("running", true);
            }
            else
            {
                animator.SetBool("running", false); // set running to false if no movement is detected
            }
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

            //if (!correctText.activeInHierarchy)
            //{
            //    // turn on indicator - correct
            //    correctText.SetActive(true);
            //}

            Instantiate(correctText, responsesPos.position, responsesPos.rotation, responsesPos);

            

            // spawn in check mark at object postion
            Instantiate(checkMark, other.transform.position, other.transform.rotation);
            // destroy the object
            Destroy(other.gameObject);
        }
        if(other.tag == "Bad Box")
        {
            Debug.Log("Incorrect.");
            ui.AddError(); // calls function that will add 1 to the total errors

            //if (!incorrectText.activeInHierarchy)
            //{
            //    // turn on indicator - incorrect
            //    incorrectText.SetActive(true);
            //}

            Instantiate(incorrectText, responsesPos.position, responsesPos.rotation, responsesPos);



            // spawn in X mark at object postion
            Instantiate(xMark, other.transform.position, other.transform.rotation);
            // destroy the object
            Destroy(other.gameObject);
        }
    }

}
