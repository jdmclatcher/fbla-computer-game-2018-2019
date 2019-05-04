using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour {

    private TextMeshProUGUI timerText;
    [SerializeField] private float gameTime;
    private float gameTimeLOCAL;
    [SerializeField] private MiniGame4UI ui;

	private void Start ()
    {
        gameTimeLOCAL = gameTime; // set local to time value
        timerText = GetComponent<TextMeshProUGUI>(); // get the attached text object
        // ui = FindObjectOfType<MiniGame4UI>(); // find reference to game ui script
	}
	
	private void Update ()
    {
        // decrease time (only while still answering) and call end game when it reaches 0
        gameTimeLOCAL -= Time.deltaTime;

        if (gameTimeLOCAL <= 0)
        {
            ui.EndGame();
        }
        timerText.text = gameTimeLOCAL.ToString("0"); // set value of text component (in an int)
    }
}
