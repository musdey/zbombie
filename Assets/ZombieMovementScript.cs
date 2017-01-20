using UnityEngine;
using System.Collections;
using System;


public class ZombieMovementScript : MonoBehaviour {

    private Animator animator;
    private Transform cameraPosition;
    public float speed = 5.0f;
    private Animation walkAnimation;
    public bool isDead = false;
    public int ZombieID;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Zombie number " + ZombieID + " was hit dramatically");
        isDead = true;
    }

    public void setZombieID(int _id)
    {
        ZombieID = _id;
    }

    // Update is called once per frame
    void Update () {

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("walk"))
        {
            float step = speed * Time.deltaTime;

            if(transform.position.z < -38)
            {
                animator.SetBool("shouldAttack", true);

            }
            else
            {
                transform.Translate(Vector3.forward * 1 * Time.deltaTime );
            }
        }
        if (isDead)
        {
            System.Random rand = new System.Random();
            int rNumber = rand.Next(0, 2);
            animator.SetInteger("shouldDie", rNumber);
        }

	}
}
