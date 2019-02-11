using UnityEngine;  
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour {

    // TODO add a credits slide to cite information - national website, national dress code, etc...

 
    // TOFIX only the main mini game items are getting the exit animation
    
    [SerializeField] private TextMeshProUGUI supportersText;

    

    [SerializeField] private GameObject pauseUI;
    private bool isPaused; // check if game is paused or not
    [SerializeField] private Player thePlayer; // ref to player
    [SerializeField] private Slider pausedSensSlider; // ref to slider for sens setting
    [SerializeField] private AudioSource clickSound;

    public bool freeToMove = false;

    #region Start and Update
    private void Start()
    {
        freeToMove = false;
        fadeToBlack.SetActive(false);
        // setup cameras
        endingCamera.enabled = false;
        endingCamera.GetComponent<AudioListener>().enabled = false;
        mainCamera.enabled = true;
        mainCamera.GetComponent<AudioListener>().enabled = true;

        // enable world canvas
        mainWorldCanvas.gameObject.SetActive(true);
        miniGameUI.SetActive(false);
        // use PlayerPrefs to get the value of the rep points
        // every time the scene is loaded
        repPoints = PlayerPrefs.GetInt("Rep Points");
        repPointsText.text = repPoints.ToString();

        SetSliderBounds();
        
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    #endregion

    private void PauseGame()
    {
        pausedSensSlider.value = PlayerPrefs.GetInt("Sens"); // set val of slider
        // do stuff based on if game is paused or not
        if (isPaused)
        {
            // PlayerPrefs.SetInt("Sens", (int) pausedSensSlider.value); // set sens to slider value
            // thePlayer.UpdateSens();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
            pauseUI.SetActive(false);
            isPaused = false;
        } else
        {
            // PlayerPrefs.SetInt("Sens", (int) pausedSensSlider.value); // set sens to slider value
            // thePlayer.UpdateSens();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            pauseUI.SetActive(true);
            isPaused = true;
        }
        
    }

    // when changed
    public void OnSensSliderChange()
    {
        PlayerPrefs.SetInt("Sens", (int)pausedSensSlider.value); // set sens to slider value
        thePlayer.UpdateSens(); // then update sens
    }

    public void QuitGame()
    {
        PlayerPrefs.SetInt("Sens", (int)slider.value); // set sens to slider value
        Time.timeScale = 1f; // resume normal time
        // Application.Quit(); // TEMP
        SceneManager.LoadScene("Title"); // go back to title screen
    }

    #region Mini-Game

    private const string miniGameScene = "MiniGame"; // constant

    // main UI
    [SerializeField] private GameObject miniGameUI;

    // sub UIs - Array List
    [SerializeField] private List<GameObject> miniGames;

    // important object that will load scene after animation is played
    [SerializeField] private OpenSceneAfterTime curtainController;

    [Header("Player Stats")]
    // main UI to display rep points
    [SerializeField] private TextMeshProUGUI repPointsText;
    private int repPoints;

    // public bool freeToMove = false;
    [HideInInspector] public int gameNumber = 0;

    [Header("Mini Game Main Stats")]
    [SerializeField] private TextMeshProUGUI repPointsMainText;
    [SerializeField] private TextMeshProUGUI supportersMainText;

    [SerializeField] private Canvas mainWorldCanvas;

    [Header("Extras")]
    [SerializeField] private GameObject miniGameItemsUI; // for anims
    public GameObject alreadyPlayedWarning;

    public void ShowMiniGameUI()
    {
        clickSound.Play(); // play SFX
        freeToMove = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; // show the cursor
        // disable world canvas
        mainWorldCanvas.gameObject.SetActive(false);
        // update text fields
        repPointsMainText.text = repPoints.ToString();
        supportersMainText.text = PlayerPrefs.GetInt("Supporters").ToString();
        
        miniGameUI.SetActive(true);
        


        // for loop that runs throught the length of mini-games, starting at 1
        for (int i = 1; i <= miniGames.Count; i++)
        {
            // activates the mini game UI when it reaches the mini game number
            if(i == gameNumber)
            {
                // because array is index 0, subtract 1 to determine what
                // to activate
                miniGames[i - 1].SetActive(true);
            }
        }
    }

    // loads RANDOM mini-game scene
    public void TransitionMiniGameScenes()
    {
        clickSound.Play(); // play SFX
        // update playerprefs
        PlayerPrefs.SetInt("Games Played", PlayerPrefs.GetInt("Games Played") + 1);

        // set that it has been played
        SetPlayed(gameNumber);

        // load specified scene from curtain 
        curtainController.SetStuff(miniGameScene + gameNumber.ToString());
        Debug.Log(PlayerPrefs.GetInt("Game" + gameNumber + 1));
    }

    public bool CheckPlayed(int gameNumber)
    {
        // checks if game has been played
        if(PlayerPrefs.GetInt("Game" + gameNumber + 1) == 1)
        {
            return true;
        } else {
            return false;
        }
    }

    private void SetPlayed(int gameNumber)
    {
        PlayerPrefs.SetInt("Game" + gameNumber + 1, 1); // set correct mini game played to true
    }

    // when the player presses the QUIT option
    public void QuitMiniGameUI()
    {
        clickSound.Play(); // play SFX
        StartCoroutine(IQuitMiniGameUI()); // run a coroutine
    }

    private IEnumerator IQuitMiniGameUI()
    {
        freeToMove = true; // free movement of the player
        Animation animation = miniGameItemsUI.gameObject.GetComponent<Animation>();
        animation.Play("Canvas Fade Out");

        // play the animation on the specific mini game UI as well
        GameObject specificMiniGameObj = GameObject.Find("MiniGame" + gameNumber.ToString());
        Animation miniAnimation = specificMiniGameObj.GetComponent<Animation>();
        miniAnimation.Play("Canvas Fade Out"); // play anim on mini game specific object as well

        // wait until anim stops
        yield return new WaitUntil(() => animation.IsPlaying("Canvas Fade Out") == false);

        // enable world canvas again
        mainWorldCanvas.gameObject.SetActive(true);
        Debug.Log(gameNumber);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; // hide the cursor again
        // disable popup UI by finding the gameObject by name

 

        specificMiniGameObj.SetActive(false);

        // disable main UI after
        miniGameUI.SetActive(false);
        // freeToMove = true;
        thePlayer.restrained = false; // can move
    }



    #endregion

    #region Slider/Rep Points to Supporters

    [Header("Slider")]
    [SerializeField] private Slider slider;
    [SerializeField] private int repPointsBoundsInc;
    [SerializeField] private TextMeshProUGUI minVal;
    [SerializeField] private TextMeshProUGUI maxVal;
    private int supporters = 0;

    // function that will determine what the slider should look like
    // based on rep points total
    private void SetSliderBounds()
    {
        if (repPoints >= slider.maxValue)
        {
            
            // supporters starts a 0 every time the person loads the world scene
            supporters++; // add one to the supporters every time this function runs
            


            PlayerPrefs.SetInt("Supporters", PlayerPrefs.GetInt("Supporters") + 1);
            // add increase in rep points to min and max value (times the scalar)
            slider.minValue += repPointsBoundsInc;
            slider.maxValue += repPointsBoundsInc;

            SetSliderBounds(); // recursive call to keep increasing, until slider max value exceeds rep points
        }

        // set slider val to number of rep points 
        slider.value = repPoints;

        // set the min and max val text to the respective slider vals
        minVal.text = slider.minValue.ToString();
        maxVal.text = slider.maxValue.ToString();
        

        // every time UI loads, will update supporters text value
        supportersText.text = supporters.ToString();

        // set the player prefs supporters
        PlayerPrefs.SetInt("Supporters", supporters);
    }

    #endregion

    #region Game End/Victory

    [Header("Victory")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera endingCamera;
    [SerializeField] private float camPause;
    public GameObject fadeToBlack;

    // "open the curtain"
    public IEnumerator OpenTheGate(GameObject wall)
    {
        // wall opening animation
        Animation wallAnim = wall.GetComponent<Animation>();
        wallAnim.Play("Wall Open");

        wall.gameObject.GetComponent<BoxCollider>().enabled = false; // disable box trigger

        // cameras
        endingCamera.enabled = true;
        endingCamera.GetComponent<AudioListener>().enabled = true;
        mainCamera.enabled = false;
        mainCamera.GetComponent<AudioListener>().enabled = false;

        // camera sweep animation
        Animation cameraAnim = endingCamera.gameObject.GetComponent<Animation>();
        cameraAnim.Play("Cutscene Wall Cam");

        // wait until camera anim is done
        yield return new WaitUntil(() => cameraAnim.IsPlaying("Cutscene Wall Cam") == false);

        yield return new WaitForSeconds(camPause);


        // reset pos
        cameraAnim.Play("Cam Revert");

        // wait until camera anim is done
        yield return new WaitUntil(() => cameraAnim.IsPlaying("Cam Revert") == false);

        // then reset cameras
        mainCamera.enabled = true;
        mainCamera.GetComponent<AudioListener>().enabled = true;
        endingCamera.enabled = false;
        endingCamera.GetComponent<AudioListener>().enabled = false;

    }

    #endregion

}
