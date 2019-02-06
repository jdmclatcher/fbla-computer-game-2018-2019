using UnityEngine.SceneManagement;
using UnityEngine;

public class StartingUI : MonoBehaviour {


    public void PlayGame()
    {
        SceneManager.LoadScene("World");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadHowTo()
    {
        SceneManager.LoadScene("Info");
    }
	
}
