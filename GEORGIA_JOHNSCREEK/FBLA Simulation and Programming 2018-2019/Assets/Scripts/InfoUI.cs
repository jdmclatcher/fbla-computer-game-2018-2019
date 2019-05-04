using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoUI : MonoBehaviour {

    private void Start()
    {
        curtainClose.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            // start routine when press escape
            StartCoroutine(ILeave());
        }
    }

    [SerializeField] private GameObject curtainClose;

    IEnumerator ILeave()
    {
        curtainClose.SetActive(true); // set curtain active
        // get ref to anim on curtain
        Animation curtainAnim = curtainClose.GetComponent<Animation>();
        curtainAnim.Play("Curtain Close"); // play anim

        // wait until anim is done playing
        yield return new WaitUntil(() => curtainAnim.IsPlaying("Curtain Close") == false);

        // then leave scene
        SceneManager.LoadScene("Title"); // return to main menu
    }
    


}
