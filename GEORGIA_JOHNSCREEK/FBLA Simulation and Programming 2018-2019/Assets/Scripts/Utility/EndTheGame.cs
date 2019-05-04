using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndTheGame : MonoBehaviour {

    [SerializeField] private float timeToWait;
    [SerializeField] private string endingScene;

    public void Start()
    {
        StartCoroutine(IEnd()); // start coroutine at start
    }

    IEnumerator IEnd()
    {
        yield return new WaitForSeconds(timeToWait); // wait...

        SceneManager.LoadScene(endingScene); // then load scene
    }

}
