using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class StartingUI : MonoBehaviour {

    [SerializeField] private GameObject curtainClose;

    private void Start()
    {
        curtainClose.SetActive(false); // disable by default
    }

    public void PlayGame()
    {
        // run routine
        StartCoroutine(ILoadScene("World"));
        // SceneManager.LoadScene("World");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadHowTo()
    {
        // run routine
        StartCoroutine(ILoadScene("Info"));
        // SceneManager.LoadScene("Info");
    }

    IEnumerator ILoadScene(string sceneToLoad)
    {
        curtainClose.SetActive(true); // set curtain active
        // get ref to anim on curtain
        Animation curtainAnim = curtainClose.GetComponent<Animation>();
        curtainAnim.Play("Curtain Close"); // play anim

        // wait until anim is done playing
        yield return new WaitUntil(() => curtainAnim.IsPlaying("Curtain Close") == false);

        // then leave scene
        SceneManager.LoadScene(sceneToLoad); // loads desired scene
    }
	
}
