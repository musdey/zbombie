using UnityEngine;
using System.Collections;
using System;

public class GameScript : MonoBehaviour {

    public static int zombieCounter = 0;
    //Zombie
    public Vector3 zombieStartRootPosition;
    public GameObject enemy;
    private ArrayList enemiesArray;
    private Quaternion rotation;

    //Grenade
    public GameObject grenade;
    public static GameObject actualGrenade;

    //Game
    public AudioSource backGroundMusic;


    void Start () {
        enemiesArray = new ArrayList();
        enemiesArray.Add(enemy);
        backGroundMusic.loop = true;
        backGroundMusic.Play();
        rotation = new Quaternion(0, 180, 0, 0);

        actualGrenade = Instantiate(grenade);
        
        
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {

            GameObject createdEnemy = Instantiate(enemy, new Vector3(randomPosition(),zombieStartRootPosition.y,zombieStartRootPosition.z), rotation) as GameObject;
            ZombieMovementScript zombiescript = createdEnemy.GetComponent<ZombieMovementScript>();
            zombiescript.setZombieID(zombieCounter++);

            enemiesArray.Add(createdEnemy);
            Debug.Log("elements in array " + enemiesArray.Count);
        }

    }

    private int randomPosition()
    {
        System.Random rand = new System.Random();
        int n = rand.Next(-5, 5);
        return n;
    }
}
