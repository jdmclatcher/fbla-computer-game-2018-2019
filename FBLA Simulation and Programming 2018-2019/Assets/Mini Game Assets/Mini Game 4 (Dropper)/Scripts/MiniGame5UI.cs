using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MiniGame5UI : MonoBehaviour
{

    
    /*
     * 
     * 
     * ALL about competitive events - not really, only a little bit about em
     * 
     * a certain topic is given, and you have to catch the falling items that relate to that topic
     * catch as many as you can before you get a certain number incorrect
     * 
     * THINGS NEEDED
     *  a spawner to drop corrrect and incorrect items
     *  a UI that displays info about what the topic is
     *  a board/catcher that will be controllable and will be able to collide with the spawned objects
     *  
     *  
     *  TODO fix box collider with edgy being riggidy
     *  TODO actually add stuff to the dropped items
     *      true or false statements, collect the true, avoid the false
     *  
    */


    #region Countdown
    [Header("Countdown")]
    [SerializeField] private GameObject countdown;
    [SerializeField] private GameObject curtain;

    IEnumerator ICountdown()
    {
        thePlayer.canMove = false;
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
        spawner.SetActive(true); // game begins when spawner becomes active
        // game begin 
        thePlayer.canMove = true;
        helpText.SetActive(true);// enable help text
    }


    #endregion

    #region Start and Update

    [SerializeField] private DropperPlayerController thePlayer;
    [SerializeField] private GameObject helpText;

    private void Start()
    {
        // pre game setup
        // thePlayer = FindObjectOfType<DropperPlayerController>(); // get ref to player
        helpText.SetActive(false);
        curtain.SetActive(true);
        countdown.SetActive(false);
        gameEndScreen.SetActive(false);
        spawner.SetActive(false);
        livesCounter.text = maxErrors.ToString();
        StartCoroutine(ICountdown());
    }

    #endregion

    #region Gameplay
    [Header("Gameplay")]
    [SerializeField] private GameObject spawner;
    [SerializeField] private int errorCount = 0;
    [SerializeField] private int maxErrors;
    [SerializeField] private int repPointsPerAnswer;
    [SerializeField] private int repPointsPerExtraLife;
    [HideInInspector] public int objectsLength;
    private int repPoints;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI livesCounter;
    [SerializeField] private TextMeshProUGUI repPointsText;
    [SerializeField] private GameObject gameEndScreen;
    [SerializeField] private TextMeshProUGUI numMissedText;

    public void AddCorrect()
    {
        // add to total rep points
        repPoints += repPointsPerAnswer;
        // decrease amount of objects left in array
        objectsLength--;

        // end game if the length of the array is 0
        // (no more objects left to spawn)
        if (objectsLength <= 0)
        {
            EndGame();
        }
    }

    public void AddError()
    {
        // decrease amount of objects left in array
        objectsLength--;
        // prevent errors from being more than the max errors
        if(errorCount >= maxErrors)
        {
            errorCount = maxErrors;
            EndGame();
        } else
        {
            // update error count
            errorCount++;
        }
        // update lives count based on errors had 
        livesCounter.text = (maxErrors - errorCount).ToString();

        // end game if the length of the array is 0
        // (no more objects left to spawn)
        if (objectsLength <= 0)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        Debug.Log("Game Over.");
        spawner.SetActive(false); // stop spawning
        thePlayer.canMove = false;

        // add to rep points the number of extra lives after
        // all objects have been spawned
        repPoints += (maxErrors - errorCount) * repPointsPerExtraLife;

        gameEndScreen.SetActive(true);
        repPointsText.text = repPoints.ToString();

        numMissedText.text = errorCount.ToString(); // get total number missed

        PlayerPrefs.SetInt("Rep Points", PlayerPrefs.GetInt("Rep Points") + repPoints);

    }

    public void ExitAndReturn()
    {
        // return to main world
        SceneManager.LoadScene("World");
    }

    #endregion

}

