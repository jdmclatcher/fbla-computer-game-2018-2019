using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameTestManager : MonoBehaviour {

    // Game Objects
    [SerializeField] private GameObject victoryText;
    [SerializeField] private GameObject defeatText;
    [SerializeField] private GameObject curtain;

    [SerializeField] private int numberReds;
    private int numberRedsCount = 0;

    private void Start()
    {
        victoryText.SetActive(false);
        defeatText.SetActive(false);
        curtain.SetActive(false);
    }

    public void ClickOther(GameObject button)
    {
        Lose(); // lose game
        KillButton(button); // calls function to disable button
        //Debug.Log("Other Clicked");
        
    }

    public void ClickRed(GameObject button)
    {
        numberRedsCount++;
        // if the number of times function has been called is more/equal to
        // the total number of reds, win game
        if(numberRedsCount >= numberReds)
        {
            Win(); // win game 
        }
        KillButton(button); // calls function to disable button

        //Debug.Log("Red Clicked");
    }

    private void KillButton(GameObject button)
    {
        button.SetActive(false);
        // probably play an animation or something...
    }

    private void Lose()
    {
        defeatText.SetActive(true);
        // makes it so player can't click the buttons under the "curtain"
        curtain.SetActive(true); 
    }

    private void Win()
    {
        victoryText.SetActive(true);
        curtain.SetActive(true);
    }
}
