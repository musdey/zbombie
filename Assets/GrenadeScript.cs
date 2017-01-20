using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrenadeScript : MonoBehaviour {

    Vector3 mousePos;
    Vector3 wantedPos;
    Ray mouseRay;
    private bool grenadeIsHold;
    Transform standardTransform;
    float depth = 0.8f;
    private GameObject grenade;

    Vector3 startingMousePos;
    Vector3 endingMousePos;
    float movementTime;

    private float length = 0;
    private bool SW = false;
    private Vector3 final;
    private Vector3 startpos;
    private Vector3 endpos;
    private float explosionRadius;

    private Rigidbody grenadeRigidBody;
    public ParticleSystem particleSystem;
    private AudioSource exlposionAudio;

    void Awake()
    {
        
    }
    // Use this for initialization
    void Start () {
        grenadeIsHold = false;
        standardTransform = this.transform;
        Debug.Log("position is " + standardTransform.position);

        grenadeRigidBody = this.GetComponent<Rigidbody>();
        exlposionAudio = this.GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        
        Debug.Log("oncollision enter called");
        ParticleSystem ps = (ParticleSystem) Instantiate(particleSystem, this.transform.position, particleSystem.transform.rotation);
        ps.Play();
        exlposionAudio.Play();

        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, explosionRadius);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            Debug.Log(hitColliders[i].name);
            //hitColliders[i].SendMessage("die");

        }
            // Debug-draw all contact points and normals
            foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }

    }

    

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.UpArrow)) // Throw grenade test 
        {
            Debug.Log("Up wurde gedrückt");
            grenadeRigidBody.useGravity = true;
            grenadeRigidBody.velocity = new Vector3(0, 5, 5);
            grenadeRigidBody.AddForce(transform.forward / 2);
        }

        RaycastHit result;
        mousePos = Input.mousePosition;
        
        if (Input.GetMouseButtonDown(0)) {
            
            mouseRay = Camera.main.ScreenPointToRay(mousePos);

            if(Physics.Raycast(mouseRay, out result)) {

                if(result.transform.gameObject.name == "grenade")
                {
                    grenadeIsHold = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0)){
            grenadeIsHold = false;
            this.transform.position = new Vector3(standardTransform.position.x, standardTransform.position.y, standardTransform.position.z);
            Debug.Log("position is now " + this.transform.position);         
        }

        if (grenadeIsHold){

            if(startingMousePos.y > mousePos.y)
            {
                startingMousePos.y = mousePos.y;
            }

            Vector3 local = new Vector3(mousePos.x-1.0f, mousePos.y, depth);
            wantedPos = Camera.main.ScreenToWorldPoint(local);
            this.transform.position = wantedPos;
        }
        
        //--------------------------------------------------------------------------------------->
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            final = Vector3.zero;
            length = 0;
            SW = false;
            Vector2 touchDeltaPosition = Input.GetTouch(0).position;
            startpos = new Vector3(touchDeltaPosition.x, 0, touchDeltaPosition.y);
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            SW = true;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Canceled)
        {
            SW = false;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary)
        {
            SW = false;
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            if (SW)
            {
                Vector2 touchPosition = Input.GetTouch(0).position;
                endpos = new Vector3(touchPosition.x, 0, touchPosition.y);
                final = endpos - startpos;
                length = final.magnitude;
                Debug.Log("result of something is" + length);
            }
        }
    }

    /*void OnGUI()
    {
        GUI.Box(new Rect(50, 300, 500, 30), "length: " + length);
    }*/
    //---------------------------------------------------------------------------------------------->

}




