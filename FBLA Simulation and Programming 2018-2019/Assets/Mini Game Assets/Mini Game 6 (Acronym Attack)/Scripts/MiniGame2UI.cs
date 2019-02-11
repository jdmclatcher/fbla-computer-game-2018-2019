using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class MiniGame2UI : MonoBehaviour {

    // game that will test player's knowldge of common FBLA acronyms

    // will present player with big letters in the center and they will have to type it
    // as fast as they can and press enter for each word they type in the acronym

    // grading will be a specific acronym based on the person's deviation from the correct word
    // commonly incorrect aswers may yield more points than a completely incorrect answer

    // array that runs the length of the spawned in gameObject consisting of acronyms


    // private variables
    private int repPoints;
    private float timeLeft;
    private GameObject currentAcronym;
    private bool paused;

    [Header("Game Details")]
    [Tooltip("for each letter correct")][SerializeField] private int pointsPerResponse;
    [Tooltip("in seconds")][SerializeField] private float gameTime;
    

    [Header("Acronyms and Spawning")]
    [SerializeField] private RectTransform spawnParent;
    [SerializeField] private List<GameObject> acronyms;
    [SerializeField] private GameObject spawnLocation;

    
    [Header("UI Elements")]
    [SerializeField] private InputField inputField;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameObject endGameScreen;
    [SerializeField] private TextMeshProUGUI repPointsText;
    [SerializeField] private TextMeshProUGUI lettersIncorrectText;
    [SerializeField] private GameObject countdown;
    [SerializeField] private GameObject curtain;
    [SerializeField] private GameObject game;
    [SerializeField] private RectTransform checkXSpawnPos;
    [SerializeField] private GameObject check;
    [SerializeField] private GameObject x;
    [SerializeField] private TextMeshProUGUI correctAnswerText;
    [SerializeField] private Transform arrowPointer;
    [SerializeField] private Transform arrowPointerSpawnPoint;
    [SerializeField] private float distanceToMovePointer;
    [SerializeField] private AudioSource clickSound;
    [SerializeField] private AudioSource wooshSound;
    [SerializeField] private AudioSource gameEndSound;

    #region Start and Update
    private void Start()
    {
        curtain.SetActive(true);
        countdown.SetActive(false);
        timeLeft = gameTime; // set current time to the start time defined in editor
        paused = true;
        game.SetActive(false); // disable games objects at the beginning
        StartCoroutine(_countdown());
    }

    private void Update()
    {

        // count down by seconds
        if(!paused)
        {
            timeLeft -= Time.deltaTime;
        }
        
        
        // if the countdown is 0, then stop the game
        if(timeLeft <= 0)
        {
            GameEnd();
        }

        // round the time float and then converts to string to display on UI text
        countdownText.text = Mathf.RoundToInt(timeLeft).ToString();
    }

    // co-routine that counts down from 3
    IEnumerator _countdown()
    {
        endGameScreen.SetActive(false);
        // wait until curtain is open, then start coutdown
        yield return new WaitUntil(() => curtain.activeInHierarchy == false);
        countdown.gameObject.SetActive(true); // then set coutdown to active

        // ref to text object
        TextMeshProUGUI countdownText = countdown.GetComponent<TextMeshProUGUI>();
        yield return new WaitForSeconds(0.9f);
        countdownText.text = "2";
        yield return new WaitForSeconds(0.9f);
        countdownText.text = "1";
        yield return new WaitForSeconds(0.9f);
        countdownText.text = "GO!";
        yield return new WaitForSeconds(0.9f);
        countdown.gameObject.SetActive(false); // stop countdown
        paused = false;
        SpawnAcronym(); // spawns first acronym
        game.SetActive(true); // enable games objects
        // immediately put focus on the input field
        inputField.Select();
        inputField.ActivateInputField();

        // set active and reset the arrow
        arrowPointer.gameObject.SetActive(true);
        arrowPointer.position = arrowPointerSpawnPoint.position;

    }

    #endregion

    #region Response Handling UTIL

    // spawns a random acronym from the list of acronyms
    public void SpawnAcronym()
    {
        wooshSound.Play();
        // sets a value to a random number from 0 to the range of the array of gameobjects
        int randNum = Random.Range(0, acronyms.Count);
        // count to determine what run of the array it is on
        int count = 0;
        // loops through the array of GameObjects until the count matches the random number
        // this provides complete randomness

        if (acronyms.Count == 0)
        {
            // only will run if the game end screen has not been
            // enabled
            if (!endGameScreen.activeInHierarchy)
            {
                GameEnd();
            } else
            {
                // otherwise, return
                return;
            }
            
        }

        foreach (GameObject acronym in acronyms)
        {
            if (count == randNum)
            {
                // creates a new clone of the specified gameobject acronym in the hierarchy
                // makes it a child of the parent container for the acronyms
                Instantiate(acronym, spawnLocation.transform.position, spawnLocation.transform.rotation, spawnParent);
                // reference to the currently spawned gameobject (acronym)
                currentAcronym = acronym;
            }
            count++;
        }
    }

    // how many times the player has pressed
    // the enter button
    private int checkCount = 1; // start at 1

    private int lettersIncorrect = 0; // finite count of letters missed

    

    public void ChangeCase()
    {
        inputField.text = inputField.text.ToLower();
    }
    
    private void DeleteAcronym(string tag)
    {
        GameObject oldAcronym = GameObject.FindGameObjectWithTag(tag);
        oldAcronym.SetActive(false);

        // loop through array until passed in tag object is found and delete 
        for (int i = 0; i < acronyms.Count; i++)
        {
            if(acronyms[i].tag == tag)
            {
                acronyms.Remove(acronyms[i]);
            }
        }
    }

    private void AddRepPoints(int repPointsToAdd)
    {
        // add custom number of rep points to the total rep points
        repPoints += repPointsToAdd;
    }

    private void GameEnd()
    {
        gameEndSound.Play();
        repPointsText.text = repPoints.ToString();
        lettersIncorrectText.text = lettersIncorrect.ToString();

        inputField.enabled = false; // disable input field
        arrowPointer.gameObject.SetActive(false); // disable arrow

        endGameScreen.SetActive(true);
        paused = true; // stops timer

        

        // decativate blinker underline
        // MISSING
        

    }

    // return to main world
    public void ExitGame()
    {
        // adds the amount of rep points won in game to the total amount 
        // of rep points right before leaving the game

        PlayerPrefs.SetInt("Rep Points", (PlayerPrefs.GetInt("Rep Points") + repPoints));

        SceneManager.LoadScene("World");
    }

    #endregion

    #region Response Switch Statements

    public void CheckResponse()
    {
        // switch statement to handle what to check based on object tags
        switch (currentAcronym.tag)
        {
            case "Acronyms/FBLA":
                Check4("future", "business", "leaders", "america", "FBLA");
                break;
            case "Acronyms/FLC":
                Check3("fall", "leadership", "conference", "FLC");
                break;
            case "Acronyms/SLC":
                Check3("state", "leadership", "conference", "SLC");
                break;
            case "Acronyms/NLC":
                Check3("national", "leadership", "conference", "NLC");
                break;
            case "Acronyms/EMERGE":
                Check6("educating", "members", "early", "regarding", "georgia's", "economy", "EMERGE");
                break;
            case "Acronyms/ML":
                Check2("middle", "level", "ML");
                break;
            case "Acronyms/PD":
                Check2("professional", "division", "PD");
                break;
        }
    }

    private void Check2(string response1, string response2, string ID)
    {
        switch (checkCount)
        {
            case 1:
                Check(response1);
                break;
            case 2:
                CheckFinal(response2, ID);
                break;
            default:
                Debug.Log("Already finished");
                break;
        }
        inputField.text = null;

        // put focus on input field
        inputField.Select();
        inputField.ActivateInputField();
    }

    private void Check3(string response1, string response2, string response3, string ID)
    {
        // call a check every time, but pass in different value to 
        // evaluate based on checkCount number
        switch (checkCount)
        {
            case 1:
                Check(response1);
                break;
            case 2:
                Check(response2);
                break;
            case 3:
                CheckFinal(response3, ID);
                break;
            default:
                Debug.Log("Already finished");
                break;
        }
        inputField.text = null;

        // put focus on input field
        inputField.Select();
        inputField.ActivateInputField();
    }

    private void Check4(string response1, string response2, string response3, string response4, string ID)
    {
        switch (checkCount)
        {
            case 1:
                Check(response1);
                break;
            case 2:
                Check(response2);
                break;
            case 3:
                Check(response3);
                break;
            case 4:
                CheckFinal(response4, ID);
                break;
            default:
                Debug.Log("Already finished");
                break;
        }
        inputField.text = null;

        // put focus on the input field
        inputField.Select();
        inputField.ActivateInputField();
    }

    private void Check6(string response1, string response2, string response3, string response4, string response5, string response6, string ID)
    {
        switch (checkCount)
        {
            case 1:
                Check(response1);
                break;
            case 2:
                Check(response2);
                break;
            case 3:
                Check(response3);
                break;
            case 4:
                Check(response4);
                break;
            case 5:
                Check(response5);
                break;
            case 6:
                CheckFinal(response6, ID);
                break;
            default:
                Debug.Log("Already finished");
                break;
        }
        inputField.text = null;

        // put focus on the input field
        inputField.Select();
        inputField.ActivateInputField();
    }
    
    // evaluates just one answer
    private void Check(string response)
    {
        clickSound.Play();
        if (inputField.text == response)
        {
            // add points
            AddRepPoints(pointsPerResponse);
            // spawn in a check mark
            Instantiate(check, checkXSpawnPos.position, checkXSpawnPos.rotation, checkXSpawnPos);
            Debug.Log("That's correct");
        }
        else
        {
            // spawn in an X mark
            Instantiate(x, checkXSpawnPos.position, checkXSpawnPos.rotation, checkXSpawnPos);
            // show text to correct answer
            StartCoroutine(IShowCorrect(response));
            lettersIncorrect++;
            Debug.Log("Incorrect.");
        }

        if (arrowPointer != null)
        {
            arrowPointer.transform.Translate(distanceToMovePointer, 0, 0); // move over certain units
        }

        checkCount++; // increase the check count because it has happened once more
    }

    // last check for one answer - does extra stuff Check() doesn't
    private void CheckFinal(string response, string ID)
    {
        clickSound.Play();
        if (inputField.text == response)
        {
            // add points
            AddRepPoints(pointsPerResponse);
            // spawn in a check mark
            Instantiate(check, checkXSpawnPos.position, checkXSpawnPos.rotation, checkXSpawnPos);
            Debug.Log("That's correct");
        }
        else
        {
            // spawn in an X mark
            Instantiate(x, checkXSpawnPos.position, checkXSpawnPos.rotation, checkXSpawnPos);
            // show text to correct answer
            StartCoroutine(IShowCorrect(response));
            lettersIncorrect++;
            Debug.Log("Incorrect.");
        }

        DeleteAcronym("Acronyms/" + ID);

        // reset position
        if(arrowPointer != null)
        {
            arrowPointer.position = arrowPointerSpawnPoint.position;
        }

        SpawnAcronym();
        checkCount = 1;

        // reset pointer position
        // MISSING
    }

    IEnumerator IShowCorrect(string correctAnswer)
    {
        correctAnswerText.text = correctAnswer; // text shows correct answer

        yield return new WaitForSeconds(0.75f); // wait about a second

        correctAnswerText.text = ""; // text shows nothing
    }

    #endregion

}
