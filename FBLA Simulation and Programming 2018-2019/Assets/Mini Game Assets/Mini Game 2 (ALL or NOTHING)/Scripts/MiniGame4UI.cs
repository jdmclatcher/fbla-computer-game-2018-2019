using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGame4UI : MonoBehaviour {

    // game that has one question - ALL OR NOTHING

    // either get a lot of rep points, or you get no rep points

    // lots of hype and build up to this one question

    // you are in front of an audience and are standing with a mic

    // cutscene plays if you get it correct, panning around the room with victory signs

    #region Countdown

    [Header("Countdown")]
    [SerializeField] private TextMeshProUGUI countdownText;

    // co-routine that counts down from 3
    IEnumerator ICountdown()
    {
        yield return new WaitForSeconds(0.9f);
        countdownText.text = "2";
        yield return new WaitForSeconds(0.9f);
        countdownText.text = "1";
        yield return new WaitForSeconds(0.9f);
        countdownText.text = "GO!";
        yield return new WaitForSeconds(0.9f);
        // init stuff
        // start couroutine to spawn and check value of answer
        StartCoroutine(IGameUIStart());
    }

    #endregion

    #region Start and Update

    private void Start()
    {
        timer.SetActive(true);
        // set active camera to be the main one
        main.enabled = true;
        bRoll.SetActive(false);
        bRoll.GetComponent<Camera>().enabled = false;
        // disbale victory stuff by default
        victoryStuff.SetActive(false);
        gameEnd.SetActive(false);
        // disable game UI
        gameUI.SetActive(false);
        // set local values
        instructionsTextLOCAL = instructionsText.text;
        // start countdown at beginning of game
        StartCoroutine(ICountdown());  
    }
    #endregion

    #region TextAnimation

    [Header("Text Dot Dot Dot Anim")]
    [SerializeField] private TextMeshProUGUI instructionsText;
    [SerializeField] private float waitInterval;
    private string instructionsTextLOCAL; // local copy of the unchanged instructions

    IEnumerator ICycleDotDotDot(TextMeshProUGUI textToDot, string localCopy)
    {
        while (!hasAnswered)
        {
            yield return new WaitForSeconds(waitInterval);
            textToDot.text = localCopy + ".";
            yield return new WaitForSeconds(waitInterval);
            textToDot.text = localCopy + "..";
            yield return new WaitForSeconds(waitInterval);
            textToDot.text = localCopy + "...";
            yield return new WaitForSeconds(waitInterval);
            textToDot.text = localCopy;
            yield return new WaitForSeconds(waitInterval);
        }
    }


    #endregion

    #region Gameplay

    [Header("Gameplay")]
    [SerializeField] private GameObject gameUI;
    [SerializeField] private int repPointsToAdd;
    [SerializeField] private GameObject gameEnd;
    [SerializeField] private TextMeshProUGUI repPointsText;

    // whether player got question correct
    [SerializeField] private TextMeshProUGUI questionResultText;
    [SerializeField] private string correctString;
    [SerializeField] private string incorrectString;
    [SerializeField] private string timeRanOutString;
    [SerializeField] private GameObject victoryStuff;
    [SerializeField] private Camera main;
    [SerializeField] private GameObject bRoll;
    [SerializeField] private int panDuration;
    [SerializeField] private float secondsToWait;
    [SerializeField] private Button[] buttons;
    [SerializeField] private GameObject timer;

    private bool correct;
    private bool hasAnswered = false;

    

    public void InCorrectAnswer(Button button)
    {
        button.GetComponent<Image>().color = Color.red;
        Debug.Log("Oops. That's not the correct answer.");
        timer.SetActive(false);

        StartCoroutine(IWaitDefeat()); // wait before defeat

    }

    public void CorrectAnswer(Button button)
    {
        button.GetComponent<Image>().color = Color.green;
        Debug.Log("Congrats! You got it correct!");
        timer.SetActive(false);

        StartCoroutine(IWaitVictory()); // wait before victory

    }

    IEnumerator IWaitVictory()
    {
        foreach (Button button in buttons)
        {
            button.interactable = false; // disbale being able to click on each button
        }
        yield return new WaitForSeconds(secondsToWait);

        correct = true;
        hasAnswered = true;
    }

    IEnumerator IWaitDefeat()
    {
        buttons[0].GetComponent<Image>().color = Color.green; // show the right answer even after defeat
        foreach (Button button in buttons)
        {
            button.interactable = false; // disbale being able to click on each button
        }
        yield return new WaitForSeconds(secondsToWait);

        correct = false;
        hasAnswered = true;
    }

    IEnumerator IGameUIStart()
    {
        // enable game UI
        gameUI.SetActive(true);
        // co-routine that appends a animated ... to instructions 
        StartCoroutine(ICycleDotDotDot(instructionsText, instructionsTextLOCAL));

        // if hasnt reached the part where player answered, the results will be that they ran out of time
        questionResultText.text = timeRanOutString;
        repPointsText.text = "0";

        yield return new WaitUntil(() => hasAnswered == true);

        if (correct)
        {
            // add rep points, then end game
            PlayerPrefs.SetInt("Rep Points", PlayerPrefs.GetInt("Rep Points") + repPointsToAdd);
            // set rep points and result accordingly
            repPointsText.text = repPointsToAdd.ToString();
            questionResultText.text = correctString;
            // start victory co-routine
            StartCoroutine(IVictoryCutscene());
            
        } else
        {
            // set rep points and result accordingly
            repPointsText.text = "0";
            questionResultText.text = incorrectString;
            // end game
            EndGame();
        }

    }

    IEnumerator IVictoryCutscene()
    {
        // spawn in victory stuff
        victoryStuff.SetActive(true);
        // change camera to panning camera
        main.enabled = false;
        bRoll.SetActive(true);
        bRoll.GetComponent<Camera>().enabled = true;
        // wait a certain amount of seconds
        gameUI.SetActive(false);
        yield return new WaitForSeconds(panDuration);
        // then activate the game end screen
        gameEnd.SetActive(true);

    }

    public void EndGame()
    {
        // activate game completed screen
        gameUI.SetActive(false);
        gameEnd.SetActive(true);
    }

    public void ExitAndReturn()
    {
        // return to main world
        SceneManager.LoadScene("World");
    }

    #endregion
}
