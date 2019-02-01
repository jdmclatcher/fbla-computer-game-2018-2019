using UnityEngine;
using System.Collections;
using TMPro;

public class MiniGame6UI : MonoBehaviour {

    /*
     * TODO top down mini game where you have to push boxes to match up key terms with definitions
     * 
     * have a room with some boxes, and you player has to push the correct boxes to the correct slots
     * match the term (boxes) with the definition (slots)
     * based on how fast you can get all the boxes in the slots correctly, no penalty for wrong answers,
     * besides the fact that it slows you down
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
    }

    #endregion

    #region Start and Update
    private void Start()
    {
        // start countdown at beginning
        StartCoroutine(ICountdown());
    }

    private void Update()
    {
        
    }
    #endregion

    #region
    [Header("Game Items")]
    [SerializeField] private int totalBoxes;

    private int boxesLeft;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI boxesLeftText;


    #endregion

}
