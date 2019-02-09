using UnityEngine;


public class FloorController : MonoBehaviour {

    [SerializeField] private MiniGame5UI ui;

    [SerializeField] private GameObject xMark;
    [SerializeField] private GameObject checkMark;

    [SerializeField] private GameObject correctText;
    [SerializeField] private GameObject incorrectText;
    [SerializeField] private Transform responsesPos;


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Good Box")
        {
            Debug.Log("Oh no! You missed one.");
            ui.AddError(); // calls function that will add 1 to the total errors
            // spawn in X mark at object postion
            Instantiate(xMark, other.transform.position, other.transform.rotation);

            // only activate if not already visible
            //if (!incorrectText.activeInHierarchy)
            //{
            //    // turn on indicator - incorrect
            //    incorrectText.SetActive(true);
            //}

            Instantiate(incorrectText, responsesPos.position, responsesPos.rotation, responsesPos);





            // destroy the object
            Destroy(other.gameObject);
        }

        if(other.tag == "Bad Box")
        {
            Debug.Log("Good job. You avoided that one.");
            ui.AddCorrect(); // add rep points
            // spawn in check mark at object postion
            Instantiate(checkMark, other.transform.position, other.transform.rotation);
            //if (!correctText.activeInHierarchy)
            //{
            //    // turn on indicator - correct
            //    correctText.SetActive(true);
            //}

            Instantiate(correctText, responsesPos.position, responsesPos.rotation, responsesPos);



            // destroy the object
            Destroy(other.gameObject);
        }
    }

}
