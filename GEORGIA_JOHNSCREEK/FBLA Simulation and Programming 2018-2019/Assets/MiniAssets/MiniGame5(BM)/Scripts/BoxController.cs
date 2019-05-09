using UnityEngine;

public class BoxController : MonoBehaviour {

    [SerializeField] private int desiredTagNum;
    [SerializeField] private MiniGame6UI UI;

    [SerializeField] private GameObject boxPoofParticle;

    // [SerializeField] private float kickBackSpeed;
    // [SerializeField] private Transform kickBackRow;


    // when collides with a Box with the correct tag, calls function on UI
    // if not, calls a dif function on UI
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Box/" + desiredTagNum.ToString())
        {
            UI.GoodBox();

            Instantiate(boxPoofParticle, transform.position, transform.rotation); //spawn in particle
            gameObject.SetActive(false); // disable itself


        } else 
        {
            // move back 0.75 units
            transform.Translate(-0.75f, 0f, 0f);
            UI.BadBox();
        }
    }
}
