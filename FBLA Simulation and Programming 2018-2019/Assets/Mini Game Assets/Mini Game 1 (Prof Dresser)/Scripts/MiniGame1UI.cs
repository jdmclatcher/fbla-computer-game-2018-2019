using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MiniGame1UI : MonoBehaviour {

    #region Start and Update

    private void Start()
    {
        countdown.gameObject.SetActive(false);
        curtain.gameObject.SetActive(true);
        // co-routine that changes the text value every second
        StartCoroutine(_countdown());
        clothes.SetActive(false);
        repPoints = startingRepPoints; // set the current rep points to the starting
                                       // default amount
        victoryScreen.SetActive(false);
    }

    #endregion

    #region Countdown

    [Header("Countdown")]
    [SerializeField] private GameObject countdown;
    [SerializeField] private float timeToWait;
    [SerializeField] private GameObject clothes;
    [SerializeField] private GameObject curtain;

    // co-routine that counts down from 3
    IEnumerator _countdown()
    {
        // wait until curtain is open, then start coutdown
        yield return new WaitUntil(() => curtain.activeInHierarchy == false);
        countdown.gameObject.SetActive(true); // then set coutdown to active

        // ref to text object
        TextMeshProUGUI countdownText = countdown.GetComponent<TextMeshProUGUI>();
        yield return new WaitForSeconds(timeToWait);
        countdownText.text = "2";
        yield return new WaitForSeconds(timeToWait);
        countdownText.text = "1";
        yield return new WaitForSeconds(timeToWait);
        countdownText.text = "GO!";
        yield return new WaitForSeconds(timeToWait);
        countdown.gameObject.SetActive(false); // stop countdown
        clothes.SetActive(true);
        // START OFFICIAL TIMER
    }

    #endregion

    #region Gameplay

    [SerializeField] private AudioSource clickSound; // click SFX
    [SerializeField] private AudioSource badClickSound; // click SFX

    [Header("Game Stats")]
    [SerializeField] private int numberOfCorrectItems;
    private int count = 0; // number of correct items picked starts at 0
    // starting amount of points when the game begins
    [SerializeField] private int startingRepPoints;
    // the rep points to be transfered to the main UI at the end of the mini game
    private int repPoints;
    [SerializeField] private int penalty;

    [Header("Game Ending")]
    [SerializeField] private GameObject victoryScreen;
    // [SerializeField] private TextMeshProUGUI correctNum;
    [SerializeField] private TextMeshProUGUI incorrectNumText;
    private int incorrectCount = 0; // track number of incorrect items picked
    [SerializeField] private TextMeshProUGUI repPointsFinalText;

    // 2 options for what the player chooses
    public void Correct(GameObject button)
    {
        Debug.Log("Correct!");
        clickSound.Play();
        // SPAWN A CHECK MARK ... 
        //Instantiate(checkSign, button.transform.position, button.transform.rotation);
        //checkSign.transform.SetParent(GameObject.FindGameObjectWithTag("Signs").transform, false);
        button.GetComponent<Button>().interactable = false; // disable button
        button.GetComponent<Image>().color = Color.green; // change to green color
        // increases the count and then checks to see if has picked all the correct items
        count++;
        if(count == numberOfCorrectItems)
        {
            StartCoroutine(IEndGame());
        }
        // not necessary -- 
        // repPointsText.text = repPoints.ToString();
    }

    public void Incorrect(GameObject button)
    {
        Debug.Log("Incorrect!");
        badClickSound.Play();
        // SPAWN A X MARK ... 
        //Instantiate(xSign, button.transform.position, button.transform.rotation);
        //xSign.transform.SetParent(GameObject.FindGameObjectWithTag("Signs").transform, false);
        button.GetComponent<Button>().interactable = false; // disable button
        button.GetComponent<Image>().color = Color.red; // change to red color
        repPoints = repPoints - penalty;
        incorrectCount++;

    }

    IEnumerator IEndGame()
    {
        Debug.Log("You've been game ended.");
        // gameObject.GetComponent<CanvasGroup>().interactable = false; // disable clicking buttons
        yield return new WaitForSeconds(0.2f); // wait half a second before ending game
        // gameObject.GetComponent<CanvasGroup>().interactable = true; // enable clicking buttons

        clothes.SetActive(false); // disable the clothes
        // input point values into victory screen text objects
        incorrectNumText.text = incorrectCount.ToString();
        repPointsFinalText.text = repPoints.ToString();
        victoryScreen.SetActive(true); // activate the ending slide/screen
        // adds the total rep points gained to the main rep points playerpref
        PlayerPrefs.SetInt("Rep Points", PlayerPrefs.GetInt("Rep Points") + repPoints);
    }

    //// function that ends the game when all the correct items have been selected
    //private void EndGame()
    //{
    //    Debug.Log("You've been game ended.");
    //    clothes.SetActive(false); // disable the clothes
    //    // input point values into victory screen text objects
    //    incorrectNumText.text = incorrectCount.ToString();
    //    repPointsFinalText.text = repPoints.ToString();
    //    victoryScreen.SetActive(true); // activate the ending slide/screen
    //    // adds the total rep points gained to the main rep points playerpref
    //    PlayerPrefs.SetInt("Rep Points", PlayerPrefs.GetInt("Rep Points") + repPoints);
    //}

    public void ExitAndReturn()
    {
        // loads the main scene
        SceneManager.LoadScene("World");
    }

    #endregion

}
