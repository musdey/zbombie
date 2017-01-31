using UnityEngine;
using System.Collections;
using System;


public class ZombieMovementScript : MonoBehaviour {

    private Animator animator;
    private Transform cameraPosition;
    public float speed = 5.0f;
    private Animation walkAnimation;
    public bool isDead = false;
    public bool isDying = false;
    public int ZombieID;
    private Camera cam;
    private Vector3 camPositionWithoutY;


    private bool playerIsAlive = true;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        cam = GameObject.Find("myCamera").GetComponent<Camera>();
        camPositionWithoutY = new Vector3(cam.transform.position.x, 0, cam.transform.position.z);
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Zombie number " + ZombieID + " was hit dramatically");
        isDead = true;
    }

     public void colleteralDamage()
    {
        isDead = true;
    }

    public void setZombieID(int _id)
    {
        ZombieID = _id;
    }

    IEnumerator DeleteGameObjectAfterDeath()
    {
        
        yield return new WaitForSeconds(0.8f); //this will wait 5 seconds
        Destroy(this.transform.root.gameObject);

    }

    // Update is called once per frame
    void Update () {

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("walk"))
        {
            float step = speed * Time.deltaTime;

            if(transform.position.z < -38)
            {

                //animator.SetBool("shouldAttack", true);

                GameScript.isPlayerAlive = false;

                //TODO: DIE HERE
                //SceneManager.UnloadScene("GameScene");
                //SceneManager.LoadScene("MenuScene1");

            }
            else
            {            
                transform.position = Vector3.MoveTowards(transform.position, camPositionWithoutY, Time.deltaTime);
                //transform.Translate(Vector3.forward * 1 * Time.deltaTime );
            }
        }
        if (isDead && !isDying)
        {
            System.Random rand = new System.Random();
            
            //int rNumber = rand.Next(0, 2);
            animator.SetInteger("shouldDie", 0);
            StartCoroutine(DeleteGameObjectAfterDeath()); //this will run your timer

            GameScript.points += 1;
            isDying = true;
        }

	}
}
