using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class MiniGame6UI : MonoBehaviour {

    /*
     * top down mini game where you have to push boxes to match up key terms with definitions
     * 
     * have a room with some boxes, and you player has to push the correct boxes to the correct slots
     * match the term (boxes) with the definition (slots)
     * based on how fast you can get all the boxes in the slots correctly, no penalty for wrong answers,
     * besides the fact that it slows you down
     * 
     * if you get the box wrong, it will force the box out far enough so you can interact with it again
     * 
     * Box sorter - you have 3 or 4 topics and you have to put the correctly relating boxes in the slots
    */ 

    #region Countdown
    
    [Header("Countdown")]
    [SerializeField] private GameObject countdown;
    [SerializeField] private GameObject curtain;

    IEnumerator ICountdown()
    {
        // cant move before countdown
        thePlayer.restrained = true;
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
        // game begin 
        // can move after countdown
        thePlayer.restrained = false;
    }

    #endregion

    #region Start and Update
    // ref to player
    [SerializeField] private BoxPlayerController thePlayer;

    private void Start()
    {
        curtain.SetActive(true);
        countdown.SetActive(false);
        gameEndScreen.SetActive(false); // disable game ending screen at start
        // thePlayer = FindObjectOfType<BoxPlayerController>();
        // set boxes text and val
        boxesLeft = totalBoxes;
        boxesLeftText.text = totalBoxes.ToString();
        // start countdown at beginning
        StartCoroutine(ICountdown());
    }

    private void Update()
    {
        // track total elapsed time
        if (!thePlayer.restrained) // only track when player can move
        {
            timeElapsed += Time.deltaTime;
            // update text 
            timeElapsedText.text = Mathf.RoundToInt(timeElapsed).ToString();
        }
    }
    #endregion

    #region Gameplay
    [Header("Game Items")]
    [SerializeField] private int totalBoxes;
    [SerializeField] private int startingRepPoints; // rep points to start with
    [SerializeField] private float pointsLostPerSecond; // points lost per second of time elapsed
    [SerializeField] private AudioSource goodSound;
    [SerializeField] private AudioSource badSound;
    [SerializeField] private AudioSource endGameSound;
    private int repPoints; 

    private int boxesLeft;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI boxesLeftText;
    [SerializeField] private TextMeshProUGUI timeElapsedText;
    [SerializeField] private GameObject gameEndScreen;
    [SerializeField] private TextMeshProUGUI finalTimeText;
    [SerializeField] private TextMeshProUGUI repPointsText;

    private float timeElapsed;

    public void GoodBox()
    {
        goodSound.Play();
        // decrease the number of boxes left
        boxesLeft--;
        // update text 
        boxesLeftText.text = boxesLeft.ToString();
        Debug.Log("Good job!");
        
        if(boxesLeft <= 0)
        {
            // end the game
            GameEnd();
        }
    }

    public void BadBox()
    {
        if (!badSound.isPlaying)
        {
            badSound.Play(); // play SFX only if not currently playing
        }
        Debug.Log("Bad job.");
    }

    private void GameEnd()
    {
        endGameSound.Play();
        thePlayer.restrained = true; // stop player which stops clock

        // calculate final rep points
        repPoints = Mathf.RoundToInt(startingRepPoints - (timeElapsed * pointsLostPerSecond));

        repPointsText.text = repPoints.ToString(); // update end rep points text
        finalTimeText.text = Mathf.RoundToInt(timeElapsed).ToString(); // final time
        gameEndScreen.SetActive(true);

        // add rep points to playerprefs
        PlayerPrefs.SetInt("Rep Points", PlayerPrefs.GetInt("Rep Points") + repPoints);
    }

    public void ExitAndReturn()
    {
        // return to main world
        SceneManager.LoadScene("World");
    }


    #endregion

}
