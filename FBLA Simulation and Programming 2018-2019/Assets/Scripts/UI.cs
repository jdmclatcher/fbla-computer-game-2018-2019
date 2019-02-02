using UnityEngine;  
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    // TODO add transitions to mini game scenes using image BGs and anims
    // TODO add textmeshpro to sub-ui text elements
    // TODO add a credits slide to cite information - national website, national dress code, etc...

    [SerializeField] private TextMeshProUGUI supportersText;
    public int supportersToFinish1;
    public int supportersToFinish2;
    public int supportersToFinish3;


    #region Start and Update
    private void Start()
    {
        // enable world canvas
        mainWorldCanvas.gameObject.SetActive(true);
        miniGameUI.SetActive(false);
        // use PlayerPrefs to get the value of the rep points
        // every time the scene is loaded
        repPoints = PlayerPrefs.GetInt("Rep Points");
        repPointsText.text = repPoints.ToString();

        SetSliderBounds();
        
    }

    #endregion

    #region Mini-Game

    private const string miniGameScene = "MiniGame"; // constant

    // main UI
    [SerializeField] private GameObject miniGameUI;

    // sub UIs - Array List
    [SerializeField] private List<GameObject> miniGames;

    [Header("Player Stats")]
    // main UI to display rep points
    [SerializeField] private TextMeshProUGUI repPointsText;
    private int repPoints;

    public bool freeToMove = false;
    [HideInInspector] public int gameNumber;

    [Header("Mini Game Main Stats")]
    [SerializeField] private TextMeshProUGUI repPointsMainText;
    [SerializeField] private TextMeshProUGUI supportersMainText;

    [SerializeField] private Canvas mainWorldCanvas;

    public void ShowMiniGameUI()
    { 
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
        Debug.Log(gameNumber);
        // converts that int to string to be the suffix for the mini-game
        SceneManager.LoadScene(miniGameScene + gameNumber.ToString());
    }

    // when the player presses the QUIT option
    public void QuitMiniGameUI()
    {
        // enable world canvas again
        mainWorldCanvas.gameObject.SetActive(true);
        Debug.Log(gameNumber);
        Cursor.visible = false; // hide the cursor again
        // disable popup UI by finding the gameObject by name
        GameObject.Find("MiniGame" + gameNumber.ToString()).SetActive(false);

        // disable main UI after
        miniGameUI.SetActive(false);
        freeToMove = true;
    }

    #endregion

    #region Slider/Rep Points to Supporters

    [Header("Slider")]
    [SerializeField] private Slider slider;
    [SerializeField] private int repPointsBoundsInc;
    [SerializeField] private TextMeshProUGUI minVal;
    [SerializeField] private TextMeshProUGUI maxVal;

    // function that will determine what the slider should look like
    // based on rep points total
    private void SetSliderBounds()
    {
        if (repPoints > slider.maxValue)
        {
            // add 1 to player prefs supporters
            PlayerPrefs.SetInt("Supporters", PlayerPrefs.GetInt("Supporters") + 1);
            // add increase in rep points to min and max value
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
        supportersText.text = PlayerPrefs.GetInt("Supporters").ToString();
    }

    #endregion

}
