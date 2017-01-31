using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour {

    public static int zombieCounter = 0;
    public static int points = 0;
    public static bool isPlayerAlive = true;
    public static bool isCombo = false;

    //Zombie
    public Vector3 zombieStartRootPosition;
    public GameObject enemy;
    //private ArrayList enemiesArray;
    private Quaternion rotation;

    //Grenade
    public GameObject grenade;
    public static GameObject actualGrenade;

    //Game
    public AudioSource backGroundMusic;

    //Cam
    private Camera cam;

    //Gameplay
    private int round = 8;       // Zombies that are generated in one round
    private int roundTime = 10; //seconds
    private int actualSecond = 0;
    private System.Random rand;
    private ArrayList list;
    private Boolean randCalculated = false;

    public Text pointsTextField;
    public Text gameOverPointsTextView;
    public GameObject comboView;
    public GameObject gameOverScreen;

    void Start () {

        getHighScore();
        cam = GameObject.Find("myCamera").GetComponent<Camera>();
        backGroundMusic.loop = true;
        backGroundMusic.Play();
        rotation = new Quaternion(0, 180, 0, 0);

        actualGrenade = Instantiate(grenade);
        list = new ArrayList();
        StartCoroutine(checkAndCreateZombie());
        comboView.SetActive(false);
        gameOverScreen.SetActive(false);

    }

    void saveHighscore(int highscore)
    {
        PlayerPrefs.SetInt("highscore", highscore);
    }
    //geting playerPrefs
    int getHighScore()
    {
        int highscore = PlayerPrefs.GetInt("highscore");
        //Debug.Log("highscore is" + highscore);
        return highscore;
    }

    private IEnumerator checkAndCreateZombie()
    {

        while (true){

            if (!randCalculated){
          
                rand = new System.Random();

                for(int i = 0; i< round; i++) {
                    list.Add(rand.Next(0, roundTime));
                }

                list.Sort();
                randCalculated = true;
                actualSecond++;
                //Debug.Log(actualSecond);
            }

            if(actualSecond <= roundTime) {

                if (list.Contains(actualSecond))
                {
                    System.Random milliRand = new System.Random();
                    float time = (float)milliRand.NextDouble();
                    Invoke("createZombie", time);
                }

                actualSecond++;

            }else{
                actualSecond = 0;
                round++;
                list.Clear();
                randCalculated = false;
                Debug.Log("This is Round " + round);
            }

            yield return new WaitForSeconds(1);
        }
    }
	
    private void createZombie()
    {
        GameObject createdEnemy = Instantiate(enemy, new Vector3(randomPosition(), zombieStartRootPosition.y, zombieStartRootPosition.z), rotation) as GameObject;
        createdEnemy.transform.LookAt(cam.transform.position);
        ZombieMovementScript zombiescript = createdEnemy.GetComponent<ZombieMovementScript>();
        zombiescript.setZombieID(zombieCounter++);

    }
	// Update is called once per frame
	void Update () {

        pointsTextField.text = "" + points;
            
        if (!isPlayerAlive)
        {
            stopGame();
        }

        if(actualGrenade == null)
        {
            actualGrenade = Instantiate(grenade);
        }

        if (isCombo)
        {
            isCombo = false;
            StartCoroutine(ShowComboImage());
        }

    }

    IEnumerator ShowComboImage()
    {

        comboView.SetActive(true);
        yield return new WaitForSeconds(3); //this will wait 5 seconds
        comboView.SetActive(false);
    }

    public void stopGame()
    {
        if (!gameOverScreen.active)
        {

        Time.timeScale = 0;
        int actualHighScore = getHighScore();
        if(actualHighScore < points)
        {
            saveHighscore(points);
        }

        gameOverPointsTextView.text = points +"";
        gameOverScreen.SetActive(true);

        Debug.Log("STOP GAME CALLED");


        }


    }

    public void restartGame()
    {
        
        gameOverScreen.SetActive(false);
        isPlayerAlive = true;
        points = 0;
        //SceneManager.UnloadScene("GameScene");
        
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("GameScene");
    }


    private int randomPosition()
    {
        System.Random rand = new System.Random();
        int n = rand.Next(-8, 8);
        return n;
    }
}
