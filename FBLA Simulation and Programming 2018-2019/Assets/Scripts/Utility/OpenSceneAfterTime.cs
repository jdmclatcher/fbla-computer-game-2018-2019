using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// SCRIPT THAT WILL OPEN A SCENE AFTER SPECIFIED TIME

public class OpenSceneAfterTime : MonoBehaviour {

    [SerializeField] private float timeToWait;
    private string sceneToLoad;

    private void Start()
    {
        gameObject.SetActive(false); // diable anim and sef by default
    }

    // TODO CHANGE TO UPDATE
    private void Update()
    {
        timeToWait -= Time.deltaTime;
        if(timeToWait <= 0)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    public void SetStuff(string sceneToLoad)
    {
        gameObject.SetActive(true);
        this.sceneToLoad = sceneToLoad;
    }


    // wait and then load scene
    //public IEnumerator IOpen(string sceneToLoad)
    //{
    //    gameObject.SetActive(true); // activate itself
    //    yield return new WaitForSeconds(timeToWait);
        
    //}
    
}
