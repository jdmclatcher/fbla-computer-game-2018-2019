using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MiniGame8UI : MonoBehaviour {

    /*
     * TODO make easy mini game
     * 
     * matching game - terms on the left and defs on the right
     * 
    */

    #region Countdown

    [Header("Countdown")]
    [SerializeField] private TextMeshProUGUI countdownText;

    IEnumerator ICountdown()
    {
        yield return new WaitForSeconds(0.9f);
        countdownText.text = "2";
        yield return new WaitForSeconds(0.9f);
        countdownText.text = "1";
        yield return new WaitForSeconds(0.9f);
        countdownText.text = "GO!";
        yield return new WaitForSeconds(0.9f);
        // game begin 
        gameStuff.SetActive(true);
    }

    #endregion

    #region Start and Update
    private void Start()
    {
        repPoints = totalStartingPoints; // rep points equal amount to start with
        gameEndScreen.SetActive(false);
        gameStuff.SetActive(false);
        // start countdown at beginning
        StartCoroutine(ICountdown());
    }

    private void Update()
    {

    }
    #endregion

    #region Gameplay
    [Header("Game Specific")]
    [SerializeField] private int pointsLostPerWrong;
    [SerializeField] private int totalStartingPoints;
    // hard coded in editor, for getting specific correct vals
    [SerializeField] private int[] correctVals;

    private int numberIncorrect = 0;
    private int repPoints = 0; // init as 0
    private int correctVal = 0; // init as 0

    [Header("UI Elements")]
    [SerializeField] private GameObject gameStuff;
    [SerializeField] private Dropdown[] dropdowns; // array of all the dropdowns
    [SerializeField] private GameObject gameEndScreen;
    [SerializeField] private TextMeshProUGUI repPointText;
    [SerializeField] private TextMeshProUGUI numberMissedText;


    public void CheckAnswers(Button button)
    {
        button.interactable = false; // disbale button so they cant spam it
        // called from button, evaluates each answer
        int correctCountTEMP = 0; // reset temp correct count 
        int x = 0; // for tracking dropdowns
        // drop down length is the same as correct vals length
        // get the correct value
        for (int y = 0; y < correctVals.Length; y++)
        {
            // set correct val to the correct value in the current array
            correctVal = correctVals[y];
           
            // check to see if correct option is selected
            // (.value is base 0)
            if (dropdowns[x].value == correctVal)
            {
                Debug.Log("Thats Correct!");
                correctCountTEMP++; // if this number reaches the number of dropdowns, end the game
            }
            else
            {
                numberIncorrect++; // increase count of questions missed
                Debug.Log("Incorrect.");
            }
            
            x++; // increase x value for tracking dropdowns
        } 

        if(correctCountTEMP >= correctVals.Length)
        {
            // take away points
            for (int i = 0; i < numberIncorrect; i++)
            {
                RemovePoints(); // remove points for every missed one
            }
            GameEnd(); // end the game
        }
    }

    // re-enable the button
    public void OnChange(Button button)
    {
        button.interactable = true;
    }

    // remove points from rep points
    private void RemovePoints()
    {
        repPoints -= pointsLostPerWrong;
    }

    private void GameEnd()
    {
        // TODO activate game end screen
        // update texts
        numberMissedText.text = numberIncorrect.ToString();
        repPointText.text = repPoints.ToString();
        gameEndScreen.SetActive(true); // set the end screen active
    }

    public void ExitAndReturn()
    {
        // go back to main world
        SceneManager.LoadScene("World");
    }

    #endregion 
}
