using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndingUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI supportersVal;

    private void Start()
    {
        Cursor.visible = true; // reactivate the cursor
        // update final supporters
        supportersVal.text = PlayerPrefs.GetInt("Supporters").ToString();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            PlayerPrefs.DeleteAll(); // reset all player prefs when they uit the game
            Application.Quit(); // quit the game
        }
    }

    public void PlayAgain()
    {
        PlayerPrefs.DeleteAll(); // delete player prefs
        SceneManager.LoadScene("Title"); // return to title screen
    }
}
