using UnityEngine;

public class EndingUI : MonoBehaviour {

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit(); // quit the game
        }
    }
}
