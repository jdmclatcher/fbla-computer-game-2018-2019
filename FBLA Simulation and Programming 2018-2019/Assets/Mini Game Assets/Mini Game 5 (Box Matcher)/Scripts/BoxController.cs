using UnityEngine;

public class BoxController : MonoBehaviour {

    [SerializeField] private int desiredTagNum;
    [SerializeField] private MiniGame6UI UI;


    // when collides with a Box with the correct tag, calls function on UI
    // if not, calls a dif function on UI
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Box/" + desiredTagNum.ToString())
        {
            UI.GoodBox();
        } else 
        {
            UI.BadBox();
        }
    }
}
