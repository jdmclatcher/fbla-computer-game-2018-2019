using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class MiniGame3UI : MonoBehaviour {

    // "RUNNING" for office -- 

    // 2D-ish runner game that will present you with obstacles along the way
    // obstacles 
    //      quesitons about running for office
    //      true of false statements
    //      trivial facts about FBLA history of office

    // backgrounds and obstacles move to create the illusion of player movement

    // getting questions correct will award you rep points
    // and if you miss a certain amount, its game over

    // however, if you get all 10 correct you win the game and get the max amount of points

    // when you get a question correct, you go into slow-mo and get to JUMP over
    // that block/hurdle

    // if you get it incorrect, then you get hit by it and stumble a little

    // Player will play all the questions each time, 
    // but will only get a chance at each one once per run


    // TODO add more questions
    //  these questions need to be about running for office, candidacy, etc..
    
    #region Countdown

    [Header("Countdown")]
    [SerializeField] private TextMeshProUGUI countdown;
    [SerializeField] private GameObject bgSpawner;

    // co-routine that counts down from 3
    IEnumerator _countdown()
    {
        yield return new WaitForSeconds(0.9f);
        countdown.text = "2";
        yield return new WaitForSeconds(0.9f);
        countdown.text = "1";
        yield return new WaitForSeconds(0.9f);
        countdown.text = "GO!";
        yield return new WaitForSeconds(0.9f);
        bgSpawner.SetActive(true);
        SpawnObject();
        SetRandTime();

    }

    #endregion

    #region Start and Update

    private void Start()
    {
        SetRandTime(); // set the init rand time interval for obstacle
        bgSpawner.SetActive(false);
        gameCompletedScreen.SetActive(false);
        correctText.text = questionsCorrect.ToString();
        StartCoroutine(_countdown());
    }

    private void Update()
    {

        // counts down until local time var is 0, then spawns object and
        // resets local time for next spawn
        timeIntervalLOCAL -= Time.deltaTime;
        if (timeIntervalLOCAL <= 0)
        {
            SpawnObject();
            SetRandTime();
        }
    }

    #endregion

    #region Gameplay

    [Header("Gameplay")]
    [SerializeField] private int questionsCorrect = 0; // questions player gets correct, starts at 0
    [SerializeField] private GameObject[] questions;
    [SerializeField] private GameObject finishLine;
    [SerializeField] private Transform finishLineSpawnPoint;
    [SerializeField] private TextMeshProUGUI correctText;
    [SerializeField] private GameObject gameCompletedScreen;
    [SerializeField] private TextMeshProUGUI repPointsText;
    [SerializeField] private int pointsPerQuestion;
    [SerializeField] private RectTransform checkXSpawnPos;
    [SerializeField] private GameObject check;
    [SerializeField] private GameObject x;

    // private vars hidden in inspector
    private int repPoints = 0;
    private bool correct;
    private bool hasChosen = false;
    private int questionsSeen = 0;


    // activates a random question from the array
    private void Question()
    {
        int rand = Random.Range(0, questions.Length); // random number
        for (int i = 0; i < questions.Length; i++)
        {
            if(i == rand)
            {
                questions[i].SetActive(true);
            }
        }
        questionsSeen++; // increase the count of questions encountered
    }

    // deactivates all the active questions in the array
    private void DeactivateAllQuestions()
    {
        foreach (GameObject question in questions)
        {
            question.SetActive(false);
        }
    }

    // bind to the correct option
    public void CorrectAnswer()
    {
        // spawn in check
        Instantiate(check, checkXSpawnPos.position, checkXSpawnPos.rotation, checkXSpawnPos);
        hasChosen = true;
        correct = true;
    }

    // bind to the incorrect option
    public void IncorrectAnswer()
    {
        // spawn in X
        Instantiate(x, checkXSpawnPos.position, checkXSpawnPos.rotation, checkXSpawnPos);
        hasChosen = true;
        correct = false;
    }

    public IEnumerator IQuestion()
    {
        Question();

        // waits until hasChosen is true
        yield return new WaitUntil(() => hasChosen == true);

        // TODO verification of correct answer
        // all the others turn red, and one turns green
        if (correct)
        {
            // TODO anims for get right
            // object explodes in a green mess

            questionsCorrect++; // increase number of questions 
            repPoints += pointsPerQuestion;
            Time.timeScale = 1f; // revert out of slow-mo
            Debug.Log("Correct!");
        }
        else
        {
            // TODO anims for get wrong
            // ui will show as incorrect but the block will still hit the person
            Time.timeScale = 1f; // revert out of slow-mo
            Debug.Log("Incorrect.");
            
        }

        DeactivateAllQuestions(); // deactivate all the currently active questions

        // reset booleans
        correct = false;
        hasChosen = false;

        // if the number of questions the player has seen equals the max amount of questions,
        // spawn in the finish line to finish the game
        if(questionsSeen == questions.Length)
        {
            obstacleSpawningStopped = true; // stops the flow of the obstacles
            Instantiate(finishLine, finishLineSpawnPoint.position, finishLineSpawnPoint.rotation, finishLineSpawnPoint);
        }
    }
    
    // called from the finish line controller script
    public void GameCompleted()
    {
        // set values of text objects
        correctText.text = questionsCorrect.ToString();
        repPointsText.text = repPoints.ToString();
        
        // add rep points to playerprefs
        PlayerPrefs.SetInt("Rep Points", PlayerPrefs.GetInt("Rep Points") + repPoints);
        gameCompletedScreen.SetActive(true); // show game completed screen
    }

    public void ExitAndReturn()
    {
        // leave game for world scene
        SceneManager.LoadScene("World");
    }

    #endregion


    #region ObstacleSpawner

    // SPAWN IN OBJECTS

    [Header("Obstacle Spawning")]
    [SerializeField] private GameObject obstacle;
    [SerializeField] private Transform obstacleSpawnLocation;
    [SerializeField] private float timeIntervalMIN;
    [SerializeField] private float timeIntervalMAX;
    private float timeIntervalLOCAL;
    private bool obstacleSpawningStopped = false;

    private void SpawnObject()
    {
        if (!obstacleSpawningStopped)
        {
            Instantiate(obstacle, obstacleSpawnLocation.position, obstacleSpawnLocation.rotation, obstacleSpawnLocation);
        }
    }

    // reset local time to random value (within restraints)
    private void SetRandTime()
    {
        timeIntervalLOCAL = Random.Range(timeIntervalMIN, timeIntervalMAX);
    }

    #endregion

}
