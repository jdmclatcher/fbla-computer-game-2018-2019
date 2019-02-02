using System.Collections;
using UnityEngine;
using TMPro;

public class MiniGame8UI : MonoBehaviour {

    /*
     * TODO make easy mini game
     * 
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
}
