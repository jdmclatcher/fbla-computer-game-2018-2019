using System.Collections;
using UnityEngine;

public class MiniGame3Player : MonoBehaviour {

    [SerializeField] private float slowMoVal;
    [SerializeField] private MiniGame3UI ui;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = 4f;
    }

    private void OnTriggerEnter(Collider other)
    { 
        if(other.tag == "Obstacle Trigger")
        { 
            Time.timeScale = slowMoVal;
            Debug.Log("1");
            // STARTS A COUROUTINE in the UI script
            StartCoroutine(ui.IQuestion(other.gameObject));
        } 

    }

    private void OnTriggerExit(Collider other)
    {
        // revert to regular time scale when exit with collider trigger
        if(other.tag == "Obstacle Trigger")
        {
            Time.timeScale = 1f;
        }
    }

    public void canRun()
    {
        animator.SetBool("canRun", true);
    }

}
